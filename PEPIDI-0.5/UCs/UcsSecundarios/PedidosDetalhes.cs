using PEPIDI.FormsSecundarios;
using PEPIDI.Models;
using PEPIDI.Organizers;
using PEPIDI.UCs.DGVS;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
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
            
            // Força a UC a usar a memória para desenhar antes de mostrar (GPU friendly)
            this.DoubleBuffered = true;

            foreach (Control control in this.Controls)
            {
                EnableDoubleBuffer(control);
            }

            ID = _ID;
            IDGestor = _IDGestor;
            Estado = _Estado;

            // MATAR O DUPLO SCROLL
            if (pnlScroll != null) pnlScroll.AutoScroll = false;
            if (pnlScroll2 != null) pnlScroll2.AutoScroll = false;

            flpLinhas.AutoScroll = true;
            flpDevolucoes.AutoScroll = true;

            // FORÇAR PADDING ZERO NOS PAINÉIS PARA ELES NÃO EMPURRAREM AS LINHAS
            flpLinhas.Padding = new Padding(0);
            flpLinhas.Margin = new Padding(0);
            flpDevolucoes.Padding = new Padding(0);
            flpDevolucoes.Margin = new Padding(0);

            flpLinhas.SizeChanged += (s, e) => AjustarLarguras(flpLinhas);
            flpDevolucoes.SizeChanged += (s, e) => AjustarLarguras(flpDevolucoes);

        }

        private void EnableDoubleBuffer(Control c)
        {
            var property = typeof(Control).GetProperty("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            property?.SetValue(c, true, null);

            foreach (Control child in c.Controls) EnableDoubleBuffer(child);
        }

        // =========================================================================
        // METODO QUE ESMAGA QUALQUER BURACO LATERAL E FORÇA A LARGURA MÁXIMA
        // =========================================================================
        private void AjustarLarguras(FlowLayoutPanel flp)
        {
            if (flp.IsDisposed || !flp.IsHandleCreated) return;

            flp.SuspendLayout();
            foreach (Control c in flp.Controls)
            {
                // A elegância voltou! 5px de cada lado.
                c.Margin = new Padding(5, 2, 5, 2);
                // Desconta os 10px das laterais + 2px de segurança
                c.Width = flp.ClientSize.Width - 12;
            }
            flp.ResumeLayout();
        }

        private async void PedidosDetalhes_Load(object sender, EventArgs e)
        {
            Application.AddMessageFilter(this);
            var info = Details.GetInfoGestor(IDGestor);
            NomeGestor = info.Nome;
            await GereEstadoAsync(Estado);
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            Application.RemoveMessageFilter(this);
            base.OnHandleDestroyed(e);
        }

        public bool PreFilterMessage(ref Message m)
        {
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
                        SendMessage(flpLinhas.Handle, m.Msg, m.WParam, m.LParam);
                    }
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
                    using (SqlCommand cmd = new SqlCommand("sp_ObterFuncionarioPorPedido", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IDPedido", idPedido);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                this.NomeFuncionario = reader["Nome"]?.ToString() ?? "Sem Nome";
                                this.NMEC = reader["Nr"]?.ToString() ?? "0000";
                                this.FuncaoFuncionario = reader["Funcao"]?.ToString() ?? "-";
                            }
                            else
                            {
                                this.NomeFuncionario = "Erro ao ler Funcionário";
                                this.NMEC = "0000";
                                this.FuncaoFuncionario = "Erro";
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    this.NomeFuncionario = "Erro SQL";
                    this.NMEC = "0000";
                    this.FuncaoFuncionario = "Erro";
                }
            }
        }

        public async Task CarregarDPacoteAsync(FlowLayoutPanel flpLinhasDev, Panel pnlScrollDev, int idPedido, string estado)
        {
            await Task.Delay(150);

            flpLinhasDev.Visible = false;
            flpLinhasDev.SuspendLayout();
            flpLinhasDev.Controls.Clear();

            DataTable dt = new DataTable();

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

                // ZERO MARGEM NAS LATERAIS
                novaLinha.AutoSize = false;
                novaLinha.Margin = new Padding(0, 2, 0, 2);
                novaLinha.Width = flpLinhasDev.ClientSize.Width;
                novaLinha.Height = 40;

                flpLinhasDev.Controls.Add(novaLinha);
            }

            flpLinhasDev.ResumeLayout(true);
            flpLinhasDev.Visible = true;

            // Força o ajuste imediatamente após desenhar
            AjustarLarguras(flpLinhasDev);
        }

        public async Task CarregarPPacoteAsync(Panel pnlConteudo, int idPedido, string estado, Panel pnlScroll, FlowLayoutPanel flpLinhas)
        {
            await Task.Delay(150);

            flpLinhas.Visible = false;
            flpLinhas.SuspendLayout();
            flpLinhas.Controls.Clear();

            DataTable dt = new DataTable();

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

                // ZERO MARGEM NAS LATERAIS
                novaLinha.AutoSize = false;
                novaLinha.Margin = new Padding(0, 2, 0, 2);
                novaLinha.Width = flpLinhas.ClientSize.Width;
                novaLinha.Height = 40;

                flpLinhas.Controls.Add(novaLinha);
            }

            flpLinhas.ResumeLayout(true);
            flpLinhas.Visible = true;

            // Força o ajuste imediatamente após desenhar
            AjustarLarguras(flpLinhas);
        }

        private void Linha_QuantidadeAlterada(object sender, EventArgs e)
        {
            if (this.Estado.Equals("Aprovado", StringComparison.OrdinalIgnoreCase) ||
                this.Estado.Equals("Concluido", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

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
            if (this.Parent != null)
            {
                this.Parent.Controls.Remove(this);
                this.Dispose();
            }
        }

        private void btnAprovar_Click(object sender, EventArgs e)
        {
            string estadoAtual = this.Estado.Trim();
            string notasFinaisParaGravar = txtObs.Text.Trim();

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
                            foreach (Control c in flpLinhas.Controls)
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
            else if (estadoAtual.Equals("Aprovado", StringComparison.OrdinalIgnoreCase))
            {
                CarregarDadosFuncionario(this.ID);

                var listaReceber = new List<(int ID, string Artigo, string Tamanho, int Qtd)>();
                var listaDevolver = new List<(int ID, string Artigo, string Tamanho, int Qtd)>();
                var todosOsItens = new List<(int IDEPI, int QtdReal, bool Selecionado)>();

                foreach (Control c in flpLinhas.Controls)
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

                foreach (Control c in flpDevolucoes.Controls)
                {
                    if (c is PEPIDI.UCs.DGVS.LinhaDevolucao linhaDev)
                    {
                        if (linhaDev.QuantidadeDevolvida > 0)
                        {
                            listaDevolver.Add((linhaDev.IDEPI, linhaDev.DescricaoArtigo, linhaDev.TamanhoSelecionado, linhaDev.QuantidadeDevolvida));
                        }
                    }
                }

                if (listaReceber.Count == 0 && listaDevolver.Count == 0)
                {
                    M.AbrirMensagem("Não há itens selecionados para entregar nem para devolver.", "Aviso");
                    return;
                }

                using (var frm = new FormAssinatura(NomeFuncionario))
                {
                    foreach (var item in listaReceber)
                        frm.AdicionarItemReceber(item.Artigo, item.Tamanho, item.Qtd);

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
                        string caminhoPDF = PEPIDI.Organizers.PDFGenerator.GerarComprovativo(
                            this.ID, NomeFuncionario, NMEC, FuncaoFuncionario, NomeGestor,
                            listaReceber, listaDevolver,
                            frm.AssinaturaFinal
                        );

                        using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
                        {
                            conn.Open();
                            using (SqlTransaction trans = conn.BeginTransaction())
                            {
                                try
                                {
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
                                catch (Exception)
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
            List<string> linhasAtuais = txtObs.Lines.ToList();
            var linhasManuais = linhasAtuais.Where(l =>
                !l.Contains("Alterou") &&
                !l.Contains("(falta de stock)") &&
                !string.IsNullOrWhiteSpace(l)
            ).ToList();

            if (linhasManuais.Count > 0)
            {
                string textoPuro = string.Join(" ", linhasManuais).Replace($"[{NomeGestor}]:", "").Trim();
                justificativaFinal = $"[{NomeGestor}]: MOTIVO REPROVAÇÃO - {textoPuro}";
            }
            else
            {
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
                            return;
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