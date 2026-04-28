using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace PEPIDI.UCs.DGVS
{
    public partial class LinhaDevolucao : UserControl
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int IDEPI { get; set; }

        public string DescricaoArtigo => lblModelo.Text;
        public string TamanhoSelecionado => lblTamanho.Text;
        public int QuantidadeDevolvida => Convert.ToInt32(lblQuantDevolvida.Text);

        // A propriedade que o Form vai ler
        public string EstadoSelecionado
        {
            get { return cmbEstado != null ? cmbEstado.Text : "Usado"; }
        }

        // O Evento!
        public event EventHandler EstadoAlterado;

        public LinhaDevolucao(int idEpi, string modelo, string tamanho, string estado, int quantDevolvida)
        {
            InitializeComponent();

            this.IDEPI = idEpi;
            lblModelo.Text = modelo;
            lblTamanho.Text = tamanho;
            lblQuantDevolvida.Text = quantDevolvida.ToString();

            // Configurar a ComboBox logo a nascença
            if (cmbEstado != null)
            {
                cmbEstado.Items.Clear();
                cmbEstado.Items.Add("Novo");
                cmbEstado.Items.Add("Usado");
                cmbEstado.Items.Add("Gasto");

                // O estado por defeito será o que passares na SP (Usado)
                cmbEstado.Text = string.IsNullOrEmpty(estado) ? "Usado" : estado;

                // Atrelar o evento de mudança (Atenção ao nome da tua combobox no design!)
                cmbEstado.SelectedIndexChanged += CmbEstado_SelectedIndexChanged;
            }
        }

        private void CmbEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Avisa o Form principal!
            EstadoAlterado?.Invoke(this, EventArgs.Empty);
        }
    }
}