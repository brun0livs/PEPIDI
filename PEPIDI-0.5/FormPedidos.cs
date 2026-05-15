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
            public string Modelo { get; set; }
            public string Tamanho { get; set; }
            public string Cor { get; set; }
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
            if (_mr == null) return;

            var itensRaw = (_modoAtual == ModoPedidos.Pedido)
                ? _mr.ObterRoupaPorFuncionarioNovo(_nrFunc)
                : _mr.ObterRoupaUsadaPorFuncionarioNovo(_nrFunc);

            if (flpLinhas == null) return;
            flpLinhas.SuspendLayout();
            flpLinhas.Controls.Clear();

            // Agrupamos por Família
            var grupos = itensRaw.GroupBy(x => x.Familia).ToList();

            foreach (var grupo in grupos)
            {
                // 1. Pergunta ao MostraRoupa qual foi o último modelo desta família (ex: Sapato)
                string ultimoModelo = _mr.ObterUltimoModeloConsumidoPorFamilia(_nrFunc, grupo.Key);

                // 2. Tenta encontrar esse modelo na lista de itens que o gajo pode pedir
                // Se não encontrar (ex: é o primeiro pedido da vida dele), usa o .First()
                var itemPrincipal = grupo.FirstOrDefault(x => x.Modelo == ultimoModelo) ?? grupo.First();

                var listaModelos = grupo.Select(x => x.Modelo).Distinct().ToList();
                bool variosModelos = listaModelos.Count > 1;

                var linha = new PEPIDI.Organizers.LinhaPedido
                {
                    Size = new Size(flpLinhas.Width - 25, 100),
                    Dock = DockStyle.Top,
                    Tag = itemPrincipal
                };

                // -------------------------------------------------------------------
                // SUBSCREVER O EVENTO QUE ACABÁMOS DE CRIAR
                linha.TamanhoAlteradoPeloUtilizador += Linha_TamanhoAlteradoPeloUtilizador;
                // -------------------------------------------------------------------

                // 3. Configura o Layout (Elastic) e o Nome Bonito
                linha.ConfigurarLayout(variosModelos, itemPrincipal.Familia, itemPrincipal.Modelo);

                if (variosModelos)
                {
                    linha.ComboModelo.Items.Clear();
                    foreach (var mod in listaModelos) linha.ComboModelo.Items.Add(mod);

                    // FORÇA a seleção do modelo que o histórico indicou
                    linha.ComboModelo.SelectedItem = itemPrincipal.Modelo;
                }

                // 4. Carrega os tamanhos baseados no modelo histórico encontrado
                var tamanhos = itemPrincipal.TamanhosDisponiveis ?? new List<string>();
                if (!string.IsNullOrEmpty(itemPrincipal.TamanhoAtual) && !tamanhos.Contains(itemPrincipal.TamanhoAtual))
                    tamanhos.Add(itemPrincipal.TamanhoAtual);

                linha.ComboTamanho.Items.Clear();
                foreach (var t in tamanhos.OrderBy(x => x)) linha.ComboTamanho.Items.Add(t);

                linha.DefinirTamanhoSemConfirmar(itemPrincipal.TamanhoAtual);
                flpLinhas.Controls.Add(linha);
            }
            flpLinhas.ResumeLayout(true);
        }

        private void Linha_TamanhoAlteradoPeloUtilizador(object sender, PEPIDI.Organizers.LinhaPedido.TamanhoAlteradoEventArgs e)
        {
            if (string.IsNullOrEmpty(e.CodigoEpi)) return;

            try
            {
                // Usa o método incrível que já tinhas preparado no MostraRoupa!
                _mr.AtualizarTamanhoPadraoFuncionario(_nrFunc, e.CodigoEpi, e.NovoTamanho);
            }
            catch (Exception ex)
            {
                M.AbrirMensagem("Erro ao atualizar o tamanho predefinido do funcionário:\n\n" + ex.Message, "Erro");
            }
        }

        // Método de mapeamento (ajusta as strings case seja necessário)
        private string MapearFamiliaParaColuna(string familia)
        {
            switch (familia.ToLower().Trim())
            {
                case "tshirt":
                case "t-shirt": return "TShirt";
                case "casaco": return "Casaco";
                case "polo manga curta": return "PoloMCurta"; // Ajusta para a string real da tua BD
                case "polo manga comprida": return "PoloMCompr"; // Ajusta para a string real da tua BD
                case "calça":
                case "calca":
                case "calças": return "Calca";
                case "sapato":
                case "sapatos": return "Sapato";
                default: return null;
            }
        }

        // Método auxiliar para não repetir código
        private void CarregarTamanhosDaLinha(PEPIDI.Organizers.LinhaPedido linha, LinhaPedidoInfo item)
        {
            var comboT = linha.ComboTamanho;
            comboT.Items.Clear();

            var listaTamanhos = item.TamanhosDisponiveis ?? new List<string>();
            if (!string.IsNullOrEmpty(item.TamanhoAtual) && !listaTamanhos.Contains(item.TamanhoAtual))
                listaTamanhos.Add(item.TamanhoAtual);

            foreach (var t in listaTamanhos.OrderBy(x => x))
                comboT.Items.Add(t);

            linha.DefinirTamanhoSemConfirmar(item.TamanhoAtual);
            comboT.Refresh();
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

                // AQUI: Se a combo de modelos estiver visível, usamos o que o user escolheu.
                // Caso contrário (ex: T-Shirt), usamos o modelo original da info.
                string modeloFinal = linha.ComboModelo.Visible
                                     ? linha.ComboModelo.SelectedItem?.ToString()
                                     : info.Modelo;

                string tamanho = linha.ComboTamanho?.SelectedItem?.ToString();
                int qtd = linha.Quantidade;

                if (string.IsNullOrWhiteSpace(tamanho) || string.IsNullOrWhiteSpace(modeloFinal) || qtd <= 0)
                    continue;

                lista.Add(new LinhaSessao
                {
                    // Importante: ResolveEpiIdPorModeloTamanho vai garantir o ID correto do Wurth
                    Modelo = modeloFinal,
                    Tamanho = tamanho,
                    Cor = info.Cor,
                    Quantidade = qtd
                });
            }
        }

        private void btnSubmeter_Click(object sender, EventArgs e)
        {
            try
            {
                if (_mr == null) _mr = new MostraRoupa();
                if (_sessao == null) _sessao = new SessaoPedido();

                // Guardar o que o utilizador selecionou no ecrã
                GuardarEstadoModoAtual();

                var itensPedido = _sessao.Pedido.ToList();
                var itensDevolucao = _sessao.Devolucao.ToList();

                if (!itensPedido.Any() && !itensDevolucao.Any())
                {
                    M.AbrirMensagem("Nenhum item com quantidade > 0 para submeter.", "Informação");
                    return;
                }

                // ----- 1. PREPARAR PEDIDOS (Usando Código em vez de ID) -----
                var listaPedidoInsert = new List<(string codigoEpi, int qtd)>();
                foreach (var it in itensPedido)
                {
                    string codigoEpi = _mr.ResolveCodigoEpi(it.Modelo, it.Tamanho, it.Cor);

                    if (string.IsNullOrEmpty(codigoEpi))
                    {
                        // Agora o erro vai dizer-te a cor e vai meter umas aspas para veres se há espaços invisíveis!
                        M.AbrirMensagem($"Erro: Não encontrei Código para Modelo: '{it.Modelo}' | Tam: '{it.Tamanho}' | Cor: '{it.Cor}'", "Erro");
                        return;
                    }

                    listaPedidoInsert.Add((codigoEpi, it.Quantidade));
                    // Opcional: _mr.AtualizarTamanhoPadraoFuncionario(_nrFunc, codigoEpi, it.Tamanho);
                }

                // ----- 2. PREPARAR DEVOLUÇÕES (Usando Código em vez de ID) -----
                var listaDevInsert = new List<(string codigoEpi, int qtd)>();
                foreach (var it in itensDevolucao)
                {
                    string codigoEpi = _mr.ResolveCodigoEpi(it.Modelo, it.Tamanho, it.Cor);

                    if (string.IsNullOrEmpty(codigoEpi))
                    {
                        M.AbrirMensagem($"Erro: Não encontrei Código para {it.Modelo} {it.Tamanho}.", "Erro");
                        return;
                    }

                    listaDevInsert.Add((codigoEpi, it.Quantidade));
                }

                // ----- 3. CRIAR/OBTER CABEÇALHO DO PEDIDO -----
                int idPedReg = _sessao.IdPedidoReg ?? _mr.GetOrCreatePedidoPendente(_nrFunc);
                _sessao.IdPedidoReg = idPedReg;

                // ----- 4. GRAVAR NA BD (INSERTS CEGOS) -----
                if (listaPedidoInsert.Any())
                    _mr.GravarPedidosCegos(idPedReg, listaPedidoInsert);

                if (listaDevInsert.Any())
                    _mr.GravarDevolucoesCegas(idPedReg, listaDevInsert);

                M.AbrirMensagem("Pedido submetido com sucesso!", "Sucesso");
                this.Close();
            }
            catch (Exception ex)
            {
                M.AbrirMensagem("Erro ao submeter pedido:\n\n" + ex.Message, "Erro SQL");
            }
        }

        private void NavD_Click(object sender, EventArgs e)
        {
            using (Form overlay = new Form())
            {
                // Configurar o formulário "sombra"
                overlay.StartPosition = FormStartPosition.CenterScreen;
                overlay.WindowState = FormWindowState.Maximized;
                overlay.FormBorderStyle = FormBorderStyle.None; // Sem bordas
                overlay.Opacity = 0.50d;                        // 50% transparente
                overlay.BackColor = Color.Black;                // Cor preta
                overlay.ShowInTaskbar = false;                  // Não aparece na barra de tarefas

                // Faz o overlay cobrir exatamente o formulário atual (this)
                overlay.Location = this.Location;
                overlay.Size = this.Size;

                // Mostra a sombra
                overlay.Show(this);
                using (FormNovaPasse frm = new FormNovaPasse(_nrFunc))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {

                    }
                }
            }
        }
    }
}
