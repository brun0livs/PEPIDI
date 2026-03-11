using Microsoft.Data.SqlClient;
using PEPIDI.UCs.UcsSecundarios; // Garante que o namespace da UC Artigo está aqui
using PEPIDI.Utils;
using PEPIDI.Organizers;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;


namespace PEPIDI.UCs
{
    public partial class CriarStock : UserControl
    {
        EfeitoUI M = new EfeitoUI();

        // 1. CARREGAMENTO DA DGV NO LOAD
        private void CriarStock_Load(object sender, EventArgs e)
        {
            // Query para preencher a tabela com os dados essenciais
            string sql = "SELECT ID, Modelo, Familia, Tamanho, Quantidade FROM EPI";
            PreencherTabela(sql);

            // Aplica os estilos (fontes, cores) definidos no teu GestorTema
            GestorTema.AplicarEstilos(this);
        }

        // 2. MÉTODO PARA ALIMENTAR A DATA GRID VIEW
        public void PreencherTabela(string sql)
        {
            try
            {
                using (var con = new SqlConnection(GetConn.ConnectionString))
                using (var da = new SqlDataAdapter(sql, con))
                {
                    var dt = new DataTable();
                    da.Fill(dt);

                    dgvStock.DataSource = dt;

                    // Esconde o ID mas mantém-no acessível para o CellClick
                    if (dgvStock.Columns.Contains("ID"))
                        dgvStock.Columns["ID"].Visible = false;

                    // Estética: Centraliza o texto da coluna Tamanho
                    if (dgvStock.Columns.Contains("Tamanho"))
                        dgvStock.Columns["Tamanho"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
            catch (Exception ex)
            {
                M.AbrirMensagem("Erro ao carregar stock: " + ex.Message, "Erro SQL");
            }
        }

        // 3. EVENTO CELLCLICK QUE ENVIA O ID PARA A UC ARTIGO
        private void dgvStock_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verifica se clicaste numa linha (e não no cabeçalho)
            if (e.RowIndex >= 0)
            {
                // Obtém o ID da linha selecionada
                int idSelecionado = Convert.ToInt32(dgvStock.Rows[e.RowIndex].Cells["ID"].Value);

                // Procura se a UC Artigo já existe dentro do pnlDetails
                var ucArtigo = pnlDetails.Controls.OfType<Artigo>().FirstOrDefault();

                if (ucArtigo == null)
                {
                    // Se não existe, cria a instância e adiciona ao painel
                    ucArtigo = new Artigo();
                    ucArtigo.Dock = DockStyle.Fill;
                    pnlDetails.Controls.Clear();
                    pnlDetails.Controls.Add(ucArtigo);
                }

                // Chama o método público da UC Artigo para carregar os dados
                ucArtigo.CarregarDadosParaEdicao(idSelecionado);
            }
        }
    }
}