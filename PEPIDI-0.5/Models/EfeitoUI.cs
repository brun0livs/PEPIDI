public class EfeitoUI // Removido o 'static'
{
    // Método para abrir a mensagem a partir de qualquer formulário
    public DialogResult AbrirMensagem(string mensagem, string titulo)
    {
        // Obtém automaticamente o formulário que está ativo no momento
        Form pai = Form.ActiveForm;
        DialogResult resultado = DialogResult.None;

        using (Form overlay = new Form())
        {
            // Configuração da sombra
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

            using (PEPIDI.FormsSecundarios.MSGBX frm = new PEPIDI.FormsSecundarios.MSGBX(mensagem, titulo))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.TopMost = true;

                // O ShowDialog(overlay) bloqueia o fundo e foca a MSGBX automaticamente
                resultado = frm.ShowDialog(overlay);
            }
        }
        return resultado;
    }
}