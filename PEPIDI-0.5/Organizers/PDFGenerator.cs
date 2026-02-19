using System;
using System.Collections.Generic;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Layout.Borders;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.IO.Font.Constants;
using iText.IO.Image;
using System.Drawing.Imaging; // Necessário para ImageFormat.Png

namespace PEPIDI.Organizers
{
    public static class PDFGenerator
    {
        // O TEU CÓDIGO ADAPTADO PARA A CLASSE
        public static string GerarComprovativo(
            int IDPEDIDO,
            string FUNCIONARIO,
            string NRFUNC,
            string FUNCAO,
            string NomeGestor,
            List<(int ID, string Artigo, string Tamanho, int Qtd)> listaReceber,
            List<(int ID, string Artigo, string Tamanho, int Qtd)> listaDevolver,
            System.Drawing.Image AssinaturaFinal)
        {
            string pasta =  @"U:\Bruno\ComprovativosPEPIDI\";

            // Garantir que a pasta existe
            if (!Directory.Exists(pasta))
            {
                Directory.CreateDirectory(pasta);
            }

            string caminhoPDF = System.IO.Path.Combine(pasta, "ComprovativoNr" + IDPEDIDO + ".pdf");
            string caminhoImagem = System.IO.Path.Combine(pasta, "imagem1.png"); // Caminho do logo

            using (PdfWriter writer = new PdfWriter(caminhoPDF))
            using (PdfDocument pdf = new PdfDocument(writer))
            {
                Document doc = new Document(pdf);

                // Definir margens
                doc.SetMargins(1f * 28.35f, 1.29f * 28.35f, 1.27f * 28.35f, 1.35f * 28.35f);

                // Definir fonte padrão menor
                PdfFont fontNegrito = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                PdfFont fontNormal = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                doc.SetFont(fontNormal).SetFontSize(8);

                // Criar a tabela do cabeçalho com 2 colunas (Logo e Dados da Nota de Encomenda)
                Table tabelaCabecalho = new Table(UnitValue.CreatePercentArray(new float[] { 50f, 50f })).UseAllAvailableWidth();

                // Criar célula para a imagem (LOGO)
                Cell cellImagem = new Cell().SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT).SetPadding(0);

                // --- ADAPTAÇÃO: Buscar a imagem diretamente aos Resources ---
                // ATENÇÃO: Substitui "NomeDoTeuLogo" pelo nome exato da imagem que está nos Resources!
                System.Drawing.Image logoProjeto = Properties.Resources.logo;

                if (logoProjeto != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        // Converte a imagem dos recursos para PNG em memória
                        logoProjeto.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        byte[] logoBytes = ms.ToArray();

                        // Cria a imagem para o iText7 a partir dos bytes
                        iText.Layout.Element.Image img = new iText.Layout.Element.Image(ImageDataFactory.Create(logoBytes));
                        img.ScaleToFit(100, 100);
                        cellImagem.Add(img);
                    }
                }
                // Criar tabela interna para a Nota de Encomenda (à direita)
                Table tabelaNota = new Table(UnitValue.CreatePercentArray(new float[] { 60f, 40f })).UseAllAvailableWidth();

                tabelaNota.AddCell(new Cell().Add(new Paragraph("COMPROVATIVO DE ENTREGA")
                                                .SetMultipliedLeading(0.001f)
                                                .SetFont(fontNegrito)
                                                .SetFontSize(9))
                                                .SetTextAlignment(TextAlignment.LEFT)
                                                .SetBorder(Border.NO_BORDER));

                string numeroFormatado = IDPEDIDO.ToString("D5");
                string yy = DateTime.Now.ToString("yy");
                tabelaNota.AddCell(new Cell().Add(new Paragraph("RHCEEPI-" + yy + "/" + numeroFormatado)
                                                .SetMultipliedLeading(0.001f)
                                                .SetFont(fontNegrito)
                                                .SetFontSize(9))
                                                .SetTextAlignment(TextAlignment.RIGHT)
                                                .SetBorder(Border.NO_BORDER));

                // Linha separadora
                tabelaNota.AddCell(new Cell(1, 2).SetBorderBottom(new SolidBorder(ColorConstants.LIGHT_GRAY, 1))
                                                .SetBorderTop(Border.NO_BORDER)
                                                .SetBorderLeft(Border.NO_BORDER)
                                                .SetBorderRight(Border.NO_BORDER));

                // Adicionar Data e Página
                tabelaNota.AddCell(new Cell().Add(new Paragraph("Data").SetFontSize(8)).SetBorder(Border.NO_BORDER));
                tabelaNota.AddCell(new Cell().Add(new Paragraph(DateTime.Now.ToString("dd/MM/yyyy")).SetFontSize(8))
                                            .SetTextAlignment(TextAlignment.RIGHT)
                                            .SetBorder(Border.NO_BORDER));

                // Adicionar elementos ao cabeçalho
                tabelaCabecalho.AddCell(cellImagem);
                tabelaCabecalho.AddCell(new Cell().Add(tabelaNota).SetBorder(Border.NO_BORDER));

                // Criar tabela de informações da empresa e destinatário
                Table tabelaInfo = new Table(UnitValue.CreatePercentArray(new float[] { 50f, 50f })).UseAllAvailableWidth();

                // Informações da empresa
                Cell cellEmpresa = new Cell().SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT);
                cellEmpresa.Add(new Paragraph("Diatosta - Indústria Alimentar, S.A.").SetFont(fontNegrito).SetFontSize(8).SetMultipliedLeading(1.1f));
                cellEmpresa.Add(new Paragraph("Rua Professora Justa Ferreira Dias, Nº155\nOliveirinha\n3810-867 Aveiro").SetFontSize(8).SetMultipliedLeading(1.1f));
                cellEmpresa.Add(new Paragraph("Tel: 234940100  Fax: 234940101").SetFontSize(8).SetMultipliedLeading(1.1f));
                cellEmpresa.Add(new Paragraph("Website: www.diatosta.pt").SetFontSize(8).SetMultipliedLeading(1.1f));
                cellEmpresa.Add(new Paragraph("Matrícula C.R.C. Aveiro e NIPC: PT500696985").SetFontSize(8).SetMultipliedLeading(1.1f));
                cellEmpresa.Add(new Paragraph("Capital Social: 1 800 000,00 EUR").SetFontSize(8).SetMultipliedLeading(1.1f));

                // Informações do destinatário
                Cell cellDestinatario = new Cell().SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT);
                cellDestinatario.Add(new Paragraph(FUNCIONARIO).SetFont(fontNegrito).SetFontSize(10).SetMultipliedLeading(1.1f));
                cellDestinatario.Add(new Paragraph("NMEC: " + NRFUNC + "\nFunção: " + FUNCAO + "\n").SetFontSize(10).SetMultipliedLeading(1.1f));

                // Adicionar células à tabela de informações
                tabelaInfo.AddCell(cellEmpresa);
                tabelaInfo.AddCell(cellDestinatario);

                // Criar a tabela com 4 colunas
                float[] largurasColunas = { 20f, 30, 30, 20f };
                Table tabelaResp = new Table(UnitValue.CreatePercentArray(largurasColunas)).UseAllAvailableWidth().SetBorder(Border.NO_BORDER);

                Cell headerHora = new Cell().Add(new Paragraph("Hora").SetFont(fontNegrito).SetTextAlignment(TextAlignment.CENTER)).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBorder(Border.NO_BORDER);
                Cell headerVazio1 = new Cell().Add(new Paragraph("").SetFont(fontNegrito).SetTextAlignment(TextAlignment.CENTER)).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBorder(Border.NO_BORDER);
                Cell headerVazio2 = new Cell().Add(new Paragraph("").SetFont(fontNegrito).SetTextAlignment(TextAlignment.CENTER)).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBorder(Border.NO_BORDER);
                Cell headerResp = new Cell().Add(new Paragraph("Entrega").SetFont(fontNegrito).SetTextAlignment(TextAlignment.CENTER)).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBorder(Border.NO_BORDER);

                tabelaResp.AddHeaderCell(headerHora);
                tabelaResp.AddHeaderCell(headerVazio1);
                tabelaResp.AddHeaderCell(headerVazio2);
                tabelaResp.AddHeaderCell(headerResp);

                tabelaResp.AddCell(new Cell().Add(new Paragraph(DateTime.Now.ToString("HH:mm"))).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.CENTER));
                tabelaResp.AddCell(new Cell().Add(new Paragraph("")).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));
                tabelaResp.AddCell(new Cell().Add(new Paragraph("")).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.CENTER));

                // Opcional: Se quiseres mostrar quem processou o documento na tabela
                tabelaResp.AddCell(new Cell().Add(new Paragraph("Processado")).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.CENTER));

                int total = 0;
                int total2 = 0;
                int linhasFinais = 22;

                Table tabelaItens = new Table(UnitValue.CreatePercentArray(largurasColunas)).UseAllAvailableWidth().SetBorder(Border.NO_BORDER);
                Table tabelaItens2 = new Table(UnitValue.CreatePercentArray(largurasColunas)).UseAllAvailableWidth().SetBorder(Border.NO_BORDER);
                Table tabelaItens3 = new Table(UnitValue.CreatePercentArray(largurasColunas)).UseAllAvailableWidth().SetBorder(Border.NO_BORDER);

                Cell headerArtigo = new Cell().Add(new Paragraph("Artigo").SetFont(fontNegrito).SetTextAlignment(TextAlignment.CENTER)).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBorder(Border.NO_BORDER);
                Cell headerDescricao = new Cell().Add(new Paragraph("Descrição").SetFont(fontNegrito).SetTextAlignment(TextAlignment.CENTER)).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBorder(Border.NO_BORDER);
                Cell headerTamanho = new Cell().Add(new Paragraph("Tamanho").SetFont(fontNegrito).SetTextAlignment(TextAlignment.CENTER)).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBorder(Border.NO_BORDER);
                Cell headerQuantidade = new Cell().Add(new Paragraph("Quantidade Entregue").SetFont(fontNegrito).SetTextAlignment(TextAlignment.CENTER)).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBorder(Border.NO_BORDER);
                Cell headerQuantidade2 = new Cell().Add(new Paragraph("Quantidade Devolvida").SetFont(fontNegrito).SetTextAlignment(TextAlignment.CENTER)).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBorder(Border.NO_BORDER);

                tabelaItens.AddHeaderCell(headerArtigo);
                tabelaItens.AddHeaderCell(headerDescricao);
                tabelaItens.AddHeaderCell(headerTamanho);
                tabelaItens.AddHeaderCell(headerQuantidade);

                tabelaItens2.AddHeaderCell(headerArtigo);
                tabelaItens2.AddHeaderCell(headerDescricao);
                tabelaItens2.AddHeaderCell(headerTamanho);
                tabelaItens2.AddHeaderCell(headerQuantidade2);

                tabelaItens.SetFixedLayout();
                tabelaItens2.SetFixedLayout();

                int usadas = 0;
                if (listaDevolver.Count > 0)
                {
                    usadas = 3;
                }

                // CÓDIGO ADAPTADO: Substituir o for(dgv) por foreach(listaReceber)
                foreach (var item in listaReceber)
                {
                    linhasFinais--;
                    total += item.Qtd;

                    tabelaItens.AddCell(new Cell().Add(new Paragraph("EPI" + item.ID.ToString("D4"))).SetFontSize(7).SetMinHeight(7).SetPadding(1).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE));
                    tabelaItens.AddCell(new Cell().Add(new Paragraph(item.Artigo)).SetMinHeight(13).SetPadding(1).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));
                    tabelaItens.AddCell(new Cell().Add(new Paragraph(item.Tamanho)).SetMinHeight(13).SetPadding(1).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.CENTER));
                    tabelaItens.AddCell(new Cell().Add(new Paragraph(item.Qtd.ToString())).SetMinHeight(13).SetPadding(1).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.RIGHT));
                }

                Table Tlinha = new Table(UnitValue.CreatePercentArray(new float[] { 50f, 50f })).UseAllAvailableWidth();

                Tlinha.AddCell(new Cell().Add(new Paragraph("Total: ")
                                                .SetMultipliedLeading(0.001f)
                                                .SetFont(fontNegrito)
                                                .SetFontSize(9))
                                                .SetTextAlignment(TextAlignment.LEFT)
                                                .SetBorder(Border.NO_BORDER));

                Tlinha.AddCell(new Cell().Add(new Paragraph(total.ToString())
                                                .SetMultipliedLeading(0.001f)
                                                .SetFont(fontNegrito)
                                                .SetFontSize(9))
                                                .SetTextAlignment(TextAlignment.RIGHT)
                                                .SetBorder(Border.NO_BORDER));

                Tlinha.AddCell(new Cell(1, 2).SetBorderBottom(new SolidBorder(ColorConstants.LIGHT_GRAY, 1))
                                                .SetBorderTop(Border.NO_BORDER)
                                                .SetBorderLeft(Border.NO_BORDER)
                                                .SetBorderRight(Border.NO_BORDER));

                // CÓDIGO ADAPTADO: Substituir o for(dgv2) por foreach(listaDevolver)
                foreach (var item in listaDevolver)
                {
                    linhasFinais--;
                    total2 += item.Qtd;

                    tabelaItens2.AddCell(new Cell().Add(new Paragraph("EPIU" + item.ID.ToString("D4"))).SetFontSize(7).SetMinHeight(7).SetPadding(1).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE));
                    tabelaItens2.AddCell(new Cell().Add(new Paragraph(item.Artigo)).SetMinHeight(13).SetPadding(1).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));
                    tabelaItens2.AddCell(new Cell().Add(new Paragraph(item.Tamanho)).SetMinHeight(13).SetPadding(1).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.CENTER));
                    tabelaItens2.AddCell(new Cell().Add(new Paragraph(item.Qtd.ToString())).SetMinHeight(13).SetPadding(1).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.RIGHT));
                }

                Table Tlinha2 = new Table(UnitValue.CreatePercentArray(new float[] { 50f, 50f })).UseAllAvailableWidth();

                Tlinha2.AddCell(new Cell().Add(new Paragraph("Total: ")
                                                .SetMultipliedLeading(0.001f)
                                                .SetFont(fontNegrito)
                                                .SetFontSize(9))
                                                .SetTextAlignment(TextAlignment.LEFT)
                                                .SetBorder(Border.NO_BORDER));

                Tlinha2.AddCell(new Cell().Add(new Paragraph(total2.ToString())
                                                .SetMultipliedLeading(0.001f)
                                                .SetFont(fontNegrito)
                                                .SetFontSize(9))
                                                .SetTextAlignment(TextAlignment.RIGHT)
                                                .SetBorder(Border.NO_BORDER));

                Tlinha2.AddCell(new Cell(1, 2).SetBorderBottom(new SolidBorder(ColorConstants.LIGHT_GRAY, 1))
                                                .SetBorderTop(Border.NO_BORDER)
                                                .SetBorderLeft(Border.NO_BORDER)
                                                .SetBorderRight(Border.NO_BORDER));

                // Preencher células vazias se faltarem linhas
                for (int i = 0; i < linhasFinais - usadas; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        tabelaItens3.AddCell(new Cell().Add(new Paragraph("")).SetMinHeight(10).SetPadding(2).SetBorder(Border.NO_BORDER));
                    }
                }

                Table tabelaAssinatura = new Table(UnitValue.CreatePercentArray(new float[] { 60f, 40f }))
                                        .UseAllAvailableWidth()
                                        .SetMarginTop(10);

                tabelaAssinatura.AddCell(new Cell().Add(new Paragraph("Assinatura:")
                                        .SetFont(fontNegrito)
                                        .SetFontSize(9))
                                        .SetBorder(Border.NO_BORDER)
                                        .SetBackgroundColor(ColorConstants.LIGHT_GRAY));

                tabelaAssinatura.AddCell(new Cell().SetBorder(Border.NO_BORDER));

                // CÓDIGO ADAPTADO: Usar a AssinaturaFinal passada por parâmetro
                if (AssinaturaFinal != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        AssinaturaFinal.Save(ms, ImageFormat.Png);
                        byte[] assinaturaBytes = ms.ToArray();

                        iText.Layout.Element.Image assinaturaImg = new iText.Layout.Element.Image(ImageDataFactory.Create(assinaturaBytes))
                                                .ScaleToFit(150, 50)
                                                .SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);
                        try
                        {
                            tabelaAssinatura.AddCell(new Cell(3, 1)
                                .Add(assinaturaImg)
                                .SetBorder(Border.NO_BORDER)
                                .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                                .SetMaxHeight(50)
                                .SetMinHeight(50));
                        }
                        catch
                        {
                            // Erro silencioso no PDF
                        }
                    }
                }
                else
                {
                    tabelaAssinatura.AddCell(new Cell(3, 1).Add(new Paragraph("")).SetBorder(Border.NO_BORDER).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetMinHeight(50));
                }

                tabelaAssinatura.AddCell(new Cell().SetBorder(Border.NO_BORDER)); // célula de espaço

                // Adicionar tudo ao documento
                doc.Add(tabelaCabecalho);
                doc.Add(tabelaInfo);
                doc.Add(new Paragraph("\n"));
                doc.Add(tabelaResp);
                doc.Add(new Paragraph("\n"));

                if (listaDevolver.Count > 0)
                {
                    doc.Add(tabelaItens);
                    doc.Add(Tlinha);
                    doc.Add(new Paragraph("\n"));
                    doc.Add(tabelaItens2);
                    doc.Add(Tlinha2);
                    doc.Add(tabelaItens3);
                }
                else
                {
                    doc.Add(tabelaItens);
                    doc.Add(Tlinha);
                    doc.Add(tabelaItens3);
                }

                doc.Add(new Paragraph(
                    "Eu abaixo assinado, declaro que recebi os Equipamentos de Proteção Individual acima mencionados e " +
                    "informação sobre os riscos do meu posto de trabalho. Comprometo-me a utilizá-los corretamente de acordo com as instruções recebidas, " +
                    "a conservá-los e mantê-los em bom estado, e a participar todas as anomalias ou defeito de que tenha conhecimento.\nEm caso de perda ou " +
                    "má utilização, cessação contratual, abandono do posto de trabalho e demais situações, ou caso não seja feita a devolução do fardamento e " +
                    "EPIs no final do contrato, o respetivo valor será descontado no último recibo de vencimento, dando desde já expressa autorização para esse" +
                    " efeito.")
                    .SetTextAlignment(TextAlignment.JUSTIFIED));

                doc.Add(tabelaAssinatura);
                doc.Add(new Paragraph("Processado por computador").SetMultipliedLeading(0.8f).SetMargin(0.5f));

                doc.Close();
                return caminhoPDF;
            }
        }

        public static string GerarListaRecolhaArmazem(List<(string Modelo, string Tamanho, int Qtd)> itensParaArmazem)
        {
            string desktopPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "PEPIDI_ListasArmazem");
            if (!Directory.Exists(desktopPath)) Directory.CreateDirectory(desktopPath);

            string caminhoPDF = Path.Combine(desktopPath, $"ListaRecolhaGeral_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");

            using (iText.Kernel.Pdf.PdfWriter writer = new iText.Kernel.Pdf.PdfWriter(caminhoPDF))
            using (iText.Kernel.Pdf.PdfDocument pdf = new iText.Kernel.Pdf.PdfDocument(writer))
            using (iText.Layout.Document doc = new iText.Layout.Document(pdf))
            {
                // Cabeçalho Principal
                doc.Add(new iText.Layout.Element.Paragraph("LISTA DE RECOLHA - ARMAZÉM GERAL")
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetFontSize(16).SimulateBold());

                doc.Add(new iText.Layout.Element.Paragraph($"Data de Impressão: {DateTime.Now:dd/MM/yyyy HH:mm}")
                    .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                    .SetFontSize(10).SetMarginBottom(20));

                // Tabela: Checkbox (10%), Quantidade (20%), Artigo (50%), Tamanho (20%)
                iText.Layout.Element.Table table = new iText.Layout.Element.Table(iText.Layout.Properties.UnitValue.CreatePercentArray(new float[] { 10, 20, 50, 20 })).UseAllAvailableWidth();
                table.SetMarginBottom(10);

                // Cabeçalhos da Tabela
                table.AddHeaderCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("Check")).SimulateBold().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
                table.AddHeaderCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("Qtd Total")).SimulateBold().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));
                table.AddHeaderCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("Artigo / Modelo")).SimulateBold().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY));
                table.AddHeaderCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph("Tamanho")).SimulateBold().SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

                foreach (var item in itensParaArmazem)
                {
                    // 1. Quadrado da Checkbox [  ]
                    var quadrado = new iText.Layout.Element.Paragraph("   ")
                        .SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.BLACK, 1f))
                        .SetMarginTop(2);

                    table.AddCell(new iText.Layout.Element.Cell().Add(quadrado).SetBorder(iText.Layout.Borders.Border.NO_BORDER).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE));

                    // 2. Quantidade em Destaque
                    table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph($"{item.Qtd}").SimulateBold().SetFontSize(12)).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE));

                    // 3. Modelo e 4. Tamanho
                    table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph(item.Modelo)).SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE));
                    table.AddCell(new iText.Layout.Element.Cell().Add(new iText.Layout.Element.Paragraph(item.Tamanho)).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER).SetVerticalAlignment(iText.Layout.Properties.VerticalAlignment.MIDDLE));
                }

                doc.Add(table);
            }

            return caminhoPDF;
        }
    }
}