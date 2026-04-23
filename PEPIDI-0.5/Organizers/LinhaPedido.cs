using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using Guna.UI2.WinForms;

namespace PEPIDI.Organizers
{
    public partial class LinhaPedido : UserControl
    {
        private int _quantidade = 0;
        // Variáveis de controlo de estado
        private string _tamanhoAnterior = null;
        private bool _isReverting = false;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int Quantidade
        {
            get => _quantidade;
            set { _quantidade = Math.Max(0, value); lblQuantidade.Text = _quantidade.ToString(); }
        }

        // Texto do item
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

        // Texto do item
        public string DescricaoEPI
        {
            get => label1.Text;
            set => label1.Text = value;
        }

        // Acede ao combo real de tamanhos
        // O nome depois da seta (=>) TEM de ser o nome que deste no Designer (modern...)
        public Guna.UI2.WinForms.Guna2ComboBox ComboTamanho => ModerComboBoxTamanho;
        public Guna.UI2.WinForms.Guna2ComboBox ComboModelo => moderComboBoxModelo;

        public LinhaPedido(Guna2ComboBox comboTamanho)
        {
            this.ModerComboBoxTamanho = comboTamanho;
        }

        // Tamanho selecionado *no momento*
        public string TamanhoSelecionado => ComboTamanho.SelectedItem?.ToString() ?? string.Empty;

        public LinhaPedido()
        {
            InitializeComponent();
            Quantidade = 0;

            btnMais.Click += (s, e) => { if (Quantidade < 5) Quantidade++; };
            btnMenos.Click += (s, e) => { if (Quantidade > 0) Quantidade--; };

            ComboTamanho.SelectedIndexChanged += ComboTamanho_SelectedIndexChanged;

            // NOVO: Evento para quando o utilizador muda o modelo (ex: de Sapato Normal para Würth)
            ComboModelo.SelectedIndexChanged += ComboModelo_SelectedIndexChanged;

            this.Load += (s, e) => {
                ArredondarCantos(this, 20);
                ArredondarCantos(tlpQuant, 20);
                _tamanhoAnterior = ComboTamanho.SelectedItem?.ToString();
            };
        }

        private void ComboModelo_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Quando o modelo muda, precisamos de avisar o FormPedidos para recarregar os tamanhos 
            // deste novo modelo específico. 
            // Por agora, podes deixar uma chamada vazia ou disparar um evento customizado.
            // Mas o ideal é que o FormPedidos trate isto.
        }

        private void ArredondarCantos(Control ctrl, int raio)
        {
            if (ctrl.Width <= 0 || ctrl.Height <= 0) return;

            int d = raio * 2;
            var rect = new Rectangle(0, 0, ctrl.Width, ctrl.Height);

            using (var path = new System.Drawing.Drawing2D.GraphicsPath())
            {
                path.AddArc(rect.X, rect.Y, d, d, 180, 90);
                path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
                path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
                path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
                path.CloseFigure();

                // evita leak da Region anterior
                ctrl.Region?.Dispose();
                ctrl.Region = new Region(path);
            }

            ctrl.Resize += (s, ev) => ArredondarCantos(ctrl, raio);
        }

        public void DefinirTamanhoSemConfirmar(string tamanho)
        {
            _isReverting = true;
            if (!string.IsNullOrEmpty(tamanho) && ComboTamanho.Items.Contains(tamanho))
            {
                ComboTamanho.SelectedItem = tamanho;
                _tamanhoAnterior = tamanho;
            }
            _isReverting = false;
        }

        private void ComboTamanho_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isReverting) return; // Se estivermos a carregar ou a reverter, ignora

            string novoTamanho = ComboTamanho.SelectedItem?.ToString();

            // Se o tamanho mudou e já tínhamos um tamanho definido anteriormente
            if (!string.IsNullOrEmpty(_tamanhoAnterior) && novoTamanho != _tamanhoAnterior)
            {
                DialogResult dr = MessageBox.Show(
                    $"Deseja mesmo alterar o tamanho de '{_tamanhoAnterior}' para '{novoTamanho}'?",
                    "Confirmar Alteração",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (dr == DialogResult.Yes)
                {
                    _tamanhoAnterior = novoTamanho; // Atualiza o histórico
                }
                else
                {
                    _isReverting = true; // Bloqueia para não perguntar de novo ao reverter
                    ComboTamanho.SelectedItem = _tamanhoAnterior; // Volta atrás
                    _isReverting = false;
                }
            }
        }

        public void AtualizarModelos(List<string> modelos)
        {
            ComboModelo.Items.Clear();

            if (modelos == null || modelos.Count <= 1)
            {
                // Esconde a label de "Modelo" e a Combo caso só haja 1 ou nenhum
                // Nota: Precisas de dar nomes às labels no Designer para as esconderes (ex: lblModeloTexto)
                label4.Visible = false; // "Modelo:"
                ComboModelo.Visible = false;

                if (modelos != null && modelos.Count == 1)
                    ComboModelo.Items.Add(modelos[0]);
            }
            else
            {
                // Se houver vários (ex: Würth, Bellota, etc), mostra as opções
                label4.Visible = true;
                ComboModelo.Visible = true;
                foreach (var m in modelos) ComboModelo.Items.Add(m);
                ComboModelo.SelectedIndex = 0;
            }
        }

        // Dentro da classe LinhaPedido.cs


        public void ConfigurarLayout(bool temVariosModelos, string familia, string modelo)
        {
            // 1. Nome bonito
            label1.Text = FormatarNome(familia, modelo, temVariosModelos);

            if (!temVariosModelos)
            {
                // ESCONDE os controlos do modelo
                label4.Visible = false;
                ComboModelo.Visible = false;

                // O PULO DO GATO:
                // Estica a Label pelas colunas 0, 1 e 2. 
                // Como o Dock está Fill e AutoSize False, ela vai ocupar o espaço 
                // da Descrição + Label Modelo + Combo Modelo automaticamente.
                tlpOP.SetColumnSpan(label1, 3);
            }
            else
            {
                // 1. MOSTRA os controlos
                label4.Visible = true;
                ComboModelo.Visible = true;

                // 2. VOLTA ao normal (ocupa só a coluna 0)
                tlpOP.SetColumnSpan(label1, 1);

                // 3. AJUSTE DAS PERCENTAGENS (Fine-Tuning)
                // Coluna 0: Label de Tipo (Descrição) -> 15%
                tlpOP.ColumnStyles[0].SizeType = SizeType.Percent;
                tlpOP.ColumnStyles[0].Width = 15F;

                // Coluna 1: Label "Modelo:" -> 8.5%
                tlpOP.ColumnStyles[1].SizeType = SizeType.Percent;
                tlpOP.ColumnStyles[1].Width = 8.5F;

                // Coluna 2: ComboBox do Modelo -> 31.5%
                tlpOP.ColumnStyles[2].SizeType = SizeType.Percent;
                tlpOP.ColumnStyles[2].Width = 31.5F;

                // NOTA: A soma destas três dá 55%, que é exatamente o que tinhas antes (27 + 8.5 + 18.5 + 1)
                // Garante que os restantes 45% estão bem distribuídos pelas colunas do Tamanho e Qtd no Designer.
            }
        }

        private string FormatarNome(string familia, string modelo, bool temVarios)
        {
            if (temVarios)
            {
                // Se tem vários modelos (ex: sapatos), mostramos o nome da Família "bonito"
                return familia.ToLower().Trim() switch
                {
                    "tshirt" => "T-Shirt",
                    "calca" => "Calças",
                    "sapato" => "Sapatos",
                    "casaco" => "Casaco",
                    _ => familia // Fallback
                };
            }

            // Se só tem um modelo, mostramos o nome do modelo diretamente
            return modelo;
        }
    }
}
