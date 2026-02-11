using iText.Kernel.Pdf.Canvas.Wmf;
using PEPIDI.Models;
using PEPIDI.Organizers;
using PEPIDI_0._5.UCs.DGVS;
using PEPIDI_0._5.UCs.UcsSecundarios;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PEPIDI.UCs.UcsSecundarios
{
    public partial class PedidosDetalhes : UserControl
    {
        int ID;
        int IDGestor;
        string NomeGestor;
        string Estado;

        public PedidosDetalhes(int _ID, int _IDGestor, string _Estado)
        {
            InitializeComponent();
            ID = _ID;
            IDGestor = _IDGestor;
            Estado = _Estado;
        }

        private void PedidosDetalhes_Load(object sender, EventArgs e)
        {
            var info = Details.GetInfoGestor(IDGestor);
            NomeGestor = info.Nome;
            GereEstado(Estado);

            //Configura(dgvPedidos);
            //Configura(dgvDevolucoes);
        }


        private void Configura(PEPIDIDataGridView dgv)
        {
            // 1. A Grid NÃO pode ser ReadOnly no geral, senão a Combo não abre
            dgv.ReadOnly = false;
        }

        private void GereEstado(string estado)
        {
            if (estado == "Pendente")
            {
                btnAprovar.Text = "Aprovar";
                btnReprovar.Enabled = true;
                CarregarPPacote(pnlConteudo, ID, estado);
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
        }

        public void CarregarPPacote(FlowLayoutPanel pnlConteudo, int idPedido, string estado)
        {
            // 1. Limpeza e Reset
            pnlConteudo.Controls.Clear();
            pnlConteudo.AutoScroll = false; // Importante: O pai NÃO faz scroll
            pnlConteudo.WrapContents = false;
            pnlConteudo.Padding = new Padding(0);
            pnlConteudo.Margin = new Padding(0);

            // 2. Busca os dados primeiro
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_DetalhesDoPedido", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDPedido", idPedido);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }

            // Se não houver dados, sai ou mostra mensagem
            if (dt.Rows.Count == 0) return;

            // 3. TLP MESTRE (O Esqueleto)
            TableLayoutPanel tlpMestre = new TableLayoutPanel();
            tlpMestre.Name = "tlpMestre";
            tlpMestre.ColumnCount = 1;
            tlpMestre.RowCount = 2;
            tlpMestre.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // Linha 0: Cabeçalho
            tlpMestre.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); // Linha 1: Dados
            tlpMestre.Margin = new Padding(0);
            tlpMestre.Padding = new Padding(0);

            // --- CORREÇÃO DO ECRÃ BRANCO ---
            // Forçamos o tamanho do TLP a ser igual ao do FlowPanel pai
            tlpMestre.Width = pnlConteudo.ClientSize.Width;
            tlpMestre.Height = pnlConteudo.ClientSize.Height;

            // --- LÓGICA DE ALINHAMENTO (Cabeçalho vs Scroll) ---
            // Se tivermos mais de 5 linhas, assumimos que vai aparecer scroll vertical.
            // Ajusta o '5' conforme a altura das tuas linhas.
            bool vaiTerScroll = dt.Rows.Count > 5;
            int paddingScroll = vaiTerScroll ? SystemInformation.VerticalScrollBarWidth : 0;

            // A. CABEÇALHO
            CabecalhoPedido cabecalho = new CabecalhoPedido();
            cabecalho.Dock = DockStyle.Top;
            cabecalho.Margin = new Padding(0);
            // Adiciona margem à direita para compensar a barra de scroll
            cabecalho.Padding = new Padding(0, 0, paddingScroll, 0);

            tlpMestre.Controls.Add(cabecalho, 0, 0);

            // B. PAINEL DE SCROLL (Panel Normal)
            Panel pnlScroll = new Panel();
            pnlScroll.Dock = DockStyle.Fill;
            pnlScroll.AutoScroll = true; // O painel gere o scroll
            pnlScroll.Margin = new Padding(0);
            pnlScroll.Padding = new Padding(0);

            // C. DADOS (Tabela interna)
            TableLayoutPanel tlpDados = new TableLayoutPanel();
            tlpDados.Name = "tlpDados";
            tlpDados.AutoSize = true;
            tlpDados.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tlpDados.Dock = DockStyle.Top;
            tlpDados.ColumnCount = 1;
            tlpDados.RowCount = 0;
            tlpDados.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tlpDados.Margin = new Padding(0);
            tlpDados.Padding = new Padding(0);

            foreach (DataRow row in dt.Rows)
            {
                string modelo = row["Modelo"].ToString();
                string tamanho = row["Tamanho"].ToString();
                int quantDisp = Convert.ToInt32(row["QuantidadeDisponivel"]);
                int quantSelecionada = row.Table.Columns.Contains("QuantidadePedida") ? Convert.ToInt32(row["QuantidadePedida"]) : 0;

                LinhaItem novaLinha = new LinhaItem(modelo, tamanho, quantDisp, quantSelecionada);
                novaLinha.Dock = DockStyle.Fill;
                // Margem 0 para garantir que ocupa a largura total disponível
                novaLinha.Margin = new Padding(0);

                tlpDados.RowCount++;
                tlpDados.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                tlpDados.Controls.Add(novaLinha, 0, tlpDados.RowCount - 1);
            }

            // Montar a Boneca Russa
            pnlScroll.Controls.Add(tlpDados);
            tlpMestre.Controls.Add(pnlScroll, 0, 1);

            // 4. ADICIONAR AO PAI (Só no final!)
            pnlConteudo.Controls.Add(tlpMestre);

            // 5. Ativar Scroll do Rato e Garantir Redimensionamento
            AtivarScrollComRato(tlpDados, pnlScroll);

            // Evento de segurança para redimensionar se o utilizador mudar o tamanho da janela
            pnlConteudo.SizeChanged += (s, e) => {
                tlpMestre.Size = pnlConteudo.ClientSize;
            };
        }

        private void AtivarScrollComRato(Control controle, Panel painelPrincipal)
        {
            if (controle != painelPrincipal)
            {
                controle.MouseWheel += (s, e) =>
                {
                    // Proteção contra Null Reference caso o painel tenha sido descartado
                    if (painelPrincipal == null || painelPrincipal.IsDisposed) return;

                    int novaPosicao = painelPrincipal.VerticalScroll.Value - e.Delta;

                    if (novaPosicao < painelPrincipal.VerticalScroll.Minimum)
                        novaPosicao = painelPrincipal.VerticalScroll.Minimum;

                    if (novaPosicao > painelPrincipal.VerticalScroll.Maximum)
                        novaPosicao = painelPrincipal.VerticalScroll.Maximum;

                    painelPrincipal.VerticalScroll.Value = novaPosicao;
                    painelPrincipal.PerformLayout();
                };
            }

            foreach (Control child in controle.Controls)
            {
                AtivarScrollComRato(child, painelPrincipal);
            }
        }

        public void CarregarRoupaPacote(DataGridView dgv, int idPedido)
        {
            dgv.DataSource = null;
            dgv.Rows.Clear();
            dgv.Columns.Clear();

            dgv.AutoGenerateColumns = true;

            using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_DetalhesDaDevolucao", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@IDPedido", idPedido);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dgv.DataSource = dt;

                // esconder ID automaticamente
                if (dgv.Columns.Contains("ID"))
                    dgv.Columns["ID"].Visible = false;

                if (dgv.Columns.Contains("Modelo"))
                    dgv.Columns["Modelo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                if (dgv.Columns.Contains("Tamanho"))
                    dgv.Columns["Tamanho"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dgv.Columns["Tamanho"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


                if (dgv.Columns.Contains("QuantidadeDevolvida"))
                    dgv.Columns["QuantidadeDevolvida"].HeaderText = "Quantidade";
                dgv.Columns["QuantidadeDevolvida"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
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
                    if (reader.Read())
                    {
                        txtObs.Text = reader["Notas"].ToString();
                    }
                    else
                    {
                        txtObs.Text = null;
                    }
                }
            }
        }

        private List<string> ObterTamanhosPorModelo(string modelo)
        {
            var tamanhos = new List<string>();

            using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_GetTamanhosPorModelo", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@Modelo", modelo);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        tamanhos.Add(reader["Tamanho"].ToString());
                }
            }

            return tamanhos;
        }

        private void close_Click(object sender, EventArgs e)
        {
            MessageBox.Show(pnlConteudo.Size.ToString());
            //// Remove este UC da lista de controlos do painel
            //this.Parent.Controls.Remove(this);

            //// Opcional: Liberta a memória
            //this.Dispose();
        }
    }
}