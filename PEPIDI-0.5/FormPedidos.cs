using PEPIDI.FormsSecundarios;
using PEPIDI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace PEPIDI
{
    public partial class FormPedidos : Form
    {
        private int _nrFunc;
        private Form _loginForm;
        private MostraRoupa _mr;
        EfeitoUI M = new EfeitoUI();
        private enum ModoPedidos
        {
            Pedido,
            Devolucao
        }
        private ModoPedidos _modoAtual = ModoPedidos.Pedido;

        // ----------------- CLASSES DE SESSÃO -----------------

        private class LinhaSessao
        {
            public int IdEpiOriginal { get; set; }   // IDEPI que veio da SP
            public string Modelo { get; set; }
            public string Tamanho { get; set; }
            public int Quantidade { get; set; }
        }

        private class SessaoPedido
        {
            public int? IdPedidoReg { get; set; }                                   // ID em PedidoRegistos
            public List<LinhaSessao> Pedido { get; } = new List<LinhaSessao>();     // Itens só para PedidoPacote
            public List<LinhaSessao> Devolucao { get; } = new List<LinhaSessao>();  // Itens só para RoupaPacote

        }

        private SessaoPedido _sessao = new SessaoPedido();

        public FormPedidos(int nrFunc, Form loginForm)
        {
            InitializeComponent();
            _nrFunc = nrFunc;
            _loginForm = loginForm;
        }

        private void FormPedidos_Load(object sender, EventArgs e)
        {
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                return;

            if (_nrFunc == 0)
                return;

            try
            {
                if (_mr == null)
                    _mr = new MostraRoupa();

                if (_sessao == null)
                    _sessao = new SessaoPedido();

                AtualizarNavButtons();
                CarregarLinhas();

                var info = Details.GetInfoGestor(_nrFunc);
                lblNome.Text = info.Nome + " · " + info.Funcao;
            }
            catch (Exception ex)
            {
                M.AbrirMensagem(
                    "Erro em Pedidos_Load / CarregarLinhas:\n\n" + ex,
                    "Erro");
                    
            }
        }

        private void CarregarLinhas()
        {
            if (_mr == null)
                return;

            if (_sessao == null)
                _sessao = new SessaoPedido();

            List<LinhaPedidoInfo> itens;

            // Escolhe a fonte de dados conforme o modo atual
            if (_modoAtual == ModoPedidos.Pedido)
            {
                itens = _mr.ObterRoupaPorFuncionarioNovo(_nrFunc)
                        ?? new List<LinhaPedidoInfo>();
            }
            else // Devolução
            {
                itens = _mr.ObterRoupaUsadaPorFuncionarioNovo(_nrFunc)
                        ?? new List<LinhaPedidoInfo>();
            }

            if (flpLinhas == null)
                return;

            flpLinhas.Controls.Clear();

            var estadoLista = (_modoAtual == ModoPedidos.Pedido)
                ? _sessao.Pedido
                : _sessao.Devolucao;

            foreach (var item in itens)
            {
                if (item == null) continue;

                var linha = new PEPIDI.Organizers.LinhaPedido
                {
                    Size = new Size(flpLinhas.Width - 20, 100),
                    Dock = DockStyle.Top,
                    Margin = new Padding(0, 0, 0, 5),
                    Tag = item
                };

                linha.DescricaoEPI = item.Modelo ?? "";

                var combo = linha.ComboTamanho;
                if (combo != null)
                {
                    combo.Items.Clear();

                    var tamanhos = item.TamanhosDisponiveis ?? new List<string>();
                    foreach (var t in tamanhos)
                        combo.Items.Add(t);

                    // Ver se já temos estado guardado para este modelo
                    var estadoLinha = estadoLista.FirstOrDefault(x => x.Modelo == item.Modelo);

                    if (estadoLinha != null)
                    {
                        // Recupera tamanho e quantidade alterados pelo utilizador
                        if (!string.IsNullOrEmpty(estadoLinha.Tamanho) &&
                            combo.Items.Contains(estadoLinha.Tamanho))
                        {
                            combo.SelectedItem = estadoLinha.Tamanho;
                        }
                        else if (!string.IsNullOrEmpty(item.TamanhoAtual) &&
                                 combo.Items.Contains(item.TamanhoAtual))
                        {
                            combo.SelectedItem = item.TamanhoAtual;
                        }
                        else if (combo.Items.Count > 0)
                        {
                            combo.SelectedIndex = 0;
                        }

                        linha.Quantidade = estadoLinha.Quantidade;
                    }
                    else
                    {
                        // Sem estado guardado, usar tamanho atual / default
                        if (!string.IsNullOrEmpty(item.TamanhoAtual) &&
                            combo.Items.Contains(item.TamanhoAtual))
                        {
                            combo.SelectedItem = item.TamanhoAtual;
                        }
                        else if (combo.Items.Count > 0)
                        {
                            combo.SelectedIndex = 0;
                        }

                        linha.Quantidade = 0;
                    }
                }
                else
                {
                    linha.Quantidade = 0;
                }

                flpLinhas.Controls.Add(linha);
            }
        }

        private void AtualizarNavButtons()
        {
            Nav1.Checked = _modoAtual == ModoPedidos.Pedido;
            Nav2.Checked = _modoAtual == ModoPedidos.Devolucao;
        }

        private void NavButtons_Click(object sender, EventArgs e)
        {
            // 1. Cast para Guna2Button ou CheckBox (conforme o que usas no Designer)
            var btnClicado = sender as Guna.UI2.WinForms.Guna2Button; // Ajusta se for outro tipo

            // 2. Se o botão já estiver selecionado ou não for Nav1/Nav2, ignora ou fecha
            if (btnClicado == null || (btnClicado != Nav1 && btnClicado != Nav2))
            {
                if (sender != Nav1 && sender != Nav2) Close();
                return;
            }

            // 3. Definir o modo alvo baseado no botão clicado
            ModoPedidos modoAlvo = (btnClicado == Nav1) ? ModoPedidos.Pedido : ModoPedidos.Devolucao;

            // 4. EVITAR DUPLICADO: Se já estamos nesse modo, não fazemos nada
            if (modoAlvo == _modoAtual) return;

            // 5. Validação específica para Devolução
            if (modoAlvo == ModoPedidos.Devolucao)
            {
                var itensUsados = _mr.ObterRoupaUsadaPorFuncionarioNovo(_nrFunc);
                if (itensUsados == null || itensUsados.Count == 0)
                {
                    // Mostra a mensagem APENAS UMA VEZ
                    M.AbrirMensagem("Nenhum produto usado encontrado para o funcionário.",
                                    "Aviso");

                    // Reverte o estado visual sem disparar o clique novamente
                    Nav1.Checked = true;
                    Nav2.Checked = false;
                    return;
                }
            }

            // 6. Se passou a validação, troca o modo
            GuardarEstadoModoAtual();
            _modoAtual = modoAlvo;

            // Atualiza os botões (garante que apenas o correto fica Checked)
            AtualizarNavButtons();
            CarregarLinhas();
        }

        private void GuardarEstadoModoAtual()
        {
            if (flpLinhas == null || _sessao == null)
                return;

            var lista = (_modoAtual == ModoPedidos.Pedido)
                ? _sessao.Pedido
                : _sessao.Devolucao;

            lista.Clear();

            foreach (var linha in flpLinhas.Controls.OfType<PEPIDI.Organizers.LinhaPedido>())
            {
                var info = linha.Tag as LinhaPedidoInfo;
                if (info == null) continue;

                string tamanho = linha.ComboTamanho?.SelectedItem?.ToString();
                int qtd = linha.Quantidade;

                if (string.IsNullOrWhiteSpace(tamanho) || qtd <= 0)
                    continue;

                lista.Add(new LinhaSessao
                {
                    IdEpiOriginal = info.IdEpi,
                    Modelo = info.Modelo ?? "",
                    Tamanho = tamanho,
                    Quantidade = qtd
                });
            }
        }

        private void btnSubmeter_Click(object sender, EventArgs e)
        {
            try
            {
                if (_mr == null)
                    _mr = new MostraRoupa();
                if (_sessao == null)
                    _sessao = new SessaoPedido();

                // Garante que o que está no modo visível também é guardado
                GuardarEstadoModoAtual();

                var itensPedido = _sessao.Pedido.ToList();
                var itensDevolucao = _sessao.Devolucao.ToList();

                if (!itensPedido.Any() && !itensDevolucao.Any())
                {
                    M.AbrirMensagem("Nenhum item com quantidade > 0 para submeter.", "Informação");
                    return;
                }

                // ----- Resolver IDs e validar devoluções -----

                var listaPedidoInsert = new List<(int idEpi, string tamanho, int qtd)>();
                foreach (var it in itensPedido)
                {
                    int idEpi = _mr.ResolveEpiIdPorModeloTamanho(it.Modelo, it.Tamanho);
                    if (idEpi <= 0)
                    {
                        M.AbrirMensagem($"Não encontrei EPI para {it.Modelo} - {it.Tamanho}. Operação cancelada.", "Erro");
                        return;
                    }

                    listaPedidoInsert.Add((idEpi, it.Tamanho, it.Quantidade));

                    // --- NOVA LINHA AQUI! ---
                    // Sempre que pedir um artigo, atualiza o perfil dele com este tamanho!
                    _mr.AtualizarTamanhoPadraoFuncionario(_nrFunc, idEpi, it.Tamanho);
                }

                var listaDevInsert = new List<(int idRoupa, string tamanho, int qtd)>();
                foreach (var it in itensDevolucao)
                {
                    int maxDevolver = _mr.GetConsumidoDisponivel(_nrFunc, it.IdEpiOriginal);
                    if (it.Quantidade > maxDevolver)
                    {
                        M.AbrirMensagem(
                            $"{it.Modelo} {it.Tamanho}: a devolver {it.Quantidade} excede o usado ({maxDevolver}).", "Erro");
                        return;
                    }

                    int idRoupa = _mr.ResolveRoupaIdPorModeloTamanho(it.Modelo, it.Tamanho);
                    if (idRoupa <= 0)
                    {
                        M.AbrirMensagem($"Não encontrei Roupa para {it.Modelo} - {it.Tamanho}. Operação cancelada.", "Erro");
                        return;
                    }

                    listaDevInsert.Add((idRoupa, it.Tamanho, it.Quantidade));
                }

                // ----- Ir buscar/criar PedidoRegistos pendente -----

                int idPedReg;
                if (_sessao.IdPedidoReg.HasValue)
                {
                    idPedReg = _sessao.IdPedidoReg.Value;
                }
                else
                {
                    idPedReg = _mr.GetOrCreatePedidoPendente(_nrFunc);
                    _sessao.IdPedidoReg = idPedReg;
                }

                // ----- Gravar itens nas duas tabelas com o MESMO IDPedReg -----

                if (listaPedidoInsert.Any())
                    _mr.SubmeterPedidoParaPedidoReg(idPedReg, listaPedidoInsert);

                if (listaDevInsert.Any())
                    _mr.SubmeterEntregaParaPedidoReg(idPedReg, listaDevInsert);

                M.AbrirMensagem("Pedido submetido com sucesso!", "Sucesso");

                this.Close();
            }
            catch (Exception ex)
            {
                M.AbrirMensagem("Erro ao submeter pedido:\n\n" + ex, "Erro");
            }
        }
    }
}
