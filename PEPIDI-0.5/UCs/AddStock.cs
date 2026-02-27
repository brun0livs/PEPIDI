using PEPIDI.Organizers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace PEPIDI.UCs
{
    public partial class AddStock : UserControl
    {
        int IDGestor;
        private readonly string _cs = GetConn.ConnectionString;
        EfeitoUI M = new EfeitoUI();

        private DataTable _dtEpi;
        private DataView _viewEpi;

        private bool _eventosLigados = false;
        private bool _aAtualizarCombos = false; // para não disparar filtros quando sincronizamos com a linha
        private int _idEpiSelecionado = -1;
        private int _acessoSelecionado = -1;
        private int _qtdAdicionar = 0;


        private List<FuncaoItem> _todasFuncoes = new List<FuncaoItem>();

        private class FuncaoItem
        {
            public int ID { get; set; }
            public string Nome { get; set; }
            public override string ToString() => Nome;
        }

        public AddStock(int _IDGestor)
        {
            InitializeComponent();
            IDGestor = _IDGestor;
        }

        private void UCAddStock_Load(object sender, EventArgs e)
        {
            //CarregaDGV(dgvStock);
            //CarregarFuncoes();
            //PreencherCombosFamiliaModelo();
            //lblQuantAdici.Text = _qtdAdicionar.ToString();
        }

        private void CarregaDGV(DataGridView dgv)
        {

            string query = @"
                    SELECT 
                        E.ID,
                        E.Familia,
                        E.Modelo,
                        E.Tamanho,
                        E.Quantidade,
                        E.Acesso AS AcessoID,
                        COALESCE(agg.Departamentos, A.Descricao) AS Departamento
                    FROM dbo.EPI AS E
                    LEFT JOIN dbo.Acessos A ON A.ID = E.Acesso
                    OUTER APPLY (
                        SELECT STUFF((
                            SELECT DISTINCT ', ' + F.Nome
                            FROM dbo.AcessoFuncoes AF2
                            JOIN dbo.Funcoes F ON F.ID = AF2.FuncaoID
                            WHERE AF2.AcessoID = E.Acesso
                            FOR XML PATH(''), TYPE
                        ).value('.','nvarchar(max)'),1,2,'')
                    ) AS agg(Departamentos)
                    ORDER BY E.Modelo, E.Tamanho;";

            using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
            using (SqlDataAdapter da = new SqlDataAdapter(query, conn))
            {
                _dtEpi = new DataTable();
                da.Fill(_dtEpi);

                _viewEpi = new DataView(_dtEpi);

                dgv.AutoGenerateColumns = true;
                dgv.DataSource = _viewEpi;

                // Aparência base
                dgv.ReadOnly = true;
                dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgv.MultiSelect = false;
                dgv.AllowUserToAddRows = false;
                dgv.AllowUserToDeleteRows = false;
                dgv.RowHeadersVisible = false;
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // Ajustes de colunas
                if (dgv.Columns["ID"] != null)
                    dgv.Columns["ID"].Visible = false;
                if (dgv.Columns["AcessoID"] != null)
                    dgv.Columns["AcessoID"].Visible = false;

                if (dgv.Columns["Familia"] != null)
                    dgv.Columns["Familia"].HeaderText = "Família";
                if (dgv.Columns["Modelo"] != null)
                    dgv.Columns["Modelo"].HeaderText = "Modelo";
                if (dgv.Columns["Tamanho"] != null)
                    dgv.Columns["Tamanho"].HeaderText = "Tamanho";
                if (dgv.Columns["Quantidade"] != null)
                    dgv.Columns["Quantidade"].HeaderText = "Qtd";
                if (dgv.Columns["Departamento"] != null)
                    dgv.Columns["Departamento"].HeaderText = "Acesso / Dep.";
            }

        }

        // --------------------------
        // Combos Família / Modelo
        // --------------------------

        private void PreencherCombosFamiliaModelo()
        {
            if (_dtEpi == null || _dtEpi.Rows.Count == 0)
            {
                comboFamilia.Items.Clear();
                comboModelo.Items.Clear();
                return;
            }

            _aAtualizarCombos = true;

            comboFamilia.Items.Clear();
            comboModelo.Items.Clear();

            comboFamilia.Items.Add(""); // todos
            comboModelo.Items.Add("");  // todos

            var familias = _dtEpi.AsEnumerable()
                .Select(r => r.Field<string>("Familia"))
                .Where(f => !string.IsNullOrEmpty(f))
                .Distinct()
                .OrderBy(f => f)
                .ToList();

            comboFamilia.Items.AddRange(familias.ToArray());

            if (comboFamilia.Items.Count > 0)
                comboFamilia.SelectedIndex = 0;

            if (comboModelo.Items.Count > 0)
                comboModelo.SelectedIndex = 0;

            _aAtualizarCombos = false;
        }


        private void AtualizarModelosDaFamiliaInterno(string familiaSel)
        {
            comboModelo.Items.Clear();
            comboModelo.Items.Add(""); // todos

            IEnumerable<string> modelos;

            if (!string.IsNullOrEmpty(familiaSel))
            {
                modelos = _dtEpi.AsEnumerable()
                    .Where(r => r.Field<string>("Familia") == familiaSel)
                    .Select(r => r.Field<string>("Modelo"))
                    .Where(m => !string.IsNullOrEmpty(m))
                    .Distinct()
                    .OrderBy(m => m);
            }
            else
            {
                modelos = _dtEpi.AsEnumerable()
                    .Select(r => r.Field<string>("Modelo"))
                    .Where(m => !string.IsNullOrEmpty(m))
                    .Distinct()
                    .OrderBy(m => m);
            }

            comboModelo.Items.AddRange(modelos.ToArray());
        }

        private void AtualizarModelosDaFamilia()
        {
            if (_dtEpi == null) return;

            _aAtualizarCombos = true;

            string familiaSel = comboFamilia.SelectedItem?.ToString();

            comboModelo.Items.Clear();
            comboModelo.Items.Add(""); // todos

            IEnumerable<string> modelos;

            if (!string.IsNullOrEmpty(familiaSel))
            {
                modelos = _dtEpi.AsEnumerable()
                    .Where(r => r.Field<string>("Familia") == familiaSel)
                    .Select(r => r.Field<string>("Modelo"))
                    .Where(m => !string.IsNullOrEmpty(m))
                    .Distinct()
                    .OrderBy(m => m);
            }
            else
            {
                modelos = _dtEpi.AsEnumerable()
                    .Select(r => r.Field<string>("Modelo"))
                    .Where(m => !string.IsNullOrEmpty(m))
                    .Distinct()
                    .OrderBy(m => m);
            }

            comboModelo.Items.AddRange(modelos.ToArray());

            if (comboModelo.Items.Count > 0)
                comboModelo.SelectedIndex = 0;

            _aAtualizarCombos = false;
        }

        private void comboFamilia_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_aAtualizarCombos) return;

            AtualizarModelosDaFamilia();
            AtualizarFiltro();
        }

        private void comboModelo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_aAtualizarCombos) return;
            AtualizarFiltro();
        }

        private void AtualizarFiltro()
        {
            if (_viewEpi == null) return;

            var filtros = new List<string>();

            string familiaSel = comboFamilia.SelectedItem?.ToString();
            string modeloSel = comboModelo.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(familiaSel))
            {
                filtros.Add($"Familia = '{familiaSel.Replace("'", "''")}'");
            }

            if (!string.IsNullOrEmpty(modeloSel))
            {
                filtros.Add($"Modelo = '{modeloSel.Replace("'", "''")}'");
            }

            _viewEpi.RowFilter = filtros.Count > 0
                ? string.Join(" AND ", filtros)
                : "";

            // opcional: garantir seleção após filtro
            if (dgvStock.Rows.Count > 0)
            {
                dgvStock.ClearSelection();
                var row = dgvStock.Rows[0];
                row.Selected = true;
                dgvStock.CurrentCell = row.Cells["Modelo"];
            }
        }

        // --------------------------
        // Seleção da DGV -> atualiza tudo
        // --------------------------

        private void dgvStock_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvStock.CurrentRow == null || _dtEpi == null) return;

            var row = dgvStock.CurrentRow;

            if (row.Cells["ID"].Value == null || row.Cells["ID"].Value == DBNull.Value)
                return;

            _idEpiSelecionado = Convert.ToInt32(row.Cells["ID"].Value);

            if (row.Cells["AcessoID"].Value != null && row.Cells["AcessoID"].Value != DBNull.Value)
                _acessoSelecionado = Convert.ToInt32(row.Cells["AcessoID"].Value);
            else
                _acessoSelecionado = -1;

            // Quantidade
            int qtd = 0;
            if (row.Cells["Quantidade"].Value != null && row.Cells["Quantidade"].Value != DBNull.Value)
                qtd = Convert.ToInt32(row.Cells["Quantidade"].Value);

            lblQuantidade.Text = qtd.ToString();

            // Família / Modelo da linha
            string familia = row.Cells["Familia"].Value?.ToString();
            string modelo = row.Cells["Modelo"].Value?.ToString();

            _aAtualizarCombos = true;

            if (!string.IsNullOrEmpty(familia) && comboFamilia.Items.Contains(familia))
                comboFamilia.SelectedItem = familia;
            else
                comboFamilia.SelectedIndex = 0;

            AtualizarModelosDaFamiliaInterno(familia);

            if (!string.IsNullOrEmpty(modelo) && comboModelo.Items.Contains(modelo))
                comboModelo.SelectedItem = modelo;
            else
                comboModelo.SelectedIndex = 0;

            _aAtualizarCombos = false;

            // Funções do acesso
            MarcarFuncoesDoAcesso(_acessoSelecionado);

            _qtdAdicionar = 0;
            lblQuantAdici.Text = "0";
        }

        // --------------------------
        // Funções / Acessos
        // --------------------------

        private void CarregarFuncoes()
        {
            _todasFuncoes.Clear();
            clbFuncoesEsq.Items.Clear();
            clbFuncoesDir.Items.Clear();

            string query = "SELECT ID, Nome FROM Funcoes ORDER BY Nome";

            try
            {
                using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            _todasFuncoes.Add(new FuncaoItem
                            {
                                ID = dr.GetInt32(0),
                                Nome = dr.GetString(1)
                            });
                        }
                    }
                }

                if (_todasFuncoes.Count == 0)
                    return;

                int metade = (int)Math.Ceiling(_todasFuncoes.Count / 2.0);

                var esquerda = _todasFuncoes.Take(metade).ToList();
                var direita = _todasFuncoes.Skip(metade).ToList();

                clbFuncoesEsq.Items.AddRange(esquerda.Cast<object>().ToArray());
                clbFuncoesDir.Items.AddRange(direita.Cast<object>().ToArray());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar funções: " + ex.Message,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MarcarFuncoesDoAcesso(int acessoId)
        {
            // limpa checks
            for (int i = 0; i < clbFuncoesEsq.Items.Count; i++)
                clbFuncoesEsq.SetItemChecked(i, false);

            for (int i = 0; i < clbFuncoesDir.Items.Count; i++)
                clbFuncoesDir.SetItemChecked(i, false);

            if (acessoId <= 0) return;

            var funcoesDoAcesso = new HashSet<int>();

            string query = "SELECT FuncaoID FROM AcessoFuncoes WHERE AcessoID = @AcessoID";

            try
            {
                using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AcessoID", acessoId);
                    conn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                            funcoesDoAcesso.Add(dr.GetInt32(0));
                    }
                }

                for (int i = 0; i < clbFuncoesEsq.Items.Count; i++)
                {
                    if (clbFuncoesEsq.Items[i] is FuncaoItem item &&
                        funcoesDoAcesso.Contains(item.ID))
                    {
                        clbFuncoesEsq.SetItemChecked(i, true);
                    }
                }

                for (int i = 0; i < clbFuncoesDir.Items.Count; i++)
                {
                    if (clbFuncoesDir.Items[i] is FuncaoItem item &&
                        funcoesDoAcesso.Contains(item.ID))
                    {
                        clbFuncoesDir.SetItemChecked(i, true);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar funções do acesso: " + ex.Message,
                    "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void rbMenos_Click(object sender, EventArgs e)
        {
            if (_qtdAdicionar > 0)
            {
                _qtdAdicionar--;
                lblQuantAdici.Text = _qtdAdicionar.ToString();
            }
        }

        private void rbMais_Click(object sender, EventArgs e)
        {
            _qtdAdicionar++;
            lblQuantAdici.Text = _qtdAdicionar.ToString();
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            if (_idEpiSelecionado <= 0)
            {
                MessageBox.Show("Nenhum EPI selecionado.");
                return;
            }

            if (_qtdAdicionar <= 0)
            {
                MessageBox.Show("Indica uma quantidade válida.");
                return;
            }

            string query = "UPDATE EPI SET Quantidade = Quantidade + @Qtd WHERE ID = @ID";

            using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Qtd", _qtdAdicionar);
                cmd.Parameters.AddWithValue("@ID", _idEpiSelecionado);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Stock atualizado!");

        }


        private void btnReject_Click(object sender, EventArgs e)
        {
            M.AbrirMensagem("Falta isto!", "Erro");
        }
    }
}
