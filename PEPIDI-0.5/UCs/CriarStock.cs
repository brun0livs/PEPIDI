using Microsoft.Data.SqlClient;
using PEPIDI.Organizers;
using PEPIDI.UCs.UcsSecundarios; // Garante que o namespace da UC Artigo está aqui
using PEPIDI.Utils;
using System.Data;
using System.Runtime.InteropServices;


namespace PEPIDI.UCs
{
    public partial class CriarStock : UserControl
    {
        EfeitoUI M = new();

        private bool BtnCriar;
        int Gestor;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);
        private const int WM_SETREDRAW = 11;

        public CriarStock(int _Gestor, bool _BtnNovo)
        {
            InitializeComponent();
            BtnCriar = _BtnNovo;
            Gestor = _Gestor;
        }

        // 1. CARREGAMENTO DA DGV NO LOAD
        private void CriarStock_Load(object sender, EventArgs e)
        {
            // Query para preencher a tabela com os dados essenciais
            string sql = "SELECT ID, Modelo, Familia, Tamanho, Quantidade FROM EPI";
            PreencherTabela(sql);

            // Aplica os estilos (fontes, cores) definidos no teu GestorTema
            GestorTema.AplicarEstilos(this);
            if (BtnCriar)
            {
                btnCriarEPI.Visible = true;
                btnCriarEPI.Enabled = true;
                btnImportarEPI.Visible = true;
                btnImportarEPI.Enabled = true;
            }
            TouchScrollHelper.AtivarScrollPorArrasto(dgvStock);
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
            string estado = "Editar";
            // Verifica se clicaste numa linha (e não no cabeçalho)
            if (e.RowIndex >= 0)
            {
                int id = Convert.ToInt16(dgvStock.Rows[e.RowIndex].Cells["ID"].Value);
                AbrirControl(new Artigo(estado, id, Gestor));
            }
        }

        public void AbrirControl(UserControl control)
        {
            // 1. Para o desenho do painel (evita o lag visual)
            SendMessage(pnlDetails.Handle, WM_SETREDRAW, false, 0);

            // 2. Antes de limpar, é boa prática fazer Dispose para libertar memória
            foreach (Control c in pnlDetails.Controls)
            {
                c.Dispose();
            }
            pnlDetails.Controls.Clear();

            // ============================================================
            // 3. SE O CONTROLO FOR DO TIPO "Artigo", SUBSCREVEMOS O EVENTO
            // ============================================================
            if (control is Artigo ucArtigo)
            {
                ucArtigo.ArtigoGuardado += (s, args) =>
                {
                    // Substitui pela tua query real de listagem
                    string sqlRefresh = "SELECT ID, Modelo, Familia, Tamanho, Quantidade FROM EPI";
                    PreencherTabela(sqlRefresh);
                };
            }

            // 4. Configurar e adicionar o novo controlo
            control.Dock = DockStyle.Fill;
            pnlDetails.Controls.Add(control);

            // 5. Retoma o desenho
            SendMessage(pnlDetails.Handle, WM_SETREDRAW, true, 0);
            pnlDetails.Refresh();
        }

        private void btnImportarEPI_Click(object sender, EventArgs e)
        {
            // 1. Identificar o Form principal (Pai) para a sombra ocupar o ecrã todo
            Form pai = this.FindForm();

            using (Form overlay = new Form())
            {
                // Configurações da Sombra
                overlay.StartPosition = FormStartPosition.Manual;
                overlay.FormBorderStyle = FormBorderStyle.None;
                overlay.Opacity = 0.50d;
                overlay.BackColor = Color.Black;
                overlay.ShowInTaskbar = false;

                if (pai != null)
                {
                    overlay.Location = pai.Location;
                    overlay.Size = pai.Size;
                    overlay.Show(pai); // Mostra a sombra por cima do Form principal
                }

                // 2. Abrir o Form de Importação
                // Nota: Verifica se o caminho/nome do Form está correto no teu projeto
                using (FormsSecundarios.FormImportarStock frm = new FormsSecundarios.FormImportarStock())
                {
                    frm.ShowDialog(overlay); // Abre o form centralizado na sombra
                }

                // 3. Fechar a sombra e atualizar a tabela caso tenham sido importados dados novos
                overlay.Close();

                // Atualiza a DGV para mostrar o que foi importado
                string sql = "SELECT ID, Modelo, Familia, Tamanho, Quantidade FROM EPI";
                PreencherTabela(sql);
            }
        }

        private void btnCriarEPI_Click(object sender, EventArgs e)
        {
            // "Criar" é o estado, 0 é o ID (novo), Gestor é a variável global que já tens na UC
            AbrirControl(new Artigo("Criar", 0, Gestor));
        }
    }
}