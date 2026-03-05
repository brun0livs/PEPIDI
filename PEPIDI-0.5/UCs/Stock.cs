using PEPIDI.Organizers;
using PEPIDI.Utils;
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
            // Ativa a performance turbo que criaste no Organizer
            HelperPerformance.AtivarDoubleBufferRecursivo(this);
            CarregarCombo();
            TouchScrollHelper.AtivarScrollPorArrasto(dgvStock);

        }

        private async void CarregarCombo()
        {
            try
            {
                DataTable dtTemp = new DataTable();

                // 1. Busca os dados em background (SQL fora da UI Thread)
                await Task.Run(() => {
                    using (var con = new SqlConnection(_cs))
                    using (var cmd = new SqlCommand(@"SELECT ID, Nome, [Query] FROM dbo.[Query] ORDER BY Nome;", con))
                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dtTemp);
                    }
                });

                // 2. Adiciona a linha de gestão se tiver permissões
                if (permissoes.PodeAlterarDefinicoes)
                {
                    DataRow rowGerir = dtTemp.NewRow();
                    rowGerir["ID"] = -1;
                    rowGerir["Nome"] = "---------------- Gerir Filtros";
                    rowGerir["Query"] = "ACTION_GERIR";
                    dtTemp.Rows.Add(rowGerir);
                }

                _dtQueries = dtTemp;

                // 3. Vinculação de dados (UI Thread)
                // Definimos os membros ANTES do DataSource para evitar erros de cast
                cmbVisoes.DisplayMember = "Nome";
                cmbVisoes.ValueMember = "Query";

                // Atribuir o DataSource dispara o SelectedIndexChanged automaticamente
                cmbVisoes.DataSource = _dtQueries;

                // 4. Lógica para encontrar o ID 1 e expor no Load
                if (_dtQueries.Rows.Count > 0)
                {
                    // Lemos o ID que guardaste nas definições do projeto
                    int idDefault = Properties.Settings.Default.VisaoStockDefault;

                    DataRow[] linhasIdDefault = _dtQueries.Select($"ID = {idDefault}");

                    if (linhasIdDefault.Length > 0)
                    {
                        cmbVisoes.SelectedValue = linhasIdDefault[0]["Query"].ToString();
                    }
                    else
                    {
                        // Se o ID das definições não for encontrado, volta para o primeiro da lista
                        cmbVisoes.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                M.AbrirMensagem("Erro a carregar visões: " + ex.Message, "Erro");
            }
        }

        private void cmbVisoes_SelectedIndexChanged(object sender, EventArgs e)
{
    // Proteção essencial para o primeiro carregamento
    if (cmbVisoes.SelectedValue == null || cmbVisoes.SelectedValue is DataRowView)
        return;

    string sql = cmbVisoes.SelectedValue.ToString();

    if (sql == "ACTION_GERIR")
    {
        // 1. Descobrir quem é o "Pai" (O Form principal que está aberto)
        Form pai = this.FindForm(); 

        // 2. Criar a Sombra (Overlay)
        using (Form overlay = new Form())
        {
            // Configuração da sombra
            overlay.StartPosition = FormStartPosition.Manual;
            overlay.FormBorderStyle = FormBorderStyle.None;
            overlay.Opacity = 0.50d;
            overlay.BackColor = Color.Black;
            overlay.ShowInTaskbar = false;

            if (pai != null)
            {
                overlay.Location = pai.Location;
                overlay.Size = pai.Size;
                overlay.Show(pai);
            }
            else
            {
                overlay.WindowState = FormWindowState.Maximized;
                overlay.Show();
            }

            // 3. Abrir o teu FormGestaoDeFiltros POR CIMA da sombra
            using (PEPIDI.FormsSecundarios.FormGestaoDeFiltros frm = new PEPIDI.FormsSecundarios.FormGestaoDeFiltros())
            {
                // Passamos o 'overlay' para o ShowDialog, para ele saber que tem de ficar colado à sombra
                frm.ShowDialog(overlay); 
            }

            // 4. Fechar a sombra logo a seguir ao FormGestaoDeFiltros ser fechado
            overlay.Close();
        }

        // 5. ATUALIZAR A COMBOBOX DEPOIS DA GESTÃO FECHAR!
        CarregarCombo();

        // 6. Abortar a missão para não tentar enviar o "ACTION_GERIR" para a grelha
        return; 
    }

    // Se for uma Query normal, aplica na grelha!
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
