using iText.Signatures;
using System;
using System.Drawing;
using System.Drawing.Drawing2D; // Importante para a suavidade da assinatura
using System.Windows.Forms;

namespace PEPIDI
{
    public partial class FormAssinatura : Form
    {
        // Variáveis para controlar o desenho da assinatura
        private bool isDrawing = false;
        private Point lastPoint;
        private Bitmap signatureBitmap;

        // Propriedade pública para devolver a imagem final ao formulário "pai"
        public Bitmap AssinaturaFinal { get; private set; }

        public FormAssinatura(string nomeFuncionario)
        {
            InitializeComponent();

            // 1. Configurar Título
            lblTitulo.Text = $"Confirmação de Movimentos: {nomeFuncionario}";

            // 2. Configurar o Texto Legal Obrigatório
            txtLegal.Text = "O Equipamento de Protecção Individual (EPI) é propriedade da DIATOSTA, sendo para uso exclusivo nas instalações da empresa e/ou durante serviços prestados à empresa;\n\n" +
                            "É da responsabilidade do colaborador zelar pelo bom estado de limpeza, higiene e conservação do(s) EPI(s) entregue(s);\n\n" +
                            "Deve ser comunicado, ao superior hierárquico, sempre que o(s) EPI(s) seja(m) danificado(s);\n\n" +
                            "O(s) EPI(s) danificado(s) deve(m) ser devolvido(s) para ser substituido(s), ou reposto(s), pelo colaborador, caso seja concluido que o dano resulta de má utilização e pouco zelo.\n\n" +
                            "Declaro que recebi os Equipamentos de Protecção Individual abaixo mencionados, informação quanto aos riscos do meu posto de trabalho, comprometendo-me a utilizá-los correctamente de acordo com as instruções recebidas, a conservá-los e mantê-los em bom estado, e a participar todas as avarias ou deficiências de que tenha conhecimento. Em caso de: perda ou má utilização; cessação contratual; abandono do posto de trabalho e demais situações, ou caso não seja feita a devolução do fardamento e EPIs no final do contrato, o respetivo valor será descontado no último recibo de vencimento, dando desde já expressa autorização para esse efeito.";

            // 3. Configurar as Grelhas
            ConfigurarGrid(dgvReceber);
            ConfigurarGrid(dgvDevolver);

            // 4. Preparar a área de desenho
            InicializarAreaDesenho();
        }

        private void ConfigurarGrid(DataGridView dgv)
        {
            dgv.Columns.Clear();
            dgv.Columns.Add("Artigo", "Artigo / Modelo");
            dgv.Columns.Add("Tamanho", "Tam.");
            dgv.Columns.Add("Qtd", "Qtd.");

            // Ajuste visual das colunas
            dgv.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; // Nome ocupa o espaço todo
            dgv.Columns[1].Width = 70; // Tamanho fixo
            dgv.Columns[2].Width = 50; // Quantidade fixa
        }

        // --- MÉTODOS PÚBLICOS PARA ADICIONAR DADOS ---

        public void AdicionarItemReceber(string artigo, string tamanho, int qtd)
        {
            dgvReceber.Rows.Add(artigo, tamanho, qtd);
        }

        public void AdicionarItemDevolver(string artigo, string tamanho, int qtd)
        {
            dgvDevolver.Rows.Add(artigo, tamanho, qtd);
        }

        // --- LÓGICA DE DESENHO ---

        private void InicializarAreaDesenho()
        {
            // Cria um bitmap do tamanho da PictureBox
            signatureBitmap = new Bitmap(picSignature.Width, picSignature.Height);
            picSignature.Image = signatureBitmap;

            // Pinta o fundo de branco (importante para o PDF não ficar com fundo preto/transparente)
            using (Graphics g = Graphics.FromImage(signatureBitmap))
            {
                g.Clear(Color.White);
            }
        }

        private void picSignature_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDrawing = true;
                lastPoint = e.Location;
            }
        }

        private void picSignature_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing && picSignature.Image != null)
            {
                using (Graphics g = Graphics.FromImage(picSignature.Image))
                {
                    // AntiAlias para a linha ficar bonita e não "pixelizada"
                    g.SmoothingMode = SmoothingMode.AntiAlias;

                    using (Pen pen = new Pen(Color.Black, 2)) // Caneta preta, espessura 2
                    {
                        pen.StartCap = LineCap.Round;
                        pen.EndCap = LineCap.Round;
                        g.DrawLine(pen, lastPoint, e.Location);
                    }
                }
                picSignature.Invalidate(); // Força a atualização visual imediata
                lastPoint = e.Location;
            }
        }

        private void picSignature_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDrawing = false;
            }
        }

        // --- BOTÕES ---

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            InicializarAreaDesenho(); // Reseta a imagem para branco
            picSignature.Invalidate();
        }

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            // Validação Obrigatória
            if (!chkAceito.Checked)
            {
                MessageBox.Show("É obrigatório aceitar os termos e condições para prosseguir.",
                                "Assinatura Pendente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Guarda a assinatura na propriedade pública
            AssinaturaFinal = new Bitmap(picSignature.Image);

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        // Evento Load (opcional, caso queiras ajustar algo ao abrir)
        private void FormAssinatura_Load(object sender, EventArgs e)
        {
            // Se não houver itens para devolver, podes esconder a grid para ficar mais limpo
            if (dgvDevolver.Rows.Count == 0)
            {
                // Opcional: lblDevolver.Visible = false; dgvDevolver.Visible = false;
            }
        }
    }
}