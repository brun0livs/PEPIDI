using Guna.UI2.WinForms;
using Microsoft.Data.SqlClient;
using PEPIDI.Organizers;
using PEPIDI.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PEPIDI.UCs
{
    public partial class Graficos : UserControl
    {
        public Graficos()
        {
            InitializeComponent();
        }

        // 1. Atualizar o Load para ser Async e chamar tudo
        private async void Graficos_Load(object sender, EventArgs e)
        {
            GestorTema.AplicarEstilos(this);

            // Carrega tudo em paralelo (ou sequencial rápido) para não bloquear a UI
            await CarregarFuncionariosAsync();
            await CarregarFuncoesAsync();
            await CarregarFiltrosTextoAsync("Familia", flpFamilia);
            await CarregarFiltrosTextoAsync("Modelo", flpModelos);
            await CarregarFiltrosTextoAsync("Tamanho", flpTamanhos);
        }

        private async Task CarregarFuncoesAsync()

        {

            flpFuncoes.Visible = false;

            flpFuncoes.SuspendLayout();

            flpFuncoes.Controls.Clear();



            // Usamos um Dictionary ou KeyValuePair para guardar ID e Nome

            var lista = new List<KeyValuePair<int, string>>();

            string query = "SELECT ID, Nome FROM Funcoes ORDER BY Nome"; // Adicionei o ID



            await Task.Run(() =>

            {

                using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))

                {

                    using (SqlCommand cmd = new SqlCommand(query, conn))

                    {

                        try

                        {

                            conn.Open();

                            using (SqlDataReader r = cmd.ExecuteReader())

                            {

                                while (r.Read())

                                {

                                    int id = Convert.ToInt32(r["ID"]);

                                    string nome = r["Nome"].ToString().Trim();

                                    lista.Add(new KeyValuePair<int, string>(id, nome));

                                }

                            }

                        }

                        catch { }

                    }

                }

            });



            foreach (var item in lista)

            {

                CriarTagFuncao(item.Key, item.Value); // Passamos o ID e o Nome

            }



            flpFuncoes.ResumeLayout(true);

            flpFuncoes.Visible = true;

        }

        private void CriarTagFuncao(int idFuncao, string nomeFuncao)

        {

            Guna2Button tag = new Guna2Button();

            tag.Name = idFuncao.ToString(); // Guardamos o ID aqui!

            tag.Text = nomeFuncao;

            tag.Font = new Font("Roboto", 10F, FontStyle.Regular);



            int larguraTexto = TextRenderer.MeasureText(nomeFuncao, tag.Font).Width;

            tag.Size = new Size(larguraTexto + 30, 35);

            tag.BorderRadius = 15;

            tag.Cursor = Cursors.Hand;

            tag.Animated = true;

            tag.FillColor = Color.FromArgb(230, 232, 235);

            tag.ForeColor = Color.FromArgb(64, 64, 64);

            tag.Margin = new Padding(0, 0, 10, 10);

            tag.Tag = false; // Estado inicial: Desligado



            tag.Click += (s, e) =>

            {

                bool isLigado = (bool)tag.Tag;

                tag.Tag = !isLigado;

                tag.FillColor = (bool)tag.Tag ? Color.FromArgb(242, 103, 34) : Color.FromArgb(230, 232, 235);

                tag.ForeColor = (bool)tag.Tag ? Color.White : Color.FromArgb(64, 64, 64);



                // Opcional: Chama logo o método de atualizar a tabela ao clicar!

                // FiltrosWorking(true);

            };



            flpFuncoes.Controls.Add(tag);

        }

        // 2. Carregar a ComboBox de Funcionários
        private async Task CarregarFuncionariosAsync()
        {
            var dtFuncs = new DataTable();
            dtFuncs.Columns.Add("Nr", typeof(int));
            dtFuncs.Columns.Add("NomeCompleto", typeof(string));

            string query = "SELECT Nr, Nome FROM Funcionarios ORDER BY Nr";

            await Task.Run(() =>
            {
                using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        using (SqlDataReader r = cmd.ExecuteReader())
                        {
                            while (r.Read())
                            {
                                int nr = Convert.ToInt32(r["Nr"]);
                                string nome = r["Nome"].ToString();
                                dtFuncs.Rows.Add(nr, $"{nr} - {nome}");
                            }
                        }
                    }
                    catch { }
                }
            });

            guna2ComboBox1.DataSource = dtFuncs;
            guna2ComboBox1.DisplayMember = "NomeCompleto";
            guna2ComboBox1.ValueMember = "Nr";
            guna2ComboBox1.SelectedIndex = -1; // Começa vazio (sem filtro)
        }

        // 3. Método genérico para Famílias, Modelos e Tamanhos
        private async Task CarregarFiltrosTextoAsync(string coluna, FlowLayoutPanel painel)
        {
            painel.Visible = false;
            painel.SuspendLayout();
            painel.Controls.Clear();

            var lista = new List<string>();

            // Vai buscar apenas os valores únicos (DISTINCT) que não sejam vazios
            string query = $"SELECT DISTINCT {coluna} FROM EPI WHERE {coluna} IS NOT NULL AND {coluna} <> '' ORDER BY {coluna}";

            await Task.Run(() =>
            {
                using (SqlConnection conn = new SqlConnection(GetConn.ConnectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        using (SqlDataReader r = cmd.ExecuteReader())
                        {
                            while (r.Read()) lista.Add(r[coluna].ToString().Trim());
                        }
                    }
                    catch { }
                }
            });

            foreach (var item in lista)
            {
                CriarTagTexto(item, painel);
            }

            painel.ResumeLayout(true);
            painel.Visible = true;
        }

        // 4. Criação das tags de Texto (ligeiramente diferente da tag de Função porque usa o texto como ID)
        private void CriarTagTexto(string texto, FlowLayoutPanel painel)
        {
            Guna2Button tag = new Guna2Button();
            tag.Name = texto; // Guardamos o texto para ler mais tarde
            tag.Text = texto;
            tag.Font = new Font("Roboto", 10F, FontStyle.Regular);

            int larguraTexto = TextRenderer.MeasureText(texto, tag.Font).Width;
            tag.Size = new Size(larguraTexto + 30, 35);
            tag.BorderRadius = 15;
            tag.Cursor = Cursors.Hand;
            tag.Animated = true;
            tag.FillColor = Color.FromArgb(230, 232, 235);
            tag.ForeColor = Color.FromArgb(64, 64, 64);
            tag.Margin = new Padding(0, 0, 10, 10);
            tag.Tag = false; // Estado inicial: Desligado

            tag.Click += (s, e) =>
            {
                bool isLigado = (bool)tag.Tag;
                tag.Tag = !isLigado;
                tag.FillColor = (bool)tag.Tag ? Color.FromArgb(242, 103, 34) : Color.FromArgb(230, 232, 235);
                tag.ForeColor = (bool)tag.Tag ? Color.White : Color.FromArgb(64, 64, 64);

                // Opcional: FiltrosWorking(true);
            };

            painel.Controls.Add(tag);
        }

        // 5. Lógica para fechar o ecrã
        private void lblClose_Click(object sender, EventArgs e)
        {
            // Libertar memória:
            this.Parent.Controls.Remove(this);
            this.Dispose();
        }
    }
}
