using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace AgentePEPIDI
{
    public partial class FormDetalhesStock : Form
    {
        public FormDetalhesStock()
        {
            InitializeComponent();

            // Fica sempre por cima das outras janelas para o utilizador ver logo
            this.TopMost = true;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Detalhes de Stock em Falta";
        }

        private async void FormDetalhesStock_Load(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = CONN.GetConnection()) // Usa a tua ponte CONN
                {
                    await conn.OpenAsync();

                    // 1. Vai buscar o limite atual às definições
                    int limite = 5;
                    string sqlDef = "SELECT Valor FROM Definicoes WHERE Chave = 'StockMinimo'";
                    using (SqlCommand cmdDef = new SqlCommand(sqlDef, conn))
                    {
                        var result = await cmdDef.ExecuteScalarAsync();
                        if (result != null) limite = Convert.ToInt32(result);
                    }

                    // 2. A Query que vai buscar o NOME e a QUANTIDADE dos EPIs
                    // ATENÇÃO: Ajusta os nomes das colunas/tabelas conforme a tua BD real!
                    string query = @"SELECT 
                                        E.Modelo AS 'Equipamento', 
                                        S.Quant AS 'Quantidade Atual'
                                    FROM Stock S
                                    INNER JOIN EPI E ON S.Codigo= E.Codigo
                                    WHERE S.Quant <=@limite";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@limite", limite);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            // 3. Preenche a grelha do Guna!
                            dgvStockBaixo.DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar detalhes: " + ex.Message);
            }
        }
    }
}