using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PEPIDI.UCs.DGVS
{
    public partial class LinhaDevolucao : UserControl
    {
        // ISTO É O QUE FALTA AÍ PARA OS ERROS DESAPARECEREM:
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        // ISTO É O QUE FALTA AÍ PARA OS ERROS DESAPARECEREM:
        public int IDEPI { get; set; }
        public string DescricaoArtigo => lblModelo.Text;
        public string TamanhoSelecionado => lblTamanho.Text;
        public int QuantidadeDevolvida => Convert.ToInt32(lblQuantDevolvida.Text);

        public LinhaDevolucao(int idEpi, string modelo, string tamanho, string estado, int quantDevolvida)
        {
            InitializeComponent();

            this.IDEPI = idEpi;
            lblModelo.Text = modelo;
            lblTamanho.Text = tamanho;
            cmbEstado.Text = estado;
            lblQuantDevolvida.Text = quantDevolvida.ToString();
        }
    }
}
