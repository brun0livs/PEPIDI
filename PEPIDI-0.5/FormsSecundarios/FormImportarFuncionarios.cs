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
            MotorIA.CarregarRegrasDaBD();

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
                        // GUARDAMOS AQUI A FUNÇÃO ORIGINAL DO EXCEL NA TAG DA CÉLULA
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

                // Extraímos a função original para ensinar a IA corretamente!
                string funcOriginal = row.Cells["Funcao"].Tag?.ToString() ?? row.Cells["Funcao"].Value?.ToString() ?? "";

                dadosParaImportar.Add(new
                {
                    Index = row.Index,
                    Nr = row.Cells["Nr"].Value?.ToString() ?? "",
                    Nome = row.Cells["Nome"].Value?.ToString() ?? "",
                    FuncaoOriginal = funcOriginal,
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
            await Task.Run(() =>
            {
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

                            // 1º - GRAVAR O FUNCIONÁRIO (Para ele passar a existir no sistema)
                            ExecutarGravacaoNoSQL(item.Nr, item.Nome, idFunc, idEstab, item.Data);

                            // 2º - CRIAR O LOGIN COM HASH (Agora sim, já podemos associar a password)
                            GestorDeLogins.RegistarOuAtualizarLogin(item.Nr);

                            // 3º - ENSINAR A IA DA FORMA CERTA
                            MotorIA.AprenderNovaRegra(item.FuncaoOriginal, item.Funcao, "Funcao", false);

                            // Apenas incrementamos, já não pintamos de verde!
                            sucessos++;
                        }
                        catch (Exception ex)
                        {
                            PintarLinha(item.Index, Color.Red, Color.White, "Erro SQL: " + ex.Message);
                        }
                    }
                    else
                    {
                        PintarLinha(item.Index, Color.Red, Color.White, "Função não existe na BD");
                    }
                }
                MotorIA.CarregarRegrasDaBD();
            });

            // 5. LIMPEZA VISUAL DA GRELHA (Remover as linhas processadas com sucesso)
            // Feito de trás para a frente para não baralhar os Índices da DataGridView
            for (int i = dgvImport.Rows.Count - 1; i >= 0; i--)
            {
                DataGridViewRow row = dgvImport.Rows[i];
                if (row.IsNewRow) continue;

                // Se a linha NÃO está vermelha, é porque foi um sucesso absoluto. Limpamos do ecrã!
                if (row.DefaultCellStyle.BackColor != Color.Red)
                {
                    dgvImport.Rows.Remove(row);
                }
            }

            ResetBotao();
            ResetBotao();

            // 6. VERIFICAÇÃO FINAL: Sobraram erros?
            // Se Count for <= 1, significa que só lá está a linha vazia do fundo (tudo o resto foi sucesso e apagado)
            if (dgvImport.Rows.Count <= 1)
            {
                M.AbrirMensagem($"Importação concluída com perfeição! {sucessos} funcionários adicionados.", "PEPIDI - Sucesso");

                // Define o DialogResult e fecha automaticamente
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                // Se sobrou mais do que 1 linha, é porque há linhas a vermelho para o utilizador corrigir. NÃO FECHA.
                int errosPendentes = dgvImport.Rows.Count - 1; // Desconta a linha nova do fundo
                M.AbrirMensagem($"Foram importados {sucessos} funcionários.\n\nPor favor, corrige os {errosPendentes} registos a vermelho e volta a clicar em Importar.", "PEPIDI - Atenção");
            }
        }

        // MÉTODOS AUXILIARES (THREAD-SAFE)
        private void PintarLinha(int index, Color fundo, Color letra, string msg = "")
        {
            this.Invoke((MethodInvoker)delegate
            {
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
                string sql = @"IF EXISTS (SELECT 1 FROM Funcionarios WHERE Nr = @nr)
                        UPDATE Funcionarios SET Nome=@nome, Funcao=@fid, DtAdmiss=@dt, Estab=@eid, AlteradoPor=@idgestor WHERE Nr=@nr
                       ELSE
                        INSERT INTO Funcionarios (Nr, Nome, Funcao, DtAdmiss, Estab, CriadoPor)
                        VALUES (@nr, @nome, @fid, @dt, @eid, @idgestor)";

                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@nr", nr);
                    cmd.Parameters.AddWithValue("@nome", nome);
                    cmd.Parameters.AddWithValue("@fid", idFunc);
                    cmd.Parameters.AddWithValue("@eid", idEstab);
                    cmd.Parameters.AddWithValue("@idgestor", IDGestor);

                    DateTime dt;
                    if (!DateTime.TryParse(data, out dt)) dt = DateTime.Now;
                    cmd.Parameters.AddWithValue("@dt", dt);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void dgvImport_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // 1. Evita erros no cabeçalho ou em grelhas vazias
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            // 2. Identifica em que coluna estamos
            string nomeColuna = dgvImport.Columns[e.ColumnIndex].Name;

            if (nomeColuna == "Funcao")
            {
                // 3. Em vez de usarmos o "Nome", a nossa chave de comparação é a Tag (a palavra original do Excel)
                var tagOriginal = dgvImport.Rows[e.RowIndex].Cells["Funcao"].Tag;
                string palavraErradaDoExcel = tagOriginal != null ? tagOriginal.ToString() : "";

                string novoValorAtribuido = dgvImport.Rows[e.RowIndex].Cells["Funcao"].Value?.ToString();

                // Se a Tag estiver vazia, não há o que replicar
                if (string.IsNullOrEmpty(palavraErradaDoExcel)) return;

                // --- INÍCIO DA MAGIA ---
                // Desativamos temporariamente o evento para não entrar em loop infinito
                dgvImport.CellValueChanged -= dgvImport_CellValueChanged;

                foreach (DataGridViewRow row in dgvImport.Rows)
                {
                    if (row.IsNewRow) continue;

                    // Vamos ver qual é a Tag da linha que estamos a analisar
                    var tagDestino = row.Cells["Funcao"].Tag;
                    string palavraDestino = tagDestino != null ? tagDestino.ToString() : "";

                    // Se a palavra original do Excel for igual (ex: "quali" == "quali"), aplicamos a correção!
                    if (row.Index != e.RowIndex && palavraDestino == palavraErradaDoExcel)
                    {
                        // ...então adapta o resto!
                        row.Cells["Funcao"].Value = novoValorAtribuido;

                        // Limpa o amarelo (feedback visual de que agora está OK)
                        row.Cells["Funcao"].Style.BackColor = Color.White;
                        row.Cells["Funcao"].Style.ForeColor = Color.Black;
                    }
                }

                // Voltamos a ligar o evento
                dgvImport.CellValueChanged += dgvImport_CellValueChanged;
            }
        }
    }
}