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
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int IDEPI { get; set; }
        public string DescricaoArtigo => lblModelo.Text;
        public string TamanhoSelecionado => lblTamanho.Text;
        public int QuantidadeDevolvida => Convert.ToInt32(lblQuantDevolvida.Text);

        // Este é o tal "construtor de 4 argumentos" que o Visual Studio está a pedir!
        public LinhaDevolucao(int idEpi, string modelo, string tamanho, int quantDevolvida)
        {
            InitializeComponent(); // OBRIGATÓRIO: É isto que desenha o ecrã

            this.IDEPI = idEpi;
            lblModelo.Text = modelo;
            lblTamanho.Text = tamanho;
            lblQuantDevolvida.Text = quantDevolvida.ToString();
        }
    }
}
