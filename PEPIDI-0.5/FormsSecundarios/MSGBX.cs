using PEPIDI.Utils;

namespace PEPIDI.FormsSecundarios
{
    public partial class MSGBX : Form
    {
        private bool _isInputBox;
        // Propriedade pública para podermos ler o valor de fora antes do form ser destruído
        public string ValorInserido { get; private set; }

        public MSGBX(string mensagem, string titulo, bool isInput = false)
        {
            InitializeComponent();
            GestorTema.AplicarEstilos(this);

            lblMessage.Text = mensagem;
            lblTitulo.Text = "PEPIDI | " + titulo;
            _isInputBox = isInput;

            // Configuração padrão: Apenas OK visível
            this.AcceptButton = btnOK;
            this.CancelButton = btnOK;
        }

        private void MSGBX_Load(object sender, EventArgs e)
        {
            Gere(lblMessage);
            ConfigurarVisibilidade(_isInputBox);
        }

        private void Gere(Label lbl)
        {
            if (lbl.Text.Contains("Deseja") || lbl.Text.Contains("?"))
            {
                btnOK.Text = "Sim";
                this.DialogResult = DialogResult.None;

                if (btnCancelar != null)
                {
                    btnCancelar.Visible = true;
                    btnCancelar.Enabled = true;
                    btnCancelar.Text = "Cancelar";
                    this.CancelButton = btnCancelar;
                    this.AcceptButton = btnOK;
                }
            }
            else
            {
                btnOK.Text = "OK";
                if (btnCancelar != null) btnCancelar.Visible = false;
                this.CancelButton = btnOK;
            }
        }

        // Renomeei para ficar mais claro o que faz
        private void ConfigurarVisibilidade(bool isInput)
        {
            if (isInput)
            {
                // Modo InputBox: Esconde a Label, mostra a TextBox
                tableLayoutPanel4.ColumnStyles[0].SizeType = SizeType.Percent;
                tableLayoutPanel4.ColumnStyles[0].Width = 50;

                tableLayoutPanel4.ColumnStyles[1].SizeType = SizeType.Percent;
                tableLayoutPanel4.ColumnStyles[1].Width = 50;
                txtEntradaDeDados.Visible = true;
            }
            else
            {
                // Modo MessageBox: Mostra a Label, esconde a TextBox
                tableLayoutPanel4.ColumnStyles[0].SizeType = SizeType.Percent;
                tableLayoutPanel4.ColumnStyles[0].Width = 100;
                lblMessage.Visible = true;

                tableLayoutPanel4.ColumnStyles[1].SizeType = SizeType.Absolute;
                tableLayoutPanel4.ColumnStyles[1].Width = 0;
                txtEntradaDeDados.Visible = false;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (_isInputBox)
            {
                // Guarda o valor inserido na propriedade pública
                ValorInserido = txtEntradaDeDados.Text;
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                this.DialogResult = DialogResult.Yes;
            }
            // Não precisas de chamar this.Close() explicitamente quando defines o DialogResult
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }
    }
}