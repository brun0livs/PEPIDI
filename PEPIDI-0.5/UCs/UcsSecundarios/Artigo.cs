using Guna.UI2.WinForms;
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
using Microsoft.Data.SqlClient;

namespace PEPIDI.UCs.UcsSecundarios
{
    public partial class Artigo : UserControl
    {
        int _id;
        string _estado;
        public Artigo(string estado, int id)
        {
            InitializeComponent();
            id = _id;
            estado = _estado;
        }

        private void Artigo_Load(object sender, EventArgs e)
        {
            CarregarCombo(cmbFamilia);
        }

        private void CarregarCombo(Guna2ComboBox combo)
        {
            DataTable dtFamilias = new DataTable();
            dtFamilias.Columns.Add("ID", typeof(int));
            dtFamilias.Columns.Add("NomeFamilia", typeof(string));
            dtFamilias.Columns.Add("Apresentacao", typeof(string));

            dtFamilias.Rows.Add(0, "Null", "Familía...");
            dtFamilias.Rows.Add(1, "TShirt", "T-Shirt");
            dtFamilias.Rows.Add(2, "Casaco", "Casaco");
            dtFamilias.Rows.Add(3, "PoloMCurta", "Polo Manga Curta");
            dtFamilias.Rows.Add(4, "PoloMCompr", "Polo Manga Comprida");
            dtFamilias.Rows.Add(5, "Calca", "Calça");
            dtFamilias.Rows.Add(6, "Sapato", "Sapato");

            combo.DataSource = dtFamilias;
            combo.DisplayMember = "Apresentacao";
            combo.ValueMember = "NomeFamilia";

            combo.SelectedIndex = 0;
        }

        private async void CarregarModelos(string familia)
        {
            DataTable dtModelos = new DataTable();
            dtModelos.Columns.Add("Modelo", typeof(string));

            dtModelos.Rows.Add("Modelo...");
            dtModelos.Rows.Add("+ Escrever Novo Modelo...");

            string query = "SELECT DISTINCT Modelo FROM EPI WHERE Familia = @Familia AND Modelo IS NOT NULL AND Modelo <> ''";

            // Executa a query pesada em background para não bugar a ComboBox principal
            await Task.Run(() =>
            {
                using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Familia", familia);
                        try
                        {
                            conn.Open();
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    dtModelos.Rows.Add(reader["Modelo"].ToString().Trim());
                                }
                            }
                        }
                        catch (Exception)
                        {
                            // Erro silencioso na thread background para não rebentar o programa
                        }
                    }
                }
            });

            cmbModelo.DataSource = dtModelos;
            cmbModelo.DisplayMember = "Modelo";
            cmbModelo.ValueMember = "Modelo";

            cmbModelo.DropDownStyle = ComboBoxStyle.DropDownList;

            if (cmbModelo.Items.Count > 0)
                cmbModelo.SelectedIndex = 0;
        }

        private void CarregarTamanhos(string familia)
        {
            DataTable dtTamanhos = new DataTable();
            dtTamanhos.Columns.Add("Valor", typeof(string));
            dtTamanhos.Columns.Add("Apresentacao", typeof(string));

            dtTamanhos.Rows.Add("Null", "Tamanho...");

            if (familia == "Sapato")
            {
                for (int i = 35; i <= 48; i++)
                    dtTamanhos.Rows.Add(i.ToString(), i.ToString());
            }
            else if (familia == "Calca")
            {
                for (int i = 36; i <= 58; i += 2)
                    dtTamanhos.Rows.Add(i.ToString(), i.ToString());
            }
            else if (familia != "Null")
            {
                string[] tamanhosLetras = { "XS", "S", "M", "L", "XL", "XXL", "3XL", "4XL" };
                foreach (string tamanho in tamanhosLetras)
                    dtTamanhos.Rows.Add(tamanho, tamanho);
            }

            cmbTamanho.DataSource = dtTamanhos;
            cmbTamanho.DisplayMember = "Apresentacao";
            cmbTamanho.ValueMember = "Valor";

            cmbTamanho.SelectedIndex = 0;
        }

        // ==========================================
        // 2. FUNÇÕES AUTORIZADAS (TAGS) - O MOTOR V8! 🚀
        // ==========================================
        private async Task CarregarFuncoesAsync()
        {
            // Esconde e congela o painel enquanto desenha
            flpFuncoes.Visible = false;
            flpFuncoes.SuspendLayout();
            flpFuncoes.Controls.Clear();

            List<string> listaFuncoes = new List<string>();
            string query = "SELECT Nome FROM Funcoes ORDER BY Nome";

            // 1. Busca os nomes todos num abrir e piscar de olhos noutra thread
            await Task.Run(() =>
            {
                using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        try
                        {
                            conn.Open();
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    listaFuncoes.Add(reader["Nome"].ToString().Trim());
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Erro ao carregar as funções: " + ex.Message, "Erro SQL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            });

            // 2. Com todos os nomes já em RAM, cospe os botões para o ecrã à bruta!
            foreach (string nomeFuncao in listaFuncoes)
            {
                CriarTagFuncao(nomeFuncao);
            }

            // 3. Liberta a besta!
            flpFuncoes.ResumeLayout(true);
            flpFuncoes.Visible = true;
        }

        private void CriarTagFuncao(string nomeFuncao)
        {
            Guna2Button tag = new Guna2Button();
            tag.Text = nomeFuncao;

            // 1. Definir a fonte e medir o tamanho
            tag.Font = new Font("Roboto", 10F, FontStyle.Regular);
            int larguraTexto = TextRenderer.MeasureText(nomeFuncao, tag.Font).Width;

            // Tamanho com folga (+30) para o texto respirar dentro do botão redondo
            tag.Size = new Size(larguraTexto + 30, 35);

            // 2. A MAGIA REDONDINHA DO GUNA!
            tag.BorderRadius = 15;
            tag.Cursor = Cursors.Hand;
            tag.Animated = true;

            // 3. Design Inicial (Desligado)
            tag.FillColor = Color.FromArgb(230, 232, 235); // Fundo Cinza
            tag.ForeColor = Color.FromArgb(64, 64, 64);    // Texto Escuro
            tag.Margin = new Padding(0, 0, 10, 10);

            // Usamos o "Tag" para saber se está ligado ou desligado
            tag.Tag = false;

            // 4. Evento para ligar/desligar ao clicar
            tag.Click += (s, e) =>
            {
                bool isLigado = (bool)tag.Tag;

                if (!isLigado)
                {
                    tag.FillColor = Color.FromArgb(242, 103, 34);
                    tag.ForeColor = Color.White;
                    tag.Tag = true;
                }
                else
                {
                    tag.FillColor = Color.FromArgb(230, 232, 235);
                    tag.ForeColor = Color.FromArgb(64, 64, 64);
                    tag.Tag = false;
                }
            };

            // Criar o controlo em memória RAM antes de o atirar para o painel!
            tag.CreateControl();
            flpFuncoes.Controls.Add(tag);
        }

        // ==========================================
        // 3. EVENTOS DA INTERFACE E VALIDAÇÕES
        // ==========================================
        private void cmbFamilia_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFamilia.SelectedValue == null) return;

            string familiaSelecionada = cmbFamilia.SelectedValue.ToString();
            CarregarTamanhos(familiaSelecionada);

            cmbModelo.Enabled = true;
            CarregarModelos(familiaSelecionada);

            ValidarTamanho();
        }

        private void ValidarTamanho()
        {
            bool ativarTamanho = false;

            if (cmbModelo.Text == "+ Escrever Novo Modelo...")
            {
                ativarTamanho = !string.IsNullOrWhiteSpace(txtNovoModeloEPI.Text);
            }
            else
            {
                string modeloEscolhido = cmbModelo.Text.Trim();
                ativarTamanho = !string.IsNullOrEmpty(modeloEscolhido) && modeloEscolhido != "Selecionar...";
            }

            cmbTamanho.Enabled = ativarTamanho;
        }

        private void cmbModelo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string modeloEscolhido = cmbModelo.Text.Trim();

            if (modeloEscolhido == "+ Escrever Novo Modelo...")
            {
                tlpModelo.ColumnStyles[0] = new ColumnStyle(SizeType.Percent, 0F);
                tlpModelo.ColumnStyles[1] = new ColumnStyle(SizeType.Percent, 100F);

                txtNovoModeloEPI.Enabled = true;
                txtNovoModeloEPI.Visible = true;
                txtNovoModeloEPI.Focus();
            }
            else
            {
                tlpModelo.ColumnStyles[1] = new ColumnStyle(SizeType.Percent, 0F);
                tlpModelo.ColumnStyles[0] = new ColumnStyle(SizeType.Percent, 100F);
            }

            ValidarTamanho();
        }

        private void txtNovoModelo_TextChanged(object sender, EventArgs e)
        {
            ValidarTamanho();
        }
    }
}
