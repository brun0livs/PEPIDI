using PEPIDI.FormsSecundarios;
using PEPIDI.Models;
using PEPIDI.Organizers;
using PEPIDI.UCs.DGVS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PEPIDI.UCs.UcsSecundarios
{
    public partial class PedidosDetalhes : UserControl, IMessageFilter
    {
        int ID;
        int IDGestor;
        string NomeGestor;
        string Estado;
        // CORREÇÃO: Inicializar com string vazia ou valores por defeito
        string NomeFuncionario = "Funcionário Desconhecido";
        string NMEC = "0000";
        string FuncaoFuncionario = "-";
        EfeitoUI M = new EfeitoUI();

        // Controlo de Fluxo para o MX Master 3S
        private DateTime lastScrollTime = DateTime.Now;
        private int scrollCount = 0;
        private const int MaxScrollMessages = 20;

        public PedidosDetalhes(int _ID, int _IDGestor, string _Estado)
        {
            InitializeComponent();
            ID = _ID;
            IDGestor = _IDGestor;
            Estado = _Estado;
        }

        private void PedidosDetalhes_Load(object sender, EventArgs e)
        {
            // Ativa o filtro global que impede a recursividade
            Application.AddMessageFilter(this);

            var info = Details.GetInfoGestor(IDGestor);
            NomeGestor = info.Nome;
            GereEstado(Estado);
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            // Remove o filtro ao fechar para evitar que a app tente dar scroll a um painel que já não existe
            Application.RemoveMessageFilter(this);
            base.OnHandleDestroyed(e);
        }

        // Este método captura o scroll antes de ele chegar aos botões e causar o StackOverflow
        public bool PreFilterMessage(ref Message m)
        {
            // WM_MOUSEWHEEL = 0x020A
            if (m.Msg == 0x020A && pnlScroll != null && pnlScroll.IsHandleCreated)
            {
                Point pos = PointToClient(Cursor.Position);

                if (this.ClientRectangle.Contains(pos))
                {
                    TimeSpan elapsed = DateTime.Now - lastScrollTime;
                    if (elapsed.TotalMilliseconds > 100)
                    {
                        scrollCount = 0;
                        lastScrollTime = DateTime.Now;
                    }

                    if (scrollCount < MaxScrollMessages)
                    {
                        scrollCount++;
                        // Envia o scroll diretamente para o pnlScroll sem passar pelos filhos
                        SendMessage(pnlScroll.Handle, m.Msg, m.WParam, m.LParam);
                    }

                    // "Mata" a mensagem original. Isto impede que o evento suba na hierarquia e crash o programa.
                    return true;
                }
            }
            return false;
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        private void GereEstado(string estado)
        {
            label1.Text = (estado == "Aprovado") ? "Selecionar" : "Quantidade";

            if (estado == "Pendente")
            {
                btnAprovar.Text = "Aprovar";
                btnReprovar.Enabled = true;
                lblQuantDisp.Text = "Quant. Disp";
            }
            else if (estado == "Aprovado")
            {
                btnAprovar.Text = "Finalizar";
                btnReprovar.Enabled = false;
                lblQuantDisp.Text = "Quantidade";
            }
            else
            {
                btnAprovar.Text = "Comprovativo";
                btnReprovar.Enabled = false;
            }

            CarregarPPacote(pnlConteudo, ID, estado, pnlScroll, tlpLinhas);
            VerComentario(ID);
        }

        private void CarregarDadosFuncionario(int idPedido)
        {
            using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
            {
                try
                {
                    conn.Open();
                    string sql = @"
                SELECT F.Nome, F.Nr, F.Funcao 
                FROM PedidoRegistos P
                INNER JOIN Funcionarios F ON P.IdFuncionario = F.Nr 
                WHERE P.ID = @ID";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", idPedido);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Dados encontrados
                                this.NomeFuncionario = reader["Nome"]?.ToString() ?? "Sem Nome";
                                this.NMEC = reader["Nr"]?.ToString() ?? "0000";
                                this.FuncaoFuncionario = reader["Funcao"]?.ToString() ?? "-";
                            }
                            else
                            {
                                // SE NÃO ENCONTRAR, DEFINIMOS VALORES PADRÃO PARA NÃO DAR ERRO
                                this.NomeFuncionario = "Erro ao ler Funcionário";
                                this.NMEC = "0000";
                                this.FuncaoFuncionario = "Erro";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log de erro (opcional) e valores seguros
                    this.NomeFuncionario = "Erro SQL";
                    this.NMEC = "0000";
                }
            }
        }

        public void CarregarPPacote(Panel pnlConteudo, int idPedido, string estado, Panel pnlScroll, TableLayoutPanel tlpLinhas)
        {
            tlpLinhas.Controls.Clear();
            tlpLinhas.RowCount = 0;
            tlpLinhas.RowStyles.Clear();

            pnlScroll.Size = tlpDesign.GetControlFromPosition(0, 1).Size;

            using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_DetalhesDoPedido", conn) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@IDPedido", idPedido);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    int idEpi = Convert.ToInt32(row["ID"]);
                    string modelo = row["Modelo"].ToString();
                    string tamanho = row["Tamanho"].ToString();
                    int quantDisp = Convert.ToInt32(row["QuantidadeDisponivel"]);
                    int quantPedida = 0;
                    if (dt.Columns.Contains("QuantidadePedida"))
                        quantPedida = Convert.ToInt32(row["QuantidadePedida"]);
                    else if (dt.Columns.Contains("Quantidade"))
                        quantPedida = Convert.ToInt32(row["Quantidade"]);
                    // Se o valor for nulo ou zero, forçamos a 1 para segurança
                    if (quantPedida == 0) quantPedida = 1;

                    LinhaItem novaLinha = new LinhaItem(modelo, tamanho, quantDisp, quantPedida, estado);
                    novaLinha.IDEPI = idEpi;
                    novaLinha.QuantidadeOriginal = quantPedida;
                    novaLinha.QuantidadeAlterada += Linha_QuantidadeAlterada;
                    novaLinha.Dock = DockStyle.Top;

                    tlpLinhas.RowCount++;
                    tlpLinhas.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                    tlpLinhas.Controls.Add(novaLinha, 0, tlpLinhas.RowCount - 1);
                }
            }
            tlpLinhas.Width = pnlScroll.Width - 5;
        }

        private void Linha_QuantidadeAlterada(object sender, EventArgs e)
        {
            if (sender is LinhaItem linha)
            {
                string logBusca = $"{linha.M} ({linha.T})";
                List<string> linhasLog = txtObs.Lines.ToList();

                // Limpa registos antigos para evitar duplicados
                linhasLog.RemoveAll(l => l.Contains(logBusca));

                if (linha.QuantidadeSelecionada != linha.QuantidadeOriginal)
                {
                    string autor = NomeGestor;
                    string notaExtra = "";

                    // Subverificação: Só o PEPIDI assume se o stock for 0
                    if (linha.QuantidadeSelecionada == 0 && linha.QD == 0)
                    {
                        autor = "PEPIDI";
                        notaExtra = " (falta de stock)";
                    }

                    string novaNota = $"[{autor}]: Alterou {logBusca} de {linha.QuantidadeOriginal} para {linha.QuantidadeSelecionada}{notaExtra}";
                    linhasLog.Add(novaNota);
                }
                txtObs.Lines = linhasLog.ToArray();
            }
        }

        // Outros métodos necessários
        private void VerComentario(int idPedido)
        {
            txtObs.Text = string.Empty;
            using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT Notas FROM PedidoRegistos WHERE ID = @ID", conn);
                cmd.Parameters.AddWithValue("@ID", idPedido);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read()) txtObs.Text = reader["Notas"].ToString();
                }
            }
        }

        private void close_Click(object sender, EventArgs e)
        {
            // Verifica se o controlo tem um pai (o painel onde o inseriste)
            if (this.Parent != null)
            {
                this.Parent.Controls.Remove(this);
                this.Dispose(); // Liberta a memória para não ter lag (mais para o meu rato)
            }
        }

        private void btnAprovar_Click(object sender, EventArgs e)
        {
            string estadoAtual = this.Estado.Trim();

            // ==============================================================================
            // 1. FASE DE APROVAÇÃO (Pendente -> Aprovado)
            // ==============================================================================
            if (estadoAtual.Equals("Pendente", StringComparison.OrdinalIgnoreCase))
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
                    {
                        conn.Open();
                        SqlTransaction trans = conn.BeginTransaction();

                        try
                        {
                            // 1. Atualizar as quantidades que o Gestor decidiu na Combo
                            foreach (Control c in tlpLinhas.Controls)
                            {
                                if (c is LinhaItem linha)
                                {
                                    SqlCommand cmdItem = new SqlCommand("sp_AtualizarQuantidadePedidoPacote", conn, trans);
                                    cmdItem.CommandType = CommandType.StoredProcedure;
                                    cmdItem.Parameters.AddWithValue("@IDPedido", this.ID);
                                    cmdItem.Parameters.AddWithValue("@IDEPI", linha.IDEPI);
                                    cmdItem.Parameters.AddWithValue("@NovaQuantidade", linha.QuantidadeSelecionada);
                                    cmdItem.ExecuteNonQuery();
                                }
                            }

                            // 2. Mudar estado para 'Aprovado'
                            string sql = @"UPDATE PedidoRegistos 
                                   SET Estado = 'Aprovado', Aprovacao = @Gestor, AlteracaoData = GETDATE() 
                                   WHERE ID = @ID";

                            using (SqlCommand cmd = new SqlCommand(sql, conn, trans))
                            {
                                cmd.Parameters.AddWithValue("@ID", this.ID);
                                cmd.Parameters.AddWithValue("@Gestor", IDGestor);
                                cmd.ExecuteNonQuery();
                            }

                            trans.Commit();
                            M.AbrirMensagem("Pedido Aprovado! Agora está pronto para entrega.", "Sucesso");

                            this.Parent.Controls.Remove(this);
                            this.Dispose();
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            throw ex;
                        }
                    }
                }
                catch (Exception ex)
                {
                    M.AbrirMensagem("Erro ao aprovar: " + ex.Message, "Erro");
                }
            }
            // ==============================================================================
            // 2. FASE DE FINALIZAÇÃO (Aprovado -> Concluido + PDF + Stock)
            // ==============================================================================
            else if (estadoAtual.Equals("Aprovado", StringComparison.OrdinalIgnoreCase))
            {
                // --- SEGURANÇA: Obriga o programa a carregar o funcionário SEMPRE (evita o "Desconhecido") ---
                CarregarDadosFuncionario(this.ID);

                // 1. Recolher itens selecionados para entrega
                var listaReceber = new List<(int ID, string Artigo, string Tamanho, int Qtd)>();

                foreach (Control c in tlpLinhas.Controls)
                {
                    if (c is LinhaItem linha)
                    {
                        if (linha.Selecionado && linha.QuantidadeSelecionada > 0)
                        {
                            listaReceber.Add((linha.IDEPI, linha.DescricaoArtigo, linha.TamanhoSelecionado, linha.QuantidadeSelecionada));
                        }
                    }
                }

                if (listaReceber.Count == 0)
                {
                    M.AbrirMensagem("Selecione pelo menos um item para entregar.", "Aviso");
                    return;
                }

                // 2. Abrir Form de Assinatura
                using (var frm = new FormAssinatura(NomeFuncionario))
                {
                    foreach (var item in listaReceber) frm.AdicionarItemReceber(item.Artigo, item.Tamanho, item.Qtd);

                    Form overlay = new Form { BackColor = Color.Black, Opacity = 0.5, ShowInTaskbar = false, FormBorderStyle = FormBorderStyle.None, WindowState = FormWindowState.Maximized };
                    overlay.Show();
                    var result = frm.ShowDialog(overlay);
                    overlay.Close();

                    if (result != DialogResult.OK) return;

                    try
                    {
                        // A. Gerar PDF (AGORA COM O NOME DO GESTOR INCLUÍDO NA CHAMADA)
                        string caminhoPDF = PEPIDI.Organizers.PDFGenerator.GerarComprovativo(
                            this.ID, NomeFuncionario, NMEC, FuncaoFuncionario, NomeGestor,
                            listaReceber, new List<(int ID, string Artigo, string Tamanho, int Qtd)>(),
                            frm.AssinaturaFinal
                        );
                        // B. Transação SQL
                        using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
                        {
                            conn.Open();
                            SqlTransaction trans = conn.BeginTransaction();

                            try
                            {
                                // Atualiza Estado e PDF
                                string sqlUpdate = @"UPDATE PedidoRegistos 
                                             SET Estado = 'Concluido', PDF = @PDF, AlteracaoData = GETDATE() 
                                             WHERE ID = @ID";

                                using (SqlCommand cmd = new SqlCommand(sqlUpdate, conn, trans))
                                {
                                    cmd.Parameters.AddWithValue("@ID", this.ID);
                                    cmd.Parameters.AddWithValue("@PDF", caminhoPDF);
                                    cmd.ExecuteNonQuery();
                                }

                                // Abate de Stock
                                foreach (Control c in tlpLinhas.Controls)
                                {
                                    if (c is LinhaItem linha)
                                    {
                                        // Se tem check, usa a quantidade. Se não, é 0.
                                        int qtdReal = (linha.Selecionado) ? linha.QuantidadeSelecionada : 0;

                                        // 1. Grava no PedidoPacote a quantidade real entregue
                                        SqlCommand cmdItem = new SqlCommand("sp_AtualizarQuantidadePedidoPacote", conn, trans);
                                        cmdItem.CommandType = CommandType.StoredProcedure;
                                        cmdItem.Parameters.AddWithValue("@IDPedido", this.ID);
                                        cmdItem.Parameters.AddWithValue("@IDEPI", linha.IDEPI);
                                        cmdItem.Parameters.AddWithValue("@NovaQuantidade", qtdReal);
                                        cmdItem.ExecuteNonQuery();

                                        // 2. Desconta na tabela EPI (Stock) se entregou algo
                                        if (qtdReal > 0)
                                        {
                                            string sqlStock = "UPDATE EPI SET Quantidade = Quantidade - @Qtd WHERE ID = @IDEPI";
                                            using (SqlCommand cmdStock = new SqlCommand(sqlStock, conn, trans))
                                            {
                                                cmdStock.Parameters.AddWithValue("@Qtd", qtdReal);
                                                cmdStock.Parameters.AddWithValue("@IDEPI", linha.IDEPI);
                                                cmdStock.ExecuteNonQuery();
                                            }
                                        }
                                    }
                                }
                                trans.Commit();
                                M.AbrirMensagem("Entrega finalizada com sucesso!\nPDF Gerado.", "Sucesso");
                                try { System.Diagnostics.Process.Start("explorer.exe", caminhoPDF); } catch { }
                                this.Parent.Controls.Remove(this);
                                this.Dispose();
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                throw ex;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        M.AbrirMensagem("Erro ao finalizar: " + ex.Message, "Erro Crítico");
                    }
                }
            }

            // ==============================================================================
            // 3. CONSULTA (Concluido -> Ver PDF)
            // ==============================================================================
            else
            {
                AbrirComprovativoExistente();
            }
        }
        private void AbrirComprovativoExistente()
        {
            try
            {
                string caminhoPDF = "";

                using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
                {
                    conn.Open();
                    // Vai buscar o caminho que guardaste na coluna PDF
                    string sql = "SELECT PDF FROM PedidoRegistos WHERE ID = @ID";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", this.ID);
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            caminhoPDF = result.ToString();
                        }
                    }
                }

                if (!string.IsNullOrEmpty(caminhoPDF) && System.IO.File.Exists(caminhoPDF))
                {
                    // Abre o PDF com o visualizador padrão do Windows
                    System.Diagnostics.Process.Start("explorer.exe", caminhoPDF);
                }
                else
                {
                    M.AbrirMensagem("O ficheiro do comprovativo não foi encontrado ou não existe.", "Erro de Ficheiro");
                }
            }
            catch (Exception ex)
            {
                M.AbrirMensagem("Erro ao abrir comprovativo: " + ex.Message, "Erro");
            }
        }

        private void btnReprovar_Click(object sender, EventArgs e)
        {
            string justificativaFinal = "";

            // 1. ANALISAR O CONTEÚDO ATUAL DA TXTOBS
            // Filtramos para ignorar o que o programa escreve sozinho (Logs de Stock / PEPIDI)
            List<string> linhasAtuais = txtObs.Lines.ToList();

            // Pegamos apenas em linhas que NÃO são logs automáticos e NÃO estão vazias
            var linhasManuais = linhasAtuais.Where(l =>
                !l.Contains("Alterou") &&
                !l.Contains("(falta de stock)") &&
                !string.IsNullOrWhiteSpace(l)
            ).ToList();

            if (linhasManuais.Count > 0)
            {
                // CASO A: O utilizador já escreveu o motivo diretamente na txtObs
                // Pegamos no texto manual (limpando possíveis prefixos que já lá estejam)
                string textoPuro = string.Join(" ", linhasManuais).Replace($"[{NomeGestor}]:", "").Trim();
                justificativaFinal = $"[{NomeGestor}]: MOTIVO REPROVAÇÃO - {textoPuro}";
            }
            else
            {
                // CASO B: A txtObs está vazia ou só tem logs do PEPIDI
                // Abrimos o formulário com o efeito de sombra (Overlay)
                using (Form overlay = new Form())
                {
                    overlay.StartPosition = FormStartPosition.Manual;
                    overlay.FormBorderStyle = FormBorderStyle.None;
                    overlay.Opacity = 0.50d;
                    overlay.BackColor = Color.Black;
                    overlay.ShowInTaskbar = false;
                    Form formPrincipal = this.FindForm();
                    overlay.Location = formPrincipal.Location;
                    overlay.Size = formPrincipal.Size;
                    overlay.Show(formPrincipal);

                    using (FormMotivo frm = new FormMotivo())
                    {
                        if (frm.ShowDialog(overlay) == DialogResult.OK)
                        {
                            justificativaFinal = $"[{NomeGestor}]: MOTIVO REPROVAÇÃO - {frm.Motivo}";
                        }
                        else
                        {
                            overlay.Close();
                            return; // Cancelou
                        }
                    }
                    overlay.Close();
                }
            }

            ExecutarReprovacao(justificativaFinal);
        }

        private void ExecutarReprovacao(string notaLimpa)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
                {
                    conn.Open();
                    // Gravamos apenas a nota formatada, descartando todo o lixo da txtObs
                    SqlCommand cmd = new SqlCommand("UPDATE PedidoRegistos SET Estado = 'Reprovado', Notas = @Notas WHERE ID = @ID", conn);
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Parameters.AddWithValue("@Notas", notaLimpa);

                    cmd.ExecuteNonQuery();
                    M.AbrirMensagem("Pedido reprovado com sucesso.", "Sucesso");

                    this.Parent.Controls.Remove(this);
                    this.Dispose();
                }
            }
            catch (Exception ex) { M.AbrirMensagem("Erro ao reprovar: " + ex.Message, "Erro"); }
        }
    }
}