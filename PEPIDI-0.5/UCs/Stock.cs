using PEPIDI.Organizers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PEPIDI.UCs
{
    public partial class Stock : UserControl
    {
        private readonly string _cs = GetConn.ConnectionString;
        private DataTable _dtQueries;
        private PermissoesPerfil permissoes;
        EfeitoUI M = new EfeitoUI();

        public Stock(PermissoesPerfil _permissoes)
        {
            InitializeComponent();
            permissoes = _permissoes;
        }

        private void Stock_Load(object sender, EventArgs e)
        {
            CarregarCombo();
            TouchScrollHelper.AtivarScrollPorArrasto(dgvStock);
        }

        private void CarregarCombo()
        {
            try
            {
                using (var con = new SqlConnection(_cs))
                using (var cmd = new SqlCommand(@"SELECT ID, Nome, [Query] FROM dbo.[Query] ORDER BY Nome;", con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    _dtQueries = new DataTable();
                    da.Fill(_dtQueries);


                    if (permissoes.PodeAlterarDefinicoes)
                    {
                        // Adicionar linha "Gerir Filtros"
                        DataRow rowGerir = _dtQueries.NewRow();
                        rowGerir["ID"] = -1;
                        rowGerir["Nome"] = "---------------- Gerir Filtros";
                        rowGerir["Query"] = "ACTION_GERIR";
                        _dtQueries.Rows.Add(rowGerir);
                    }
                    

                    // === A CORREÇÃO ESTÁ AQUI ===
                    // 1. Define as propriedades PRIMEIRO
                    cmbVisoes.DisplayMember = "Nome";
                    cmbVisoes.ValueMember = "Query";

                    // 2. Define o DataSource DEPOIS. 
                    // Assim, quando o evento disparar, ele já sabe qual é o ValueMember correto.
                    cmbVisoes.DataSource = _dtQueries;

                    // 3. Define a seleção inicial
                    cmbVisoes.SelectedIndex = -1;
                }

                // Lógica de seleção por defeito (ID=1)
                if (_dtQueries != null && _dtQueries.Rows.Count > 0)
                {
                    foreach (DataRow row in _dtQueries.Rows)
                    {
                        if (row["ID"] != DBNull.Value && Convert.ToInt32(row["ID"]) == 1)
                        {
                            cmbVisoes.SelectedValue = row["Query"];
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                M.AbrirMensagem("Erro a carregar visões:" + Environment.NewLine + ex.Message, "Erro");
            }
        }

        private void cmbVisoes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbVisoes.SelectedIndex == -1 || cmbVisoes.SelectedValue == null) return;

            // PROTEÇÃO EXTRA:
            // Se por acaso vier um DataRowView (o objeto) em vez da string, ignoramos
            if (cmbVisoes.SelectedValue is System.Data.DataRowView) return;

            var sql = cmbVisoes.SelectedValue.ToString();

            // Lógica do botão Gerir
            if (sql == "ACTION_GERIR")
            {
                // new FormGerir().ShowDialog(); // Abre o teu form aqui
                M.AbrirMensagem("Janela de Gestão de Filtros.", "PEPIDI");
                return;
            }

            if (string.IsNullOrWhiteSpace(sql))
            {
                dgvStock.DataSource = null;
                return;
            }

            AplicarQueryNaDgv(sql);
        }

        private void AplicarQueryNaDgv(string sql)
        {
            try
            {
                using (var con = new SqlConnection(_cs))
                using (var da = new SqlDataAdapter(sql, con))
                {
                    var dt = new DataTable();
                    da.Fill(dt);

                    dgvStock.AutoGenerateColumns = true;
                    dgvStock.DataSource = dt;

                    // ============================================
                    // CONFIGURAÇÃO DA COR (ADICIONA ISTO AQUI)
                    // ============================================

                    // 1. Configura a coluna que tem o Texto (ex: "Armazém")
                    dgvStock.BadgeColumnName = "Departamento";

                    // 2. Configura a coluna que tem a Cor (ex: "#FF0000")
                    dgvStock.BadgeColorColumnName = "CorHex";

                    // 3. Esconde a coluna da cor para não ficar feio na tabela
                    if (dgvStock.Columns.Contains("CorHex"))
                    {
                        dgvStock.Columns["CorHex"].Visible = false;
                    }

                    // 4. Alinhamentos cosméticos
                    if (dgvStock.Columns.Contains("Tamanho"))
                    {
                        dgvStock.Columns["Tamanho"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }
                dgvStock.ReadOnly = true;
            }
            catch (Exception ex)
            {
                M.AbrirMensagem("Erro ao aplicar visão: " + Environment.NewLine + ex.Message, "Erro");
            }
        }
    }
}
