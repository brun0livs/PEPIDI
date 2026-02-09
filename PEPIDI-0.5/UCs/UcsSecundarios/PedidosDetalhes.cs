using iText.Kernel.Pdf.Canvas.Wmf;
using PEPIDI_0._5.Models;
using PEPIDI.Organizers;
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
            CarregarPPacote(dgvPedidos, ID);
            Configura(dgvPedidos);
            Configura(dgvDevolucoes);
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

        public void CarregarPPacote(PEPIDIDataGridView dgv, int idPedido)
        {
            dgv.DataSource = null;
            dgv.Rows.Clear();
            dgv.Columns.Clear();

            VerComentario(idPedido);

            using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_DetalhesDoPedido", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@IDPedido", idPedido);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dgv.DataSource = dt;

                //AdicionarColunaQuantidade(dgv);
                //AdicionarColunaTamanho(dgv);

                foreach (DataGridViewColumn col in dgv.Columns)
                    col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            // usamos SEMPRE o field 'estado', não o parâmetro
            var eAtual = (Estado ?? "").Trim();

            if (eAtual.Equals("Pendente", StringComparison.OrdinalIgnoreCase))
            {
                if (dgv.Columns.Contains("TamanhoAtualizado"))
                    dgv.Columns["TamanhoAtualizado"].Visible = false;

                if (dgv.Columns.Contains("QuantidadePedida"))
                    dgv.Columns["QuantidadePedida"].Visible = false;

                if (dgv.Columns.Contains("ID"))
                    dgv.Columns["ID"].Visible = false;

                if (dgv.Columns.Contains("Modelo"))
                    dgv.Columns["Modelo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                if (dgv.Columns.Contains("Quantidade"))
                    dgv.Columns["Quantidade"].DisplayIndex = dgv.Columns.Count - 1;

                if (dgv.Columns.Contains("QuantidadeDisponivel"))
                    dgv.Columns["QuantidadeDisponivel"].HeaderText = "Quant. Disp.";

                // preencher combo de quantidade depois do binding
                dgv.BeginInvoke(new Action(() =>
                {
                    //FillComboQuanty(dgv);
                }));
            }
            else if (eAtual.Equals("Aprovado", StringComparison.OrdinalIgnoreCase))
            {
                if (dgv.Columns.Contains("TamanhoAtualizado"))
                    dgv.Columns["TamanhoAtualizado"].Visible = true;

                if (dgv.Columns.Contains("QuantidadePedida"))
                    dgv.Columns["QuantidadePedida"].Visible = true;

                if (dgv.Columns.Contains("Quantidade"))
                    dgv.Columns["Quantidade"].Visible = false;

                if (dgv.Columns.Contains("Tamanho"))
                    dgv.Columns["Tamanho"].Visible = false;

                if (dgv.Columns.Contains("ID"))
                    dgv.Columns["ID"].Visible = false;

                if (dgv.Columns.Contains("Modelo"))
                    dgv.Columns["Modelo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                if (dgv.Columns.Contains("Modelo") && dgv.Columns.Contains("TamanhoAtualizado"))
                {
                    int idxModelo = dgv.Columns["Modelo"].DisplayIndex;
                    dgv.Columns["TamanhoAtualizado"].DisplayIndex = idxModelo + 1;
                }

                if (dgv.Columns.Contains("TamanhoAtualizado"))
                    dgv.Columns["TamanhoAtualizado"].HeaderCell.Style.Alignment =
                        DataGridViewContentAlignment.MiddleCenter;

                // preencher combo de tamanho depois do binding
                dgv.BeginInvoke(new Action(() =>
                {
                    //FillComboSize(dgv);
                }));
            }

            // devolução de roupa
            //CarregarRoupaPacote(dgvDevolucoes, idPedido);

            // === AJUSTE VISUAL DAS COLUNAS ===
            try
            {
                // Evita auto-sizing automático lento
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

                // Define manualmente proporções (ajusta à tua DGV)
                if (dgv.Columns.Contains("Modelo"))
                {
                    dgv.Columns["Modelo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dgv.Columns["Modelo"].MinimumWidth = 180;
                }

                if (dgv.Columns.Contains("Tamanho") || dgv.Columns.Contains("TamanhoAtualizado"))
                {
                    var col = dgv.Columns.Contains("TamanhoAtualizado")
                        ? dgv.Columns["TamanhoAtualizado"]
                        : dgv.Columns["Tamanho"];

                    col.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    col.Width = 70;
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                if (dgv.Columns.Contains("QuantidadeDisponivel"))
                {
                    dgv.Columns["QuantidadeDisponivel"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    dgv.Columns["QuantidadeDisponivel"].Width = 100;
                    dgv.Columns["QuantidadeDisponivel"].HeaderText = "Quant. Disp.";
                    dgv.Columns["QuantidadeDisponivel"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                if (dgv.Columns.Contains("QuantidadePedida"))
                {
                    dgv.Columns["QuantidadePedida"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    dgv.Columns["QuantidadePedida"].Width = 110;
                    dgv.Columns["QuantidadePedida"].HeaderText = "Quant. Pedida";
                    dgv.Columns["QuantidadePedida"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                if (dgv.Columns.Contains("Quantidade"))
                {
                    dgv.Columns["Quantidade"].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
                    dgv.Columns["Quantidade"].Width = 90;
                    dgv.Columns["Quantidade"].HeaderText = "Quantidade";
                    dgv.Columns["Quantidade"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[DEBUG] Erro ao ajustar colunas: {ex.Message}");
            }
        }

        //private void AdicionarColunaTamanho(DataGridView dgv)
        //{
        //    const string nomeColuna = "TamanhoAtualizado";

        //    if (!dgv.Columns.Contains(nomeColuna))
        //    {
        //        // Trocamos aqui: de DataGridViewComboBoxColumn para PEPIDIComboBoxColumn
        //        var comboCol = new PEPIDIComboBoxColumn
        //        {
        //            Name = nomeColuna,
        //            HeaderText = "Tamanho",
        //            DisplayIndex = dgv.Columns.Count,
        //            DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing // Ajuda a manter o look clean
        //        };

        //        if (dgv.Columns.Contains("Quantidade") && dgv.Columns["Quantidade"] is PEPIDIComboBoxColumn qtdCol)
        //        {
        //            comboCol.Width = qtdCol.Width;
        //            comboCol.DefaultCellStyle = qtdCol.DefaultCellStyle.Clone() as DataGridViewCellStyle;
        //        }
        //        else
        //        {
        //            comboCol.Width = 80; // Um pouco mais largo para tamanhos
        //        }

        //        // O evento EditingControlShowing continua igual para garantir o DropDownList
        //        dgv.EditingControlShowing += (s, e) =>
        //        {
        //            if (e.Control is ComboBox combo && dgv.CurrentCell.OwningColumn.Name == nomeColuna)
        //            {
        //                combo.DropDownStyle = ComboBoxStyle.DropDownList;
        //            }
        //        };
        //        dgv.Columns.Add(comboCol);
        //    }
        //}

        //private void AdicionarColunaQuantidade(DataGridView dgv)
        //{
        //    const string nomeColuna = "Quantidade";

        //    if (!dgv.Columns.Contains(nomeColuna))
        //    {
        //        // Trocamos aqui também para a nossa coluna customizada
        //        var comboCol = new PEPIDIComboBoxColumn
        //        {
        //            Name = nomeColuna,
        //            HeaderText = "Quantidade",
        //            Width = 60,
        //            DisplayIndex = dgv.Columns.Count
        //        };

        //        dgv.EditingControlShowing += (s, e) =>
        //        {
        //            if (e.Control is ComboBox combo && dgv.CurrentCell.OwningColumn.Name == nomeColuna)
        //            {
        //                combo.DropDownStyle = ComboBoxStyle.DropDownList;
        //            }
        //        };

        //        for (int i = 0; i <= 10; i++) // Ajustado para exemplo
        //            comboCol.Items.Add(i.ToString());

        //        dgv.Columns.Add(comboCol);
        //    }
        //}

        //private void FillComboQuanty(DataGridView dgv)
        //{
        //    const string colunaOriginal = "QuantidadePedida";
        //    const string colunaCombo = "Quantidade";

        //    if (!dgv.Columns.Contains(colunaOriginal) || !dgv.Columns.Contains(colunaCombo))
        //        return;

        //    foreach (DataGridViewRow row in dgv.Rows)
        //    {
        //        if (row.IsNewRow) continue;

        //        int quantidadeOriginal = 0;
        //        object valOriginal = row.Cells[colunaOriginal].Value;

        //        if (valOriginal != null && int.TryParse(valOriginal.ToString(), out int parsed))
        //            quantidadeOriginal = parsed;

        //        row.Cells[colunaCombo].Value = quantidadeOriginal.ToString();
        //    }
        //}

        //private void FillComboSize(DataGridView dgv)
        //{
        //    foreach (DataGridViewRow row in dgv.Rows)
        //    {
        //        if (row.IsNewRow) continue;

        //        object modeloObj = row.Cells["Modelo"].Value;
        //        object tamanhoAtual = row.Cells["Tamanho"].Value;

        //        if (modeloObj == null) continue;

        //        string modelo = modeloObj.ToString();
        //        List<string> tamanhosValidos = ObterTamanhosPorModelo(modelo);

        //        // AQUI: Usamos a PEPIDIComboBoxCell em vez da default
        //        var combo = new PEPIDIComboBoxCell();

        //        foreach (var tam in tamanhosValidos)
        //            combo.Items.Add(tam);

        //        if (tamanhoAtual != null && tamanhosValidos.Contains(tamanhoAtual.ToString()))
        //            combo.Value = tamanhoAtual.ToString();
        //        else if (tamanhosValidos.Count > 0)
        //            combo.Value = tamanhosValidos[0];

        //        row.Cells["TamanhoAtualizado"] = combo;
        //    }
        //}

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
            // Remove este UC da lista de controlos do painel
            this.Parent.Controls.Remove(this);

            // Opcional: Liberta a memória
            this.Dispose();
        }
    }
}
