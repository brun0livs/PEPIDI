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
using System.Threading.Tasks;
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

        private async void PedidosDetalhes_Load(object sender, EventArgs e)
        {
            // Ativa o filtro global que impede a recursividade
            Application.AddMessageFilter(this);

            var info = Details.GetInfoGestor(IDGestor);
            NomeGestor = info.Nome;
            await GereEstadoAsync(Estado);
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

        private async Task GereEstadoAsync(string estado)
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

            VerComentario(ID);

            // Carrega os dois painéis em pano de fundo!
            await CarregarPPacoteAsync(pnlConteudo, ID, estado, pnlScroll, flpLinhas);
            await CarregarDPacoteAsync(flpDevolucoes, pnlScroll2, ID, estado);
        }

        private void CarregarDadosFuncionario(int idPedido)
        {
            using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
            {
                try
                {
                    conn.Open();

                    // Chama a nova SP que acabaste de criar no SQL
                    using (SqlCommand cmd = new SqlCommand("sp_ObterFuncionarioPorPedido", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Passa o parâmetro com o nome exato que está na SP (@IDPedido)
                        cmd.Parameters.AddWithValue("@IDPedido", idPedido);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Dados encontrados com sucesso
                                this.NomeFuncionario = reader["Nome"]?.ToString() ?? "Sem Nome";
                                this.NMEC = reader["Nr"]?.ToString() ?? "0000";
                                this.FuncaoFuncionario = reader["Funcao"]?.ToString() ?? "-";
                            }
                            else
                            {
                                // Se por algum motivo o pedido não existir ou não tiver funcionário
                                this.NomeFuncionario = "Erro ao ler Funcionário";
                                this.NMEC = "0000";
                                this.FuncaoFuncionario = "Erro";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log de erro invisível (para não rebentar o programa) e define valores de segurança
                    this.NomeFuncionario = "Erro SQL";
                    this.NMEC = "0000";
                    this.FuncaoFuncionario = "Erro";
                }
            }
        }

        public async Task CarregarDPacoteAsync(FlowLayoutPanel flpLinhasDev, Panel pnlScrollDev, int idPedido, string estado)
        {
            await Task.Delay(150); // Deixa o ecrã respirar

            // 1. FECHA OS OLHOS e limpa!
            flpLinhasDev.Visible = false;
            flpLinhasDev.SuspendLayout();
            flpLinhasDev.Controls.Clear();

            DataTable dt = new DataTable();

            // 2. SQL EM PANO DE FUNDO!
            await Task.Run(() =>
            {
                using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_DetalhesDaDevolucao", conn) { CommandType = CommandType.StoredProcedure };
                    cmd.Parameters.AddWithValue("@IDPedido", idPedido);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
            });

            // 3. DESENHA RAPIDAMENTE SEM GEOMETRIAS
            foreach (DataRow row in dt.Rows)
            {
                int idEpi = Convert.ToInt32(row["ID"]);
                string modelo = row["Modelo"].ToString().Trim();
                string tamanho = row["Tamanho"].ToString().Trim();

                int quantDevolvida = 0;
                if (dt.Columns.Contains("QuantidadePedida")) quantDevolvida = Convert.ToInt32(row["QuantidadePedida"]);
                else if (dt.Columns.Contains("Quantidade")) quantDevolvida = Convert.ToInt32(row["Quantidade"]);
                else if (dt.Columns.Contains("QuantidadeDevolvida")) quantDevolvida = Convert.ToInt32(row["QuantidadeDevolvida"]);
                if (quantDevolvida == 0) quantDevolvida = 1;

                PEPIDI.UCs.DGVS.LinhaDevolucao novaLinha = new PEPIDI.UCs.DGVS.LinhaDevolucao(idEpi, modelo, tamanho, quantDevolvida);
                novaLinha.Width = flpLinhasDev.Width - 20; // Ajuste para a scrollbar
                novaLinha.CreateControl();
                flpLinhasDev.Controls.Add(novaLinha); // Simples e instantâneo
            }

            // 4. ABRE OS OLHOS!
            flpLinhasDev.ResumeLayout(true);
            flpLinhasDev.Visible = true;
        }

        public async Task CarregarPPacoteAsync(Panel pnlConteudo, int idPedido, string estado, Panel pnlScroll, FlowLayoutPanel flpLinhas)
        {
            await Task.Delay(150); // Deixa o ecrã respirar

            // 1. FECHA OS OLHOS e limpa!
            flpLinhas.Visible = false;
            flpLinhas.SuspendLayout();
            flpLinhas.Controls.Clear();

            pnlScroll.Size = tlpDesign.GetControlFromPosition(0, 1).Size;

            DataTable dt = new DataTable();

            // 2. VAI AO SQL EM PANO DE FUNDO!
            await Task.Run(() =>
            {
                using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_DetalhesDoPedido", conn) { CommandType = CommandType.StoredProcedure };
                    cmd.Parameters.AddWithValue("@IDPedido", idPedido);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
            });

            // 3. DESENHA INSTANTANEAMENTE SEM GEOMETRIAS DE TABELAS
            foreach (DataRow row in dt.Rows)
            {
                int idEpi = Convert.ToInt32(row["ID"]);
                string modelo = row["Modelo"].ToString().Trim();
                string tamanho = row["Tamanho"].ToString().Trim();
                int quantDisp = Convert.ToInt32(row["QuantidadeDisponivel"]);

                int quantPedida = 0;
                if (dt.Columns.Contains("QuantidadePedida")) quantPedida = Convert.ToInt32(row["QuantidadePedida"]);
                else if (dt.Columns.Contains("Quantidade")) quantPedida = Convert.ToInt32(row["Quantidade"]);
                if (quantPedida == 0) quantPedida = 1;

                LinhaItem novaLinha = new LinhaItem(modelo, tamanho, quantDisp, quantPedida, estado);
                novaLinha.IDEPI = idEpi;
                novaLinha.QuantidadeOriginal = quantPedida;
                novaLinha.QuantidadeAlterada += Linha_QuantidadeAlterada;
                Linha_QuantidadeAlterada(novaLinha, EventArgs.Empty);

                novaLinha.Width = flpLinhas.Width - 20; // Ajuste para a scrollbar não tapar os botões
                novaLinha.CreateControl(); // Pré-renderizar
                flpLinhas.Controls.Add(novaLinha);
            }

            // 4. ABRE OS OLHOS E MOSTRA O RESULTADO FINAL!
            flpLinhas.ResumeLayout(true);
            flpLinhas.Visible = true;
        }

        private void Linha_QuantidadeAlterada(object sender, EventArgs e)
        {
            // 1. O EXORCISTA DO FANTASMA DO "0"! 👻🚫
            if (this.Estado.Equals("Aprovado", StringComparison.OrdinalIgnoreCase) ||
                this.Estado.Equals("Concluido", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            // 2. PROTEÇÃO DO DISPOSE (Para quando o ecrã fecha)
            if (this.IsDisposed || this.Disposing) return;

            if (sender is LinhaItem linha)
            {
                if (linha.IsDisposed || linha.Disposing) return;

                string logBusca = $"{linha.M} ({linha.T})";
                List<string> linhasLog = txtObs.Lines.ToList();

                linhasLog.RemoveAll(l => l.TrimStart().StartsWith("[") && l.Contains(logBusca));

                if (linha.QuantidadeSelecionada != linha.QuantidadeOriginal)
                {
                    string autor = NomeGestor;
                    string notaExtra = "";

                    if (linha.QuantidadeOriginal > linha.QD && linha.QuantidadeSelecionada == linha.QD)
                    {
                        autor = "PEPIDI";
                        notaExtra = " (falta de stock)";
                    }

                    string novaNota = $"[{autor}]: Alterou a quantidade de '{logBusca}' de {linha.QuantidadeOriginal} para {linha.QuantidadeSelecionada}{notaExtra}.";
                    linhasLog.Add(novaNota);
                }

                txtObs.Text = string.Join(Environment.NewLine, linhasLog.Where(l => !string.IsNullOrWhiteSpace(l)));
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

            // GUARDA-COSTAS ANTI-FANTASMA: Guardamos as notas na memória LOGO no início do clique!
            string notasFinaisParaGravar = txtObs.Text.Trim();

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
                            foreach (Control c in flpLinhas.Controls) // <--- ATUALIZADO PARA flpLinhas
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

                            // 2. Mudar estado para 'Aprovado' E GRAVAR AS NOTAS!
                            string sql = @"UPDATE PedidoRegistos 
                                   SET Estado = 'Aprovado', Aprovacao = @Gestor, Notas = @Notas, AlteracaoData = GETDATE() 
                                   WHERE ID = @ID";

                            using (SqlCommand cmd = new SqlCommand(sql, conn, trans))
                            {
                                cmd.Parameters.AddWithValue("@ID", this.ID);
                                cmd.Parameters.AddWithValue("@Gestor", IDGestor);
                                cmd.Parameters.AddWithValue("@Notas", string.IsNullOrEmpty(notasFinaisParaGravar) ? (object)DBNull.Value : notasFinaisParaGravar);
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
                CarregarDadosFuncionario(this.ID);

                var listaReceber = new List<(int ID, string Artigo, string Tamanho, int Qtd)>();
                var listaDevolver = new List<(int ID, string Artigo, string Tamanho, int Qtd)>(); // <--- LISTA PARA AS DEVOLUÇÕES
                var todosOsItens = new List<(int IDEPI, int QtdReal, bool Selecionado)>();

                // 1. RECOLHER ENTREGAS (Do painel flpLinhas)
                foreach (Control c in flpLinhas.Controls) // <--- ATUALIZADO PARA flpLinhas
                {
                    if (c is LinhaItem linha)
                    {
                        int qtdReal = linha.Selecionado ? linha.QuantidadeSelecionada : 0;
                        todosOsItens.Add((linha.IDEPI, qtdReal, linha.Selecionado));

                        if (linha.Selecionado && linha.QuantidadeSelecionada > 0)
                        {
                            listaReceber.Add((linha.IDEPI, linha.DescricaoArtigo, linha.TamanhoSelecionado, linha.QuantidadeSelecionada));
                        }
                    }
                }

                // 2. RECOLHER DEVOLUÇÕES (Do novo flpDevolucoes)
                foreach (Control c in flpDevolucoes.Controls) // <--- ATUALIZADO PARA flpDevolucoes
                {
                    if (c is PEPIDI.UCs.DGVS.LinhaDevolucao linhaDev)
                    {
                        if (linhaDev.QuantidadeDevolvida > 0)
                        {
                            listaDevolver.Add((linhaDev.IDEPI, linhaDev.DescricaoArtigo, linhaDev.TamanhoSelecionado, linhaDev.QuantidadeDevolvida));
                        }
                    }
                }

                // Validação: Tem de haver ou algo para receber, ou algo para devolver!
                if (listaReceber.Count == 0 && listaDevolver.Count == 0)
                {
                    M.AbrirMensagem("Não há itens selecionados para entregar nem para devolver.", "Aviso");
                    return;
                }

                using (var frm = new FormAssinatura(NomeFuncionario))
                {
                    // Passa as entregas para o form de assinatura
                    foreach (var item in listaReceber)
                        frm.AdicionarItemReceber(item.Artigo, item.Tamanho, item.Qtd);

                    // Passa as retomas para o form de assinatura
                    foreach (var item in listaDevolver)
                        frm.AdicionarItemDevolver(item.Artigo, item.Tamanho, item.Qtd);

                    using (Form overlay = new Form { BackColor = Color.Black, Opacity = 0.5, ShowInTaskbar = false, FormBorderStyle = FormBorderStyle.None, WindowState = FormWindowState.Maximized })
                    {
                        overlay.Show();
                        var result = frm.ShowDialog(overlay);
                        if (result != DialogResult.OK) return;
                    }

                    try
                    {
                        // GERA O PDF E PASSA A LISTA DE DEVOLUÇÕES TAMBÉM!
                        string caminhoPDF = PEPIDI.Organizers.PDFGenerator.GerarComprovativo(
                            this.ID, NomeFuncionario, NMEC, FuncaoFuncionario, NomeGestor,
                            listaReceber, listaDevolver, // <--- AQUI VAI A MAGIA DA DEVOLUÇÃO
                            frm.AssinaturaFinal
                        );

                        using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
                        {
                            conn.Open();
                            using (SqlTransaction trans = conn.BeginTransaction())
                            {
                                try
                                {
                                    // Atualiza Estado, PDF E AS NOTAS!
                                    string sqlUpdate = @"UPDATE PedidoRegistos 
                                                 SET Estado = 'Concluido', PDF = @PDF, Notas = @Notas, AlteracaoData = GETDATE() 
                                                 WHERE ID = @ID";

                                    using (SqlCommand cmd = new SqlCommand(sqlUpdate, conn, trans))
                                    {
                                        cmd.Parameters.AddWithValue("@ID", this.ID);
                                        cmd.Parameters.AddWithValue("@PDF", caminhoPDF);
                                        cmd.Parameters.AddWithValue("@Notas", string.IsNullOrEmpty(notasFinaisParaGravar) ? (object)DBNull.Value : notasFinaisParaGravar);
                                        cmd.ExecuteNonQuery();
                                    }

                                    // Abate de Stock APENAS do que foi entregue
                                    foreach (var item in todosOsItens)
                                    {
                                        using (SqlCommand cmdItem = new SqlCommand("sp_AtualizarQuantidadePedidoPacote", conn, trans))
                                        {
                                            cmdItem.CommandType = CommandType.StoredProcedure;
                                            cmdItem.Parameters.AddWithValue("@IDPedido", this.ID);
                                            cmdItem.Parameters.AddWithValue("@IDEPI", item.IDEPI);
                                            cmdItem.Parameters.AddWithValue("@NovaQuantidade", item.QtdReal);
                                            cmdItem.ExecuteNonQuery();
                                        }

                                        if (item.QtdReal > 0)
                                        {
                                            string sqlStock = "UPDATE EPI SET Quantidade = Quantidade - @Qtd WHERE ID = @IDEPI";
                                            using (SqlCommand cmdStock = new SqlCommand(sqlStock, conn, trans))
                                            {
                                                cmdStock.Parameters.AddWithValue("@Qtd", item.QtdReal);
                                                cmdStock.Parameters.AddWithValue("@IDEPI", item.IDEPI);
                                                cmdStock.ExecuteNonQuery();
                                            }
                                        }
                                    }

                                    trans.Commit();

                                    M.AbrirMensagem("Entrega e retoma finalizadas com sucesso!\nPDF Gerado.", "Sucesso");
                                    try { System.Diagnostics.Process.Start("explorer.exe", caminhoPDF); } catch { }

                                    this.Parent.Controls.Remove(this);
                                    this.Dispose();
                                }
                                catch (Exception ex)
                                {
                                    trans.Rollback();
                                    throw;
                                }
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