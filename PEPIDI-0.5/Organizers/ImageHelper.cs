using System.Drawing;
using System.Drawing.Imaging;

public static class ImageHelper
{
    public static Image InverterCores(Image imagemOriginal)
    {
        if (imagemOriginal == null) return null;

        // Cria uma nova imagem com o mesmo tamanho
        Bitmap novaImagem = new Bitmap(imagemOriginal.Width, imagemOriginal.Height);

        using (Graphics g = Graphics.FromImage(novaImagem))
        {
            // Matriz de cor para inverter:
            // O segredo está em multiplicar as cores RGB por -1 e adicionar 1
            // (Ex: Branco 1 vira -1, soma 1 = 0 Preto)
            ColorMatrix colorMatrix = new ColorMatrix(new float[][]
            {
                new float[] {-1, 0, 0, 0, 0}, // Red
                new float[] { 0,-1, 0, 0, 0}, // Green
                new float[] { 0, 0,-1, 0, 0}, // Blue
                new float[] { 0, 0, 0, 1, 0}, // Alpha (Mantém transparência)
                new float[] { 1, 1, 1, 0, 1}  // Offset (Adiciona 1 ao RGB)
            });

            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(colorMatrix);

            // Desenha a imagem original na nova imagem aplicando o filtro
            g.DrawImage(imagemOriginal,
                new Rectangle(0, 0, imagemOriginal.Width, imagemOriginal.Height),
                0, 0, imagemOriginal.Width, imagemOriginal.Height,
                GraphicsUnit.Pixel,
                attributes);
        }

        return novaImagem;
    }
}