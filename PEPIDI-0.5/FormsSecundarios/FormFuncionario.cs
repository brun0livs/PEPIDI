using Guna.UI2.WinForms;
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
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace PEPIDI.FormsSecundarios
{
    public partial class FormFuncionario : Form
    {
        private int? NMEC = null;
        private int? IDGestor;

        public FormFuncionario(int? _nr = null, int? _IDGestor = null)
        {
            InitializeComponent();
            NMEC = _nr;
            IDGestor = _IDGestor;
            CarregarComboFuncoes();
            ConfigurarModo();
        }

        private void ConfigurarModo()
        {
            if (NMEC.HasValue)
            {
                // --- MODO EDITAR ---
                this.Text = "Editar Funcionário";
                txtNr.Text = NMEC.Value.ToString();
                txtNr.Enabled = false; // Bloqueia a edição do Nº (ou .ReadOnly = true)

                // IMPORTANTE: Aqui deves chamar um método para carregar os dados da BD para as caixas
                CarregarFunc(NMEC.Value);
            }
            else
            {
                // --- MODO CRIAR ---
                this.Text = "Novo Funcionário";
                txtNr.Text = "";
                txtNr.Enabled = true; // Deixa escrever o Nº
            }
        }

        private void CarregarFunc(int nr)
        {
            try
            {
                string cs = GetConn.ConnectionString;

                using (SqlConnection cn = new SqlConnection(cs))
                using (SqlCommand cmd = new SqlCommand("sp_ObterFuncionarioPorNr", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nr", nr);

                    cn.Open();
                    using (SqlDataReader rd = cmd.ExecuteReader())
                    {
                        if (!rd.Read())
                        {
                            MessageBox.Show($"Funcionário com Nº {nr} não encontrado.", "PEPIDI");
                            return;
                        }

                        // === Leitura dos campos ===
                        string nome = rd["Nome"]?.ToString() ?? "";
                        int? funcaoId = rd["Funcao"] == DBNull.Value ? (int?)null : Convert.ToInt32(rd["Funcao"]);
                        string nomeFuncao = rd["NomeFuncao"]?.ToString() ?? "";

                        string tshirt = rd["TShirt"]?.ToString() ?? "-";
                        string casaco = rd["Casaco"]?.ToString() ?? "-";
                        string poloCurta = rd["PoloMCurta"]?.ToString() ?? "-";
                        string poloCompr = rd["PoloMCompr"]?.ToString() ?? "-";
                        string calca = rd["Calca"]?.ToString() ?? "-";
                        string sapato = rd["Sapato"]?.ToString() ?? "-";

                        DateTime? dtAdmiss = rd["DtAdmiss"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rd["DtAdmiss"]);
                        string estab = rd["Estab"]?.ToString() ?? "";

                        txtNome.Text = nome;

                        // Função
                        SelectByText(cmbFuncoes, nomeFuncao);

                        // Fardamento
                        SelectByText(cmbTshirt, tshirt);
                        SelectByText(cmbCasaco, casaco);
                        SelectByText(cmbPolo, poloCurta);
                        SelectByText(cmbPolomc, poloCompr);
                        SelectByText(cmbCalca, calca);
                        SelectByText(cmbSapato, sapato);

                        if (dtAdmiss.HasValue)
                        {
                            dtpDataAdmiss.Value = dtAdmiss.Value;
                            dtpDataAdmiss.Checked = true;
                        }
                        else
                        {
                            dtpDataAdmiss.Checked = false;
                        }

                        SelectByText(cmbEstab, estab);

                        Debug.WriteLine($"[EditFunc] Funcionário {nr} carregado com sucesso.");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[EditFunc] Erro em CarregarFun(): " + ex);
                MessageBox.Show("Erro ao carregar funcionário: " + ex.Message, "PEPIDI");
            }
        }

        private void FormFuncionario_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
        }

        private void SelectByText(Guna2ComboBox cb , string text)
        {
            if (cb == null || string.IsNullOrWhiteSpace(text)) return;
            int idx = cb.FindStringExact(text);
            if (idx < 0) idx = cb.FindString(text); // permite correspondência parcial
            if (idx >= 0) cb.SelectedIndex = idx;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_UPSERT_FUNC", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                // --- 1. IDs e Campos Obrigatórios ---
                if (this.NMEC == null) cmd.Parameters.AddWithValue("@Nr", txtNr.Text);
                else cmd.Parameters.AddWithValue("@Nr", this.NMEC);

                cmd.Parameters.AddWithValue("@Nome", txtNome.Text);

                // Para a ComboBox da Função, enviamos o SelectedValue (o ID da função)
                // Se a combo estiver vazia, cuidado para não dar erro
                if (cmbFuncoes.SelectedValue != null)
                    cmd.Parameters.AddWithValue("@FuncaoId", Convert.ToInt32(cmbFuncoes.SelectedValue));
                else
                    cmd.Parameters.AddWithValue("@FuncaoId", DBNull.Value);

                // --- 2. Os Tamanhos (Textos das ComboBoxes) ---
                // Aqui usamos .Text ou .SelectedItem.ToString()
                cmd.Parameters.AddWithValue("@TShirt", cmbTshirt.Text);
                cmd.Parameters.AddWithValue("@Casaco", cmbCasaco.Text);
                cmd.Parameters.AddWithValue("@PoloMCurta", cmbPolo.Text);
                cmd.Parameters.AddWithValue("@PoloMCompr", cmbPolomc.Text);
                cmd.Parameters.AddWithValue("@Calca", cmbCalca.Text);
                cmd.Parameters.AddWithValue("@Sapato", cmbSapato.Text);

                // --- 3. Outros Dados ---
                cmd.Parameters.AddWithValue("@DtAdmiss", dtpDataAdmiss.Value);
                cmd.Parameters.AddWithValue("@Estab", cmbEstab.Text);
                cmd.Parameters.AddWithValue("@CriadoPor", this.IDGestor); // O teu ID de gestor

                try
                {
                    conn.Open();
                    var res = cmd.ExecuteScalar(); // Recebe o novo ID
                    MessageBox.Show($"Funcionário gravado com sucesso! ID: {res}");

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao guardar: " + ex.Message);
                }
            }
        }

        private void CarregarComboFuncoes()
        {
            using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
            {
                try
                {
                    conn.Open();
                    // 1. A Query para ir buscar os dados
                    string query = "SELECT ID, Nome FROM Funcoes ORDER BY ID";

                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    cmbFuncoes.DataSource = dt;
                    cmbFuncoes.DisplayMember = "Nome";
                    cmbFuncoes.ValueMember = "ID";

                    // 1. Procura a posição exata do texto "Produção"
                    int index = cmbFuncoes.FindStringExact("Produção");

                    // 2. Verifica se encontrou
                    if (index != -1)
                    {
                        // Se encontrou, seleciona essa linha
                        cmbFuncoes.SelectedIndex = index;
                    }
                    else
                    {
                        // Se NÃO encontrou, mostra erro e fecha ou seleciona o vazio
                        MessageBox.Show("Erro Crítico: A função 'Produção' não existe na Base de Dados!", "Erro de Configuração", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        // Opcional: Selecionar o vazio para não dar erro visual
                        cmbFuncoes.SelectedValue = -1;
                    }
                }
                catch (Exception ex)   
                {
                    MessageBox.Show("Erro ao carregar funções: " + ex.Message);
                }
            }
        }

        private void lblFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormFuncionario_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
