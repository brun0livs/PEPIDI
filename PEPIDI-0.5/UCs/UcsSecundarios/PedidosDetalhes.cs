using Microsoft.Data.SqlClient;
using PEPIDI.FormsSecundarios;
using PEPIDI.Models;
using PEPIDI.Organizers;
using PEPIDI.UCs.DGVS;
using PEPIDI.Utils;
using System;
using System.Collections.Generic;
using System.Data;
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
        public event EventHandler AcaoConcluida;

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
            GestorTema.AplicarEstilos(this);

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
            lblDGVQuant1.Text = (estado == "Aprovado") ? "Selecionar" : "Quantidade";

            if (estado == "Pendente")
            {
                btnAprovar.Text = "Aprovar";
                btnReprovar.Enabled = true;
                lblDGVQuantDisp.Text = "Quant. Disp";
            }
            else if (estado == "Aprovado")
            {
                btnAprovar.Text = "Finalizar";
                btnReprovar.Enabled = false;
                lblDGVQuantDisp.Text = "Quantidade";
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
                // 1. Usar o nome exato que aparece no teu print do SSMS
                int idLinha = Convert.ToInt32(row["IDLinhaDevolucao"]);
                string modelo = row["ModeloComCor"].ToString().Trim();
                string tamanho = row["Tamanho"].ToString().Trim();
                string codigoEpi = row["Codigo"].ToString();

                // 2. Quantidade (O teu print diz 'QuantidadeDevolvida')
                int quantDevolvida = Convert.ToInt32(row["QuantidadeDevolvida"]);
                if (quantDevolvida == 0) quantDevolvida = 1;

                // 3. Estado por defeito
                string cena = "Usado";

                PEPIDI.UCs.DGVS.LinhaDevolucao novaLinha = new PEPIDI.UCs.DGVS.LinhaDevolucao(idLinha, modelo, tamanho, cena, quantDevolvida);
                novaLinha.Tag = codigoEpi;

                // ATRELA O EVENTO AQUI!
                novaLinha.EstadoAlterado += LinhaDev_EstadoAlterado;

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

        private void LinhaDev_EstadoAlterado(object sender, EventArgs e)
        {
            if (this.IsDisposed || this.Disposing) return;

            if (sender is PEPIDI.UCs.DGVS.LinhaDevolucao linhaDev)
            {
                // O texto de busca exato que vai aparecer na nota!
                string logBusca = $"a devolução de '{linhaDev.DescricaoArtigo} ({linhaDev.TamanhoSelecionado})'";
                List<string> linhasLog = txtObs.Lines.ToList();

                // Limpa notas antigas DESTA devolução específica para não fazer spam (agora já encontra!)
                linhasLog.RemoveAll(l => l.TrimStart().StartsWith("[") && l.Contains(logBusca) && l.Contains("Classificou"));

                string estadoAtual = linhaDev.EstadoSelecionado;

                // Se o estado for o normal (Usado), não chateia. Mas se forcar a "Novo" ou mandar para o Lixo ("Gasto"), escreve!
                if (!estadoAtual.Equals("Usado", StringComparison.OrdinalIgnoreCase))
                {
                    string novaNota = $"[{NomeGestor}]: Classificou {logBusca} como {estadoAtual.ToUpper()}.";
                    linhasLog.Add(novaNota);
                }

                txtObs.Text = string.Join(Environment.NewLine, linhasLog.Where(l => !string.IsNullOrWhiteSpace(l)));
            }
        }

        public async Task CarregarPPacoteAsync(Panel pnlConteudo, int idPedido, string estado, Panel pnlScroll, FlowLayoutPanel flpLinhas)
        {
            await Task.Delay(100);
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
                string codigoEpi = row["Codigo"].ToString();
                string modelo = row["ModeloComCor"].ToString().Trim();
                string tamanho = row["Tamanho"].ToString().Trim();

                int stockNovo = Convert.ToInt32(row["QtdStockNovo"]);
                int stockUsado = Convert.ToInt32(row["QtdStockUsado"]);
                int quantDisp = stockNovo + stockUsado;
                int quantPedida = Convert.ToInt32(row["QuantidadePedida"]);

                //if (quantPedida <= 0) continue;

                LinhaItem novaLinha = new LinhaItem(modelo, tamanho, quantDisp, quantPedida, estado);
                novaLinha.Tag = codigoEpi;
                novaLinha.IDEPI = Convert.ToInt32(row["IDLinhaPedido"]);
                novaLinha.QuantidadeOriginal = quantPedida;

                // 1º Atrelar os eventos
                novaLinha.QuantidadeAlterada += Linha_QuantidadeAlterada;
                novaLinha.EstadoAlterado += Linha_EstadoAlterado;

                // 2º Disparar o da Quantidade (para notas de falta de stock iniciais)
                Linha_QuantidadeAlterada(novaLinha, EventArgs.Empty);

                novaLinha.AutoSize = false;
                novaLinha.Margin = new Padding(0, 2, 0, 2);
                novaLinha.Width = flpLinhas.ClientSize.Width - 10;
                novaLinha.Height = 40;

                // 3º CONFIGURAR O CÉREBRO DO STOCK PRIMEIRO!
                novaLinha.ConfigurarSugestaoStock(stockNovo, stockUsado);

                // 4º AGORA SIM, DISPARAR O EVENTO DO ESTADO (Para ele escrever na TextBox se faltar Novo)
                Linha_EstadoAlterado(novaLinha, EventArgs.Empty);

                flpLinhas.Controls.Add(novaLinha);
            }

            flpLinhas.ResumeLayout(true);
            flpLinhas.Visible = true;
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

                // O SEGREDO ESTÁ AQUI: Só apaga se contiver o nome da peça E a palavra "quantidade"
                linhasLog.RemoveAll(l => l.TrimStart().StartsWith("[") && l.Contains(logBusca) && l.Contains("quantidade"));

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

        private void Linha_EstadoAlterado(object sender, EventArgs e)
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

                string logBusca = $"estado de '{linha.M} ({linha.T})'";
                List<string> linhasLog = txtObs.Lines.ToList();

                // Limpa notas antigas deste artigo para não fazer spam
                linhasLog.RemoveAll(l => l.TrimStart().StartsWith("[") && l.Contains(logBusca));

                string estadoAtual = linha.EstadoSelecionado;

                // Se o sistema trancou a combobox em "Usado", foi porque não havia Novo!
                if (linha.EstadoForcadoPeloSistema && estadoAtual == "Usado")
                {
                    string novaNota = $"[PEPIDI]: Definiu o {logBusca} como USADO{linha.ObservacoesAutomaticas}.";
                    linhasLog.Add(novaNota);
                }
                // Se a combobox estiver destrancada e o estado for "Usado", foi o Gestor que mudou à mão!
                else if (!linha.EstadoForcadoPeloSistema && estadoAtual == "Usado")
                {
                    string novaNota = $"[{NomeGestor}]: Definiu o {logBusca} como USADO.";
                    linhasLog.Add(novaNota);
                }
                // Se o estado for "Novo" (que é o normal), apagamos as notas e não dizemos nada para não poluir o ecrã.

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
            // O UC avisa o Form Principal que quer fechar. O Form trata do resto!
            AcaoConcluida?.Invoke(this, EventArgs.Empty);
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
                                    // Se não houver stock, o sistema agora corta sozinho e não bloqueia!
                                    int qtdParaAprovar = Math.Min(linha.QuantidadeSelecionada, linha.QD);
                                    if (qtdParaAprovar <= 0)
                                    {
                                        // Se for 0, atualizamos apenas a linha para 0 no pacote
                                        string sqlZero = "UPDATE PedidoPacote SET Quantidade = 0 WHERE ID = @id";
                                        using (SqlCommand cmdZ = new SqlCommand(sqlZero, conn, trans))
                                        {
                                            cmdZ.Parameters.AddWithValue("@id", linha.IDEPI);
                                            cmdZ.ExecuteNonQuery();
                                        }
                                        continue;
                                    }

                                    string codEpi = linha.Tag.ToString();
                                    int idEstado = (linha.EstadoSelecionado == "Novo") ? 1 : 2;

                                    // Buscar gaveta do stock
                                    int idStockReal = 0;
                                    using (SqlCommand cmdS = new SqlCommand("SELECT TOP 1 ID FROM Stock WHERE Codigo = @c AND Estado = @e", conn, trans))
                                    {
                                        cmdS.Parameters.AddWithValue("@c", codEpi);
                                        cmdS.Parameters.AddWithValue("@e", idEstado);
                                        object r = cmdS.ExecuteScalar();
                                        if (r != null) idStockReal = Convert.ToInt32(r);
                                    }

                                    if (idStockReal > 0)
                                    {
                                        // Atualizar Pedido e Abater Stock
                                        string sqlUp = "UPDATE PedidoPacote SET Quantidade = @q, IDStock = @is WHERE ID = @id";
                                        using (SqlCommand cmdUp = new SqlCommand(sqlUp, conn, trans))
                                        {
                                            cmdUp.Parameters.AddWithValue("@q", qtdParaAprovar);
                                            cmdUp.Parameters.AddWithValue("@is", idStockReal);
                                            cmdUp.Parameters.AddWithValue("@id", linha.IDEPI);
                                            cmdUp.ExecuteNonQuery();
                                        }
                                        using (SqlCommand cmdA = new SqlCommand("UPDATE Stock SET Quant = Quant - @q WHERE ID = @is", conn, trans))
                                        {
                                            cmdA.Parameters.AddWithValue("@q", qtdParaAprovar);
                                            cmdA.Parameters.AddWithValue("@is", idStockReal);
                                            cmdA.ExecuteNonQuery();
                                        }
                                    }
                                }
                            }

                            string sqlFinal = "UPDATE PedidoRegistos SET Estado = 'Aprovado', Aprovacao = @g, AlteradoPor = @g, Notas = @n, AlteracaoData = GETDATE() WHERE ID = @id";
                            using (SqlCommand cmdF = new SqlCommand(sqlFinal, conn, trans))
                            {
                                cmdF.Parameters.AddWithValue("@id", this.ID);
                                cmdF.Parameters.AddWithValue("@g", IDGestor);
                                cmdF.Parameters.AddWithValue("@n", notasFinaisParaGravar);
                                cmdF.ExecuteNonQuery();
                            }
                            trans.Commit();
                            M.AbrirMensagem("Aprovado! Stock reservado.", "Sucesso");
                            AcaoConcluida?.Invoke(this, EventArgs.Empty);
                        }
                        catch { trans.Rollback(); throw; }
                    }
                }
                catch (Exception ex) { M.AbrirMensagem("Erro: " + ex.Message, "Erro"); }
            }
            else if (estadoAtual.Equals("Aprovado", StringComparison.OrdinalIgnoreCase))
            {
                CarregarDadosFuncionario(this.ID);
                var listaReceber = new List<(int ID, string Artigo, string Tamanho, int Qtd)>();
                var listaDevolver = new List<(int ID, string Artigo, string Tamanho, int Qtd)>();

                foreach (Control c in flpLinhas.Controls)
                {
                    if (c is LinhaItem l && l.Selecionado && l.QuantidadeSelecionada > 0)
                        listaReceber.Add((l.IDEPI, $"{l.DescricaoArtigo} [{l.EstadoSelecionado.ToUpper()}]", l.TamanhoSelecionado, l.QuantidadeSelecionada));
                }
                foreach (Control c in flpDevolucoes.Controls)
                {
                    if (c is LinhaDevolucao d && d.QuantidadeDevolvida > 0)
                        listaDevolver.Add((d.IDEPI, $"{d.DescricaoArtigo} [{d.EstadoSelecionado.ToUpper()}]", d.TamanhoSelecionado, d.QuantidadeDevolvida));
                }

                using (var frm = new FormAssinatura(NomeFuncionario))
                {
                    foreach (var i in listaReceber) frm.AdicionarItemReceber(i.Artigo, i.Tamanho, i.Qtd);
                    foreach (var i in listaDevolver) frm.AdicionarItemDevolver(i.Artigo, i.Tamanho, i.Qtd);
                    if (frm.ShowDialog() != DialogResult.OK) return;

                    try
                    {
                        string path = PDFGenerator.GerarComprovativo(this.ID, NomeFuncionario, NMEC, FuncaoFuncionario, NomeGestor, listaReceber, listaDevolver, frm.AssinaturaFinal);
                        using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
                        {
                            conn.Open();
                            using (SqlTransaction trans = conn.BeginTransaction())
                            {
                                try
                                {
                                    // Fechar Pedido
                                    new SqlCommand($"UPDATE PedidoRegistos SET Estado='Finalizado', PDF='{path}', Entrega={IDGestor}, AlteradoPor={IDGestor}, AlteracaoData=GETDATE() WHERE ID={this.ID}", conn, trans).ExecuteNonQuery();

                                    // Tratar Retomas (Inclui Estado 3 - Gasto)
                                    foreach (Control c in flpDevolucoes.Controls)
                                    {
                                        if (c is LinhaDevolucao d && d.QuantidadeDevolvida > 0)
                                        {
                                            int est = (d.EstadoSelecionado == "Novo") ? 1 : (d.EstadoSelecionado == "Usado" ? 2 : 3);
                                            // 1. Garantir gaveta no stock
                                            int idS = 0;
                                            var cmdS = new SqlCommand("SELECT ID FROM Stock WHERE Codigo=@c AND Estado=@e", conn, trans);
                                            cmdS.Parameters.AddWithValue("@c", d.Tag.ToString()); cmdS.Parameters.AddWithValue("@e", est);
                                            var res = cmdS.ExecuteScalar();
                                            if (res == null)
                                            {
                                                var cmdI = new SqlCommand("INSERT INTO Stock (Codigo, Estado, Quant) OUTPUT INSERTED.ID VALUES (@c, @e, 0)", conn, trans);
                                                cmdI.Parameters.AddWithValue("@c", d.Tag.ToString()); cmdI.Parameters.AddWithValue("@e", est);
                                                idS = Convert.ToInt32(cmdI.ExecuteScalar());
                                            }
                                            else idS = Convert.ToInt32(res);

                                            // 2. Somar e Vincular
                                            new SqlCommand($"UPDATE Stock SET Quant=Quant+{d.QuantidadeDevolvida} WHERE ID={idS}", conn, trans).ExecuteNonQuery();
                                            new SqlCommand($"UPDATE RoupaPacote SET IDStock={idS} WHERE ID={d.IDEPI}", conn, trans).ExecuteNonQuery();
                                        }
                                    }
                                    trans.Commit();
                                    System.Diagnostics.Process.Start("explorer.exe", path);
                                    AcaoConcluida?.Invoke(this, EventArgs.Empty);
                                }
                                catch { trans.Rollback(); throw; }
                            }
                        }
                    }
                    catch (Exception ex) { M.AbrirMensagem(ex.Message, "Erro"); }
                }
            }
            else { AbrirComprovativoExistente(); }
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

            // O NOVO FILTRO MÁGICO: É considerado "Manual" se não for vazio E não começar por "["
            var linhasManuais = linhasAtuais.Where(l =>
                !string.IsNullOrWhiteSpace(l) &&
                !l.TrimStart().StartsWith("[")
            ).ToList();

            if (linhasManuais.Count > 0)
            {
                // Se o gestor escreveu texto livre no meio da confusão, usamos isso!
                string textoPuro = string.Join(" ", linhasManuais).Trim();
                justificativaFinal = $"[{NomeGestor}]: MOTIVO REPROVAÇÃO - {textoPuro}";
            }
            else
            {
                // Se só há notas do sistema, OBRIGA a abrir o popup!
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
                            return; // Abortar reprovação se ele fechar a janela no X
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

                    // O SQL agora preenche a auditoria toda: Quem reprovou (Aprovacao / AlteradoPor) e Quando (AlteracaoData)
                    string sql = @"UPDATE PedidoRegistos 
                           SET Estado = 'Reprovado', 
                               Notas = @Notas, 
                               Aprovacao = @Gestor, 
                               AlteracaoData = GETDATE(), 
                               AlteradoPor = @Gestor 
                           WHERE ID = @ID";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@ID", this.ID);
                    cmd.Parameters.AddWithValue("@Notas", notaLimpa);
                    cmd.Parameters.AddWithValue("@Gestor", this.IDGestor); // O teu ID que vem do Form Principal

                    cmd.ExecuteNonQuery();
                    M.AbrirMensagem("Pedido reprovado com sucesso.", "Sucesso");

                    AcaoConcluida?.Invoke(this, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                M.AbrirMensagem("Erro ao reprovar: " + ex.Message, "Erro");
            }
        }
    }
}