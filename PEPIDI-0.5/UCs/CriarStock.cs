using Guna.UI2.WinForms;
using Microsoft.Data.SqlClient;
using PEPIDI.Organizers;
using PEPIDI.UCs.UcsSecundarios;
using PEPIDI.Utils;
using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PEPIDI.UCs
{
    public partial class CriarStock : UserControl
    {
        EfeitoUI M = new();

        private bool BtnCriar;
        int Gestor;
        string funcao;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);
        private const int WM_SETREDRAW = 11;

        public CriarStock(int _Gestor, bool _BtnNovo, string _funcao)
        {
            InitializeComponent();
            BtnCriar = _BtnNovo;
            Gestor = _Gestor;
            funcao = _funcao;
        }

        // 1. CARREGAMENTO DA DGV NO LOAD
        private void CriarStock_Load(object sender, EventArgs e)
        {
            // Preencher os Estados na combo
            PreencherCombo(cmbEstado);

            // CORREÇÃO 1: Usar Convert.ToInt32 é o cinto de segurança do WinForms
            PreencherTabela(Convert.ToInt32(cmbEstado.SelectedValue));

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

        private void PreencherCombo(Guna2ComboBox combo)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Texto");
            dt.Columns.Add("Valor", typeof(int));

            dt.Rows.Add("Novo", 1);
            dt.Rows.Add("Usado", 2);
            dt.Rows.Add("Gasto", 3);

            combo.DataSource = dt;
            combo.DisplayMember = "Texto";
            combo.ValueMember = "Valor";

            combo.SelectedIndex = 0; // Por defeito seleciona "Novo" (Valor 1)
        }

        // 2. MÉTODO PARA ALIMENTAR A DATA GRID VIEW
        public void PreencherTabela(int idEstado)
        {
            try
            {
                // Query que junta a definição do EPI com o Stock filtrado pelo Estado
                string query = @"
            SELECT 
                E.Codigo,
                E.Modelo, 
                E.Familia, 
                E.Tamanho, 
                S.Quant AS Quantidade 
            FROM EPI E
            INNER JOIN Stock S ON E.Codigo = S.Codigo
            WHERE S.Estado = @estadoId AND E.Ativo = 1";

                using (var con = new SqlConnection(GetConn.ConnectionString))
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@estadoId", idEstado);

                    var da = new SqlDataAdapter(cmd);
                    var dt = new DataTable();
                    da.Fill(dt);

                    dgvStock.DataSource = dt;

                    // Formatação das colunas
                    if (dgvStock.Columns.Contains("Codigo") && funcao != "Programador")
                        dgvStock.Columns["Codigo"].Visible = false;

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
                // Agora vamos ler uma STRING (Codigo) em vez de um INT (ID)
                string codigo = dgvStock.Rows[e.RowIndex].Cells["Codigo"].Value.ToString();

                AbrirControl(new Artigo(estado, codigo, Gestor));
            }
        }

        public void  AbrirControl(UserControl control)
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
                    // CORREÇÃO 2: Em vez de passar uma query string, passamos o estado atual da Combo!
                    int estadoAtual = Convert.ToInt32(cmbEstado.SelectedValue);
                    PreencherTabela(estadoAtual);
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
            Form pai = this.FindForm();

            using (Form overlay = new Form())
            {
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

                using (FormsSecundarios.FormImportarStock frm = new FormsSecundarios.FormImportarStock())
                {
                    frm.ShowDialog(overlay);
                }

                overlay.Close();

                // CORREÇÃO 3: Atualiza a DGV baseada no Estado que a Combo tem no momento
                if (cmbEstado.SelectedValue != null)
                {
                    PreencherTabela(Convert.ToInt32(cmbEstado.SelectedValue));
                }
            }
        }

        private void btnCriarEPI_Click(object sender, EventArgs e)
        {
            // Passamos "" (vazio) em vez de 0, pois agora é uma string!
            AbrirControl(new Artigo("Criar", "", Gestor));
        }

        private void cmbEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Verifica se o valor selecionado é um inteiro válido (forma super segura)
            if (cmbEstado.SelectedValue is int idEstado)
            {
                PreencherTabela(idEstado);
            }
            else if (cmbEstado.SelectedValue != null && int.TryParse(cmbEstado.SelectedValue.ToString(), out int parsedId))
            {
                PreencherTabela(parsedId);
            }
        }
    }
}