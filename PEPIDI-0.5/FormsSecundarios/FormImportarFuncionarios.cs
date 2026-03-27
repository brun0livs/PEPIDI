using Microsoft.Data.SqlClient;
using PEPIDI.Utils;
using PEPIDI.Organizers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PEPIDI.FormsSecundarios
{
    public partial class FormImportarFuncionarios : Form
    {
        int IDGestor;
        EfeitoUI M = new EfeitoUI();

        public FormImportarFuncionarios(int _IDGestor)
        {
            InitializeComponent();
            IDGestor = _IDGestor;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void FormImportarFuncionarios_Load(object sender, EventArgs e)
        {
            ConfigurarColunasGrid();
            dgvImport.Focus();
        }

        private void ConfigurarColunasGrid()
        {
            dgvImport.Columns.Clear();
            dgvImport.Columns.Add("Nr", "Nº Mecanográfico");
            dgvImport.Columns.Add("Nome", "Nome do Funcionário");
            dgvImport.Columns.Add("Funcao", "Função (Ex: Produção)");
            dgvImport.Columns.Add("DtAdmissao", "Data Admissão");
            dgvImport.Columns.Add("Estab", "Estabelecimento");

            dgvImport.Columns["Nr"].Width = 100;
            dgvImport.Columns["Funcao"].Width = 150;
            dgvImport.Columns["DtAdmissao"].Width = 100;
            dgvImport.Columns["Nome"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgvImport.AllowUserToAddRows = true;
            dgvImport.EditMode = DataGridViewEditMode.EditOnKeystroke;
        }

        private void dgvImport_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                try
                {
                    string textoClipboard = Clipboard.GetText();
                    string[] linhas = textoClipboard.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    int linhaAtual = dgvImport.CurrentRow != null ? dgvImport.CurrentRow.Index : 0;

                    dgvImport.SuspendLayout();
                    foreach (string linha in linhas)
                    {
                        string[] celulas = linha.Split('\t');
                        if (linhaAtual >= dgvImport.Rows.Count - 1) dgvImport.Rows.Add();

                        dgvImport["Nr", linhaAtual].Value = celulas.Length > 0 ? celulas[0].Trim() : "";
                        dgvImport["Nome", linhaAtual].Value = celulas.Length > 1 ? celulas[1].Trim() : "";

                        string funcExcel = celulas.Length > 2 ? celulas[2].Trim() : "";
                        string funcIA = MotorIA.CorrigirFuncao(funcExcel);

                        dgvImport["Funcao", linhaAtual].Value = funcIA;
                        dgvImport["Funcao", linhaAtual].Tag = funcExcel;

                        dgvImport["DtAdmissao", linhaAtual].Value = celulas.Length > 3 ? celulas[3].Trim() : DateTime.Now.ToShortDateString();
                        dgvImport["Estab", linhaAtual].Value = celulas.Length > 4 ? celulas[4].Trim() : "Sede";

                        if (funcIA == "Verificar Colagem")
                        {
                            dgvImport["Funcao", linhaAtual].Style.BackColor = Color.LightGoldenrodYellow;
                            dgvImport["Funcao", linhaAtual].Value = funcExcel;
                        }
                        else
                        {
                            dgvImport["Funcao", linhaAtual].Style.BackColor = Color.White;
                        }

                        linhaAtual++;
                    }
                    dgvImport.ResumeLayout();
                }
                catch (Exception ex) { MessageBox.Show("Erro na colagem: " + ex.Message); }
            }
        }

        private async void btnImportar_Click(object sender, EventArgs e)
        {
            // 1. Validação inicial
            if (dgvImport.Rows.Count <= 1) return;

            this.Cursor = Cursors.WaitCursor;
            btnImportar.Enabled = false;

            // 2. Criar uma lista local com os dados (Thread-Safe)
            var dadosParaImportar = new List<dynamic>();
            foreach (DataGridViewRow row in dgvImport.Rows)
            {
                if (row.IsNewRow) continue;
                dadosParaImportar.Add(new
                {
                    Index = row.Index,
                    Nr = row.Cells["Nr"].Value?.ToString() ?? "",
                    Nome = row.Cells["Nome"].Value?.ToString() ?? "",
                    Funcao = row.Cells["Funcao"].Value?.ToString() ?? "",
                    Data = row.Cells["DtAdmissao"].Value?.ToString() ?? "",
                    Estab = row.Cells["Estab"].Value?.ToString() ?? ""
                });
            }

            // 3. Carregar Dicionários (Cache)
            var funcoesValidas = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            var estabValidos = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            try
            {
                using (var conn = GetConn.GetConnection())
                {
                    conn.Open();
                    using (var rdr = new SqlCommand("SELECT ID, Nome FROM Funcoes", conn).ExecuteReader())
                        while (rdr.Read()) funcoesValidas.Add(rdr["Nome"].ToString().Trim(), Convert.ToInt32(rdr["ID"]));

                    using (var rdr = new SqlCommand("SELECT ID, Estab FROM Estab", conn).ExecuteReader())
                        while (rdr.Read()) estabValidos.Add(rdr["Estab"].ToString().Trim(), Convert.ToInt32(rdr["ID"]));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar tabelas: " + ex.Message);
                ResetBotao(); return;
            }

            int sucessos = 0;

            // 4. PROCESSAR EM BACKGROUND
            await Task.Run(() => {
                foreach (var item in dadosParaImportar)
                {
                    // Lógica do Estabelecimento (Oliveirinha ID 2 por defeito)
                    int idEstab = 2;
                    if (!string.IsNullOrEmpty(item.Estab) && !item.Estab.Equals("Sede", StringComparison.OrdinalIgnoreCase))
                    {
                        if (estabValidos.ContainsKey(item.Estab)) idEstab = estabValidos[item.Estab];
                    }

                    // Validação da Função
                    if (funcoesValidas.ContainsKey(item.Funcao))
                    {
                        try
                        {
                            int idFunc = funcoesValidas[item.Funcao];

                            // --- PONTO DE INTERROGAÇÃO PARA BREAKPOINT AQUI ---
                            ExecutarGravacaoNoSQL(item.Nr, item.Nome, idFunc, idEstab, item.Data);

                            MotorIA.AprenderNovaRegra(item.Nome, item.Funcao, "Funcao", false);
                            PintarLinha(item.Index, Color.LightGreen, Color.Black);
                            sucessos++;
                        }
                        catch (Exception ex) { PintarLinha(item.Index, Color.Red, Color.White, "Erro SQL: " + ex.Message); }
                    }
                    else { PintarLinha(item.Index, Color.Red, Color.White, "Função não existe na BD"); }
                }
                MotorIA.CarregarRegrasDaBD();
            });

            ResetBotao();
            M.AbrirMensagem($"Sucessos: {sucessos}", "PEPIDI");
        }

        // MÉTODOS AUXILIARES (THREAD-SAFE)
        private void PintarLinha(int index, Color fundo, Color letra, string msg = "")
        {
            this.Invoke((MethodInvoker)delegate {
                dgvImport.Rows[index].DefaultCellStyle.BackColor = fundo;
                dgvImport.Rows[index].DefaultCellStyle.ForeColor = letra;
                if (!string.IsNullOrEmpty(msg)) dgvImport.Rows[index].Cells["Funcao"].ToolTipText = msg;
            });
        }

        private void ResetBotao() { this.Cursor = Cursors.Default; btnImportar.Enabled = true; }

        private void ExecutarGravacaoNoSQL(string nr, string nome, int idFunc, int idEstab, string data)
        {
            using (var conn = GetConn.GetConnection())
            {
                conn.Open();
                string sql = @"IF EXISTS (SELECT 1 FROM Funcionarios WHERE NrMecanografico = @nr)
                        UPDATE Funcionarios SET Nome=@nome, FuncaoID=@fid, DataAdmissao=@dt, EstabID=@eid WHERE NrMecanografico=@nr
                       ELSE
                        INSERT INTO Funcionarios (NrMecanografico, Nome, FuncaoID, DataAdmissao, EstabID, Ativo, PalavraPasse) 
                        VALUES (@nr, @nome, @fid, @dt, @eid, 1, '1234')";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@nr", nr);
                    cmd.Parameters.AddWithValue("@nome", nome);
                    cmd.Parameters.AddWithValue("@fid", idFunc);
                    cmd.Parameters.AddWithValue("@eid", idEstab);

                    DateTime dt;
                    if (!DateTime.TryParse(data, out dt)) dt = DateTime.Now;
                    cmd.Parameters.AddWithValue("@dt", dt);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void GravarFuncionarioNoSQL(DataGridViewRow row, int idFuncao, int idEstab)
        {
            using (var conn = GetConn.GetConnection())
            {
                conn.Open();
                string sql = @"
                    IF EXISTS (SELECT 1 FROM Funcionarios WHERE NrMecanografico = @nr)
                        UPDATE Funcionarios SET Nome=@nome, FuncaoID=@fid, DataAdmissao=@dt, EstabID=@eid 
                        WHERE NrMecanografico=@nr
                    ELSE
                        INSERT INTO Funcionarios (NrMecanografico, Nome, FuncaoID, DataAdmissao, EstabID, Ativo, PalavraPasse) 
                        VALUES (@nr, @nome, @fid, @dt, @eid, 1, '1234')";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@nr", row.Cells["Nr"].Value?.ToString() ?? "");
                    cmd.Parameters.AddWithValue("@nome", row.Cells["Nome"].Value?.ToString() ?? "");
                    cmd.Parameters.AddWithValue("@fid", idFuncao);
                    cmd.Parameters.AddWithValue("@eid", idEstab);

                    DateTime dt;
                    if (!DateTime.TryParse(row.Cells["DtAdmissao"].Value?.ToString(), out dt)) dt = DateTime.Now;
                    cmd.Parameters.AddWithValue("@dt", dt);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void MarcarErro(DataGridViewRow row, string msg)
        {
            this.Invoke((MethodInvoker)delegate {
                row.DefaultCellStyle.BackColor = Color.Red;
                row.DefaultCellStyle.ForeColor = Color.White;
                row.Cells["Funcao"].ToolTipText = msg;
            });
        }

        private void dgvImport_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            string nomeColuna = dgvImport.Columns[e.ColumnIndex].Name;

            if (nomeColuna == "Funcao")
            {
                string colunaChave = "Nome";
                var valorChaveOriginal = dgvImport.Rows[e.RowIndex].Cells[colunaChave].Value?.ToString();
                var novoValorAtribuido = dgvImport.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();

                if (string.IsNullOrEmpty(valorChaveOriginal)) return;

                dgvImport.CellValueChanged -= dgvImport_CellValueChanged;
                foreach (DataGridViewRow row in dgvImport.Rows)
                {
                    if (row.Index != e.RowIndex && row.Cells[colunaChave].Value?.ToString() == valorChaveOriginal)
                    {
                        row.Cells[e.ColumnIndex].Value = novoValorAtribuido;
                        row.Cells[e.ColumnIndex].Style.BackColor = Color.White;
                        row.Cells[e.ColumnIndex].Style.ForeColor = Color.Black;
                    }
                }
                dgvImport.CellValueChanged += dgvImport_CellValueChanged;
            }
        }
    }
}