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
            }
            else if (estado == "Aprovado")
            {
                btnAprovar.Text = "Finalizar";
                btnReprovar.Enabled = true;
            }
            else
            {
                btnAprovar.Text = "Comprovativo";
                btnReprovar.Enabled = false;
            }

            CarregarPPacote(pnlConteudo, ID, estado, pnlScroll, tlpLinhas);
            VerComentario(ID);
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
                    int quantPedida = row.Table.Columns.Contains("QuantidadePedida") ? Convert.ToInt32(row["QuantidadePedida"]) : 0;

                    LinhaItem novaLinha = new LinhaItem(modelo, tamanho, quantDisp, quantPedida, estado);
                    novaLinha.IDEPI = idEpi;
                    novaLinha.QuantidadeOriginal = quantPedida;
                    novaLinha.QuantidadeAlterada += Linha_QuantidadeAlterada; // Liga o evento de log
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
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
                {
                    conn.Open();
                    SqlTransaction trans = conn.BeginTransaction();

                    try
                    {
                        // 1. ATUALIZAR ITENS
                        foreach (Control c in tlpLinhas.Controls)
                        {
                            if (c is LinhaItem linha)
                            {
                                SqlCommand cmdItem = new SqlCommand("sp_AtualizarQuantidadePedidoPacote", conn, trans)
                                {
                                    CommandType = CommandType.StoredProcedure
                                };

                                cmdItem.Parameters.AddWithValue("@IDPedido", ID);
                                // Aqui usamos o IDEPI. Se não o tiveres, usamos Modelo/Tamanho como filtro
                                cmdItem.Parameters.AddWithValue("@IDEPI", linha.IDEPI);
                                cmdItem.Parameters.AddWithValue("@NovaQuantidade", linha.QuantidadeSelecionada);

                                cmdItem.ExecuteNonQuery();
                            }
                        }

                        // 2. NOTAS NA APROVAÇÃO (O que tu querias!)
                        // Gravamos o conteúdo TOTAL da txtObs, incluindo as notas do PEPIDI
                        string notaFinal = txtObs.Text.Trim();

                        // 3. ATUALIZAR ESTADO DO PEDIDO
                        string sqlPedido = @"UPDATE PedidoRegistos 
                                   SET Estado = 'Aprovado', 
                                       Aprovacao = @Aprovador, 
                                       Notas = @Notas,
                                       AlteracaoData = GETDATE(),
                                       AlteradoPor = @Aprovador
                                   WHERE ID = @ID";

                        SqlCommand cmdPedido = new SqlCommand(sqlPedido, conn, trans);
                        cmdPedido.Parameters.AddWithValue("@ID", ID);
                        cmdPedido.Parameters.AddWithValue("@Aprovador", IDGestor);
                        cmdPedido.Parameters.AddWithValue("@Notas", notaFinal); // Grava tudo o que vês no ecrã

                        cmdPedido.ExecuteNonQuery();

                        trans.Commit();
                        M.AbrirMensagem("Pedido aprovado com sucesso!", "Aprovado");

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
            catch (Exception ex) { M.AbrirMensagem("Erro ao aprovar: " + ex.Message, "Erro"); }
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