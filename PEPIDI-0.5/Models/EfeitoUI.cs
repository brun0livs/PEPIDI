public class EfeitoUI
{
    // Método principal que devolve texto (InputBox)
    public DialogResult AbrirMensagem(string mensagem, string titulo, bool txt, out string valorInserido)
    {
        Form pai = Form.ActiveForm;
        DialogResult resultado = DialogResult.None;
        valorInserido = string.Empty; // Inicialização obrigatória para parâmetros 'out'

        using (Form overlay = new Form())
        {
            overlay.StartPosition = FormStartPosition.Manual;
            overlay.FormBorderStyle = FormBorderStyle.None;
            overlay.Opacity = 0.50d;
            overlay.BackColor = Color.Black;
            overlay.ShowInTaskbar = false;

            if (pai != null)
            {
                overlay.Location = pai.Location;
                overlay.Size = pai.Size;
                overlay.Show(pai);
            }
            else
            {
                overlay.WindowState = FormWindowState.Maximized;
                overlay.Show();
            }

            using (PEPIDI.FormsSecundarios.MSGBX frm = new PEPIDI.FormsSecundarios.MSGBX(mensagem, titulo, txt))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.TopMost = true;

                resultado = frm.ShowDialog(overlay);

                // O form fechou. Se foi um sucesso e era modo Input, extraímos o valor
                if (txt && resultado == DialogResult.OK)
                {
                    valorInserido = frm.ValorInserido;
                }
            }
        }
        return resultado;
    }

    // Overload para mensagens simples (Message Box normal)
    public DialogResult AbrirMensagem(string mensagem, string titulo)
    {
        // Chama o método principal, mas ignora o valor de saída ('out _')
        return AbrirMensagem(mensagem, titulo, false, out _);
    }
}