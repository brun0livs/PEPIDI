using Guna.UI2.WinForms;
using PEPIDI.Organizers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using PEPIDI.Utils;


namespace PEPIDI.FormsSecundarios
{
    public partial class FormFuncionario : Form
    {
        private int? NMEC = null;
        private int? IDGestor;
        EfeitoUI M = new();

        public FormFuncionario(int? _nr = null, int? _IDGestor = null)
        {
            InitializeComponent();
            NMEC = _nr;
            IDGestor = _IDGestor;
            CarregarComboFuncoes();
            CarregaComboEstabs();
            ConfigurarModo();
            GestorTema.AplicarEstilos(this);
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
                txtNr.Enabled = true; //Deixa escrever o Nº
                txtNome.Focus();
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
                            M.AbrirMensagem($"Funcionário com Nº {nr} não encontrado.", "Erro");
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
                        int? estab = rd["Estab"] == DBNull.Value ? (int?)null : Convert.ToInt32(rd["Estab"]);
                        string nomeEstab = rd["NomeEstab"]?.ToString() ?? "";

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

                        SelectByText(cmbEstab, nomeEstab);

                        Debug.WriteLine($"[EditFunc] Funcionário {nr} carregado com sucesso.");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[EditFunc] Erro em CarregarFun(): " + ex);
                M.AbrirMensagem("Erro ao carregar funcionário: " + ex.Message, "Erro");
            }
        }

        private void FormFuncionario_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
        }

        private void SelectByText(Guna2ComboBox cb, string text)
        {
            if (cb == null || string.IsNullOrWhiteSpace(text)) return;
            int idx = cb.FindStringExact(text);
            if (idx < 0) idx = cb.FindString(text); // permite correspondência parcial
            if (idx >= 0) cb.SelectedIndex = idx;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // 1. Validação prévia de campos obrigatórios no C# para poupar ida ao SQL
            if (string.IsNullOrWhiteSpace(txtNr.Text) || string.IsNullOrWhiteSpace(txtNome.Text))
            {
                M.AbrirMensagem("O Número e o Nome são obrigatórios.", "Dados em Falta");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
                using (SqlCommand cmd = new SqlCommand("sp_UPSERT_FUNC", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // 2. DEFINIR O MODO (A chave de tudo!)
                    // Se NMEC tem valor, estamos a editar ('U'). Se for null, é novo ('I').
                    bool isEdicao = this.NMEC.HasValue;
                    cmd.Parameters.AddWithValue("@Modo", isEdicao ? "U" : "I");

                    // 3. Identificação
                    // Se for edição, usamos o NMEC guardado. Se for novo, usamos o da caixa de texto.
                    int nrFunc = isEdicao ? this.NMEC.Value : int.Parse(txtNr.Text);
                    cmd.Parameters.AddWithValue("@Nr", nrFunc);

                    // 4. Dados Pessoais
                    cmd.Parameters.AddWithValue("@Nome", txtNome.Text.Trim());
                    cmd.Parameters.AddWithValue("@FuncaoId", cmbFuncoes.SelectedValue ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@EstabId", cmbEstab.SelectedValue ?? (object)DBNull.Value);

                    // Tratamento da Data de Admissão (pode ser nula?)
                    if (dtpDataAdmiss.Checked)
                        cmd.Parameters.AddWithValue("@DtAdmiss", dtpDataAdmiss.Value);
                    else
                        cmd.Parameters.AddWithValue("@DtAdmiss", DBNull.Value);

                    // 5. Fardamento (Tratamento de Strings Vazias vs Nulos)
                    // Função auxiliar para limpar a tralha das combos
                    object GetParam(string text) => string.IsNullOrWhiteSpace(text) ? (object)DBNull.Value : text;

                    cmd.Parameters.AddWithValue("@TShirt", GetParam(cmbTshirt.Text));
                    cmd.Parameters.AddWithValue("@Casaco", GetParam(cmbCasaco.Text));
                    cmd.Parameters.AddWithValue("@PoloMCurta", GetParam(cmbPolo.Text));
                    cmd.Parameters.AddWithValue("@PoloMCompr", GetParam(cmbPolomc.Text));

                    // Tratamento especial para Calça e Sapato que são INT na BD
                    // Se o texto não for número, manda NULL
                    if (int.TryParse(cmbCalca.Text, out int calcaVal))
                        cmd.Parameters.AddWithValue("@Calca", calcaVal);
                    else
                        cmd.Parameters.AddWithValue("@Calca", DBNull.Value);

                    if (int.TryParse(cmbSapato.Text, out int sapatoVal))
                        cmd.Parameters.AddWithValue("@Sapato", sapatoVal);
                    else
                        cmd.Parameters.AddWithValue("@Sapato", DBNull.Value);

                    // 6. Auditoria
                    if (isEdicao)
                        cmd.Parameters.AddWithValue("@AlteradoPor", this.IDGestor ?? (object)DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@CriadoPor", this.IDGestor ?? (object)DBNull.Value);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    GestorDeLogins.RegistarOuAtualizarLogin(nrFunc.ToString());

                    M.AbrirMensagem(isEdicao ? "Dados atualizados com sucesso!" : "Funcionário criado com sucesso!", "Sucesso");

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (SqlException ex)
            {
                // Apanha o erro do RAISERROR (Ex: "Número já existe")
                M.AbrirMensagem(ex.Message, "Erro de Validação");
            }
            catch (Exception ex)
            {
                M.AbrirMensagem("Erro inesperado: " + ex.Message, "Erro");
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
                        M.AbrirMensagem("Erro Crítico: A função 'Produção' não existe na Base de Dados!", "Erro de Configuração");

                        // Opcional: Selecionar o vazio para não dar erro visual
                        cmbFuncoes.SelectedValue = -1;
                    }
                }
                catch (Exception ex)
                {
                    M.AbrirMensagem("Erro ao carregar funções: " + ex.Message, "Erro");
                }
            }
        }

        private void CarregaComboEstabs()
        {
            using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT ID,Estab FROM Estab ORDER BY ID";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    cmbEstab.DataSource = dt;
                    cmbEstab.DisplayMember = "Estab";
                    cmbEstab.ValueMember = "ID";
                }
                catch (Exception ex)
                {
                    M.AbrirMensagem("Erro ao carregar estabelecimentos: " + ex.Message, "Erro");
                }
            }
        }

        private void lblFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormFuncionario_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                this.Close();
            }
        }

        private void txtNr_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Só aceita números (Digit) e teclas de controlo (como Backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // "Come" o evento, impedindo a letra de aparecer
            }
        }

        private void txtNr_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNr.Text)) return;

            if (VerificarSeNrExiste(txtNr.Text))
            {
                M.AbrirMensagem("Este Número Mecanográfico já está atribuído!", "Erro de Duplicado");
                txtNr.Clear();
                txtNr.Focus();
            }
        }

        private bool VerificarSeNrExiste(string nr)
        {
            using (SqlConnection conn = GetConn.GetConnection())
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Funcionarios WHERE Nr = @nr", conn);
                cmd.Parameters.AddWithValue("@nr", nr);
                return (int)cmd.ExecuteScalar() > 0;
            }
        }
    }
}
