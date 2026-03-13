using PEPIDI.Utils;

namespace PEPIDI.FormsSecundarios
{
    public partial class MSGBX : Form
    {
        public MSGBX(string _Message, string _Titulo)
        {
            InitializeComponent();
            GestorTema.AplicarEstilos(this);
            lblMessage.Text = _Message;
            lblTitulo.Text = "PEPIDI | " + _Titulo;

            // Configuração padrão: Apenas OK visível
            this.AcceptButton = btnOK;
            this.CancelButton = btnOK;
        }

        private void MSGBX_Load(object sender, EventArgs e)
        {
            Gere(lblMessage);
        }

        private void Gere(Label lbl)
        {
            // Verifica se a mensagem contém "Deseja" ou "?" para agir como pergunta
            if (lbl.Text.Contains("Deseja") || lbl.Text.Contains("?"))
            {
                // Configura o botão principal como SIM
                btnOK.Text = "Sim";
                this.DialogResult = DialogResult.None; // Reset para não fechar logo

                // Ativa o botão de Cancelar (tens de ter um btnCancelar no Designer)
                if (btnCancelar != null)
                {
                    btnCancelar.Visible = true;
                    btnCancelar.Enabled = true;
                    btnCancelar.Text = "Cancelar";
                    this.CancelButton = btnCancelar; // Agora o ESC faz "Não"
                    this.AcceptButton = btnOK;       // O ENTER faz "Sim"
                }
            }
            else
            {
                // Modo Aviso normal
                btnOK.Text = "OK";
                if (btnCancelar != null) btnCancelar.Visible = false;
                this.CancelButton = btnOK;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes; // Se clicou em "Sim/OK"
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No; // Se clicou em "Não"
            this.Close();
        }
    }
}