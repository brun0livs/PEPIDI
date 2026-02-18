using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.IO.Image;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using iText.Layout.Properties;
using iText.Layout.Borders;
using iText.Kernel.Colors;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace PEPIDI.Organizers
{
    public class PDFGenerator
    {
        public static string GerarComprovativo(int idPedido, string nomeFunc, string nmec, string funcao,
                                               List<(int ID ,string Artigo, string Tamanho, int Qtd)> itensReceber,
                                               List<(string Artigo, string Tamanho, int Qtd)> itensDevolver,
                                               Bitmap assinaturaBitmap)
        {
            // 1. Configurar Caminhos
            string desktopPath = @"C:\PEPIDI_Docs\"; // Altera para o teu caminho de rede ou local
            if (!Directory.Exists(desktopPath)) Directory.CreateDirectory(desktopPath);

            string nomeFicheiro = $"Entrega_{idPedido}_{nmec}_{System.DateTime.Now:yyyyMMddHHmm}.pdf";
            string caminhoPDF = Path.Combine(desktopPath, nomeFicheiro);
            string caminhoLogo = "logo.png"; // Certifica-te que o logo está na pasta do executável ou mete o caminho completo

            // 2. Criar PDF
            using (PdfWriter writer = new PdfWriter(caminhoPDF))
            using (PdfDocument pdf = new PdfDocument(writer))
            using (Document doc = new Document(pdf))
            {
                doc.SetMargins(20, 20, 20, 20);
                PdfFont fontBold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                PdfFont fontNormal = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                doc.SetFont(fontNormal).SetFontSize(9);

                // --- CABEÇALHO ---
                Table header = new Table(UnitValue.CreatePercentArray(new float[] { 1, 1 })).UseAllAvailableWidth();

                // Logo
                if (File.Exists(caminhoLogo))
                {
                    iText.Layout.Element.Image img = new iText.Layout.Element.Image(ImageDataFactory.Create(caminhoLogo)).ScaleToFit(100, 50);
                    header.AddCell(new Cell().Add(img).SetBorder(Border.NO_BORDER));
                }
                else
                {
                    header.AddCell(new Cell().Add(new Paragraph("DIATOSTA")).SetBorder(Border.NO_BORDER));
                }

                // Dados do Documento
                Paragraph pDados = new Paragraph()
                    .Add(new Text("COMPROVATIVO DE ENTREGA\n").SetFont(fontBold))
                    .Add($"Ref: PEPIDI-{System.DateTime.Now.Year}/{idPedido:D5}\n")
                    .Add($"Data: {System.DateTime.Now:dd/MM/yyyy HH:mm}")
                    .SetTextAlignment(TextAlignment.RIGHT);

                header.AddCell(new Cell().Add(pDados).SetBorder(Border.NO_BORDER));
                doc.Add(header);
                doc.Add(new Paragraph("\n"));

                // --- DADOS DO FUNCIONÁRIO ---
                doc.Add(new Paragraph($"Funcionário: {nomeFunc} (Nº {nmec})").SetFont(fontBold));
                doc.Add(new Paragraph($"Função: {funcao}"));
                doc.Add(new Paragraph("\n"));

                // --- TABELA DE ITENS A RECEBER ---
                if (itensReceber.Count > 0)
                {
                    doc.Add(new Paragraph("ARTIGOS ENTREGUES (A RECEBER)").SetFont(fontBold).SetFontColor(ColorConstants.GREEN));
                    Table tReceber = CriarTabelaItens();
                    foreach (var item in itensReceber)
                    {
                        AdicionarLinhaTabela(tReceber, item.Artigo, item.Tamanho, item.Qtd);
                    }
                    doc.Add(tReceber);
                    doc.Add(new Paragraph("\n"));
                }

                // --- TABELA DE ITENS A DEVOLVER ---
                if (itensDevolver.Count > 0)
                {
                    doc.Add(new Paragraph("ARTIGOS DEVOLVIDOS (RETOMA)").SetFont(fontBold).SetFontColor(ColorConstants.RED));
                    Table tDevolver = CriarTabelaItens();
                    foreach (var item in itensDevolver)
                    {
                        AdicionarLinhaTabela(tDevolver, item.Artigo, item.Tamanho, item.Qtd);
                    }
                    doc.Add(tDevolver);
                    doc.Add(new Paragraph("\n"));
                }

                // --- TEXTO LEGAL ---
                doc.Add(new Paragraph("DECLARAÇÃO:").SetFont(fontBold));
                Paragraph legal = new Paragraph(
                    "Declaro que recebi os Equipamentos de Protecção Individual acima mencionados..." +
                    "(texto completo da diatosta aqui)... dando desde já expressa autorização para esse efeito.")
                    .SetFontSize(7).SetTextAlignment(TextAlignment.JUSTIFIED);
                doc.Add(legal);
                doc.Add(new Paragraph("\n"));

                // --- ASSINATURA ---
                doc.Add(new Paragraph("Assinatura do Colaborador:").SetFont(fontBold));

                if (assinaturaBitmap != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        assinaturaBitmap.Save(ms, ImageFormat.Png);
                        iText.Layout.Element.Image imgAssinatura = new iText.Layout.Element.Image(ImageDataFactory.Create(ms.ToArray()));
                        imgAssinatura.ScaleToFit(150, 60);
                        doc.Add(imgAssinatura);
                    }
                }

                doc.Add(new Paragraph("\nProcessado por computador - PEPIDI").SetFontSize(6).SetFontColor(ColorConstants.GRAY));
            }

            return caminhoPDF;
        }

        private static Table CriarTabelaItens()
        {
            Table table = new Table(UnitValue.CreatePercentArray(new float[] { 60, 20, 20 })).UseAllAvailableWidth();
            table.AddHeaderCell(new Cell().Add(new Paragraph("Artigo").SimulateBold()).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Tam.").SimulateBold()).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Qtd.").SimulateBold()).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
            return table;
        }

        private static void AdicionarLinhaTabela(Table table, string artigo, string tam, int qtd)
        {
            table.AddCell(new Paragraph(artigo));
            table.AddCell(new Paragraph(tam).SetTextAlignment(TextAlignment.CENTER));
            table.AddCell(new Paragraph(qtd.ToString()).SetTextAlignment(TextAlignment.CENTER));
        }
    }
}