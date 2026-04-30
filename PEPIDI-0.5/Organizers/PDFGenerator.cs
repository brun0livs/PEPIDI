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
using System.Drawing.Imaging;
using iText.Forms.Xfdf;
using iText.Kernel.Pdf.Annot;
using iText.Forms.Fields;
using System.Windows.Forms.VisualStyles;
using VerticalAlignment = iText.Layout.Properties.VerticalAlignment;
using Microsoft.Data.SqlClient;

namespace PEPIDI.Organizers
{
    public static class PDFGenerator
    {
        public static iText.Layout.Properties.VerticalAlignment? VerticalAlign { get; private set; }

        private static string ObterCaminhoDoSQL(string chave, string pastaFallback)
        {
            try
            {
                using (var conn = GetConn.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqlCommand("SELECT Valor FROM Definicoes WHERE Chave = @Chave", conn))
                    {
                        cmd.Parameters.AddWithValue("@Chave", chave);
                        var resultado = cmd.ExecuteScalar();

                        if (resultado != null && !string.IsNullOrWhiteSpace(resultado.ToString()))
                        {
                            return resultado.ToString(); // Retorna o caminho que está no SQL
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Ignora o erro e avança para a alternativa abaixo
            }

            // Se falhar a BD, cria uma pasta no Ambiente de Trabalho para não perder o PDF!
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            return Path.Combine(desktop, pastaFallback);
        }

        public static string GerarComprovativo(
            int IDPEDIDO,
            string FUNCIONARIO,
            string NRFUNC,
            string FUNCAO,
            string NomeGestor, // NOME DO GESTOR ENTRA AQUI
            List<(string ID, string Artigo, string Tamanho, int Qtd)> listaReceber,
            List<(string ID, string Artigo, string Tamanho, int Qtd)> listaDevolver,
            System.Drawing.Image AssinaturaFinal)
        {
            string caminhoBase = ObterCaminhoDoSQL("CaminhoComprovativos", "ComprovativosPEPIDI_Backup");

            if (!Directory.Exists(caminhoBase)) Directory.CreateDirectory(caminhoBase);

            string caminhoPDF = Path.Combine(caminhoBase, $"ComprovativoNr{IDPEDIDO:D5}.pdf");

            using (PdfWriter writer = new PdfWriter(caminhoPDF))
            using (PdfDocument pdf = new PdfDocument(writer))
            {
                Document doc = new Document(pdf);

                // AS TUAS MARGENS EXATAS
                doc.SetMargins(1f * 28.35f, 1.29f * 28.35f, 1.27f * 28.35f, 1.35f * 28.35f);

                PdfFont fontNegrito = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                PdfFont fontNormal = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                doc.SetFont(fontNormal).SetFontSize(8);

                // =========================================================
                // 1. CABEÇALHO (LOGO E NÚMERO)
                // =========================================================
                Table tabelaCabecalho = new Table(UnitValue.CreatePercentArray(new float[] { 50f, 50f })).UseAllAvailableWidth();

                Cell cellImagem = new Cell().SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT).SetPadding(0);
                System.Drawing.Image logoProjeto = Properties.Resources.logo; // Vai buscar o Logo às Resources
                if (logoProjeto != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        logoProjeto.Save(ms, ImageFormat.Png);
                        byte[] logoBytes = ms.ToArray();
                        iText.Layout.Element.Image img = new iText.Layout.Element.Image(ImageDataFactory.Create(logoBytes));
                        img.ScaleToFit(100, 100);
                        cellImagem.Add(img);
                    }
                }

                Table tabelaNota = new Table(UnitValue.CreatePercentArray(new float[] { 60f, 40f })).UseAllAvailableWidth();
                tabelaNota.AddCell(new Cell().Add(new Paragraph("COMPROVATIVO DE ENTREGA").SetMultipliedLeading(0.001f).SetFont(fontNegrito).SetFontSize(9)).SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER));
                string yy = DateTime.Now.ToString("yy");
                tabelaNota.AddCell(new Cell().Add(new Paragraph("RHCEEPI-" + yy + "/" + IDPEDIDO.ToString("D5")).SetMultipliedLeading(0.001f).SetFont(fontNegrito).SetFontSize(9)).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER));
                tabelaNota.AddCell(new Cell(1, 2).SetBorderBottom(new SolidBorder(ColorConstants.LIGHT_GRAY, 1)).SetBorderTop(Border.NO_BORDER).SetBorderLeft(Border.NO_BORDER).SetBorderRight(Border.NO_BORDER));
                tabelaNota.AddCell(new Cell().Add(new Paragraph("Data").SetFontSize(8)).SetBorder(Border.NO_BORDER));
                tabelaNota.AddCell(new Cell().Add(new Paragraph(DateTime.Now.ToString("dd/MM/yyyy")).SetFontSize(8)).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER));

                tabelaCabecalho.AddCell(cellImagem);
                tabelaCabecalho.AddCell(new Cell().Add(tabelaNota).SetBorder(Border.NO_BORDER));

                // =========================================================
                // 2. INFORMAÇÃO EMPRESA E FUNCIONÁRIO
                // =========================================================
                Table tabelaInfo = new Table(UnitValue.CreatePercentArray(new float[] { 50f, 50f })).UseAllAvailableWidth();

                Cell cellEmpresa = new Cell().SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT);
                cellEmpresa.Add(new Paragraph("Diatosta - Indústria Alimentar, S.A.").SetFont(fontNegrito).SetFontSize(8).SetMultipliedLeading(1.1f));
                cellEmpresa.Add(new Paragraph("Rua Professora Justa Ferreira Dias, Nº155\nOliveirinha\n3810-867 Aveiro").SetFontSize(8).SetMultipliedLeading(1.1f));
                cellEmpresa.Add(new Paragraph("Tel: 234940100  Fax: 234940101").SetFontSize(8).SetMultipliedLeading(1.1f));
                cellEmpresa.Add(new Paragraph("Website: www.diatosta.pt").SetFontSize(8).SetMultipliedLeading(1.1f));
                cellEmpresa.Add(new Paragraph("Matrícula C.R.C. Aveiro e NIPC: PT500696985").SetFontSize(8).SetMultipliedLeading(1.1f));
                cellEmpresa.Add(new Paragraph("Capital Social: 1 800 000,00 EUR").SetFontSize(8).SetMultipliedLeading(1.1f));

                Cell cellDestinatario = new Cell().SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT);
                cellDestinatario.Add(new Paragraph(FUNCIONARIO).SetFont(fontNegrito).SetFontSize(10).SetMultipliedLeading(1.1f));
                cellDestinatario.Add(new Paragraph("NMEC: " + NRFUNC + "\nFunção: " + FUNCAO + "\n").SetFontSize(10).SetMultipliedLeading(1.1f));

                tabelaInfo.AddCell(cellEmpresa);
                tabelaInfo.AddCell(cellDestinatario);

                // =========================================================
                // 3. TABELA DE RESPONSABILIDADE (HORA E GESTOR)
                // =========================================================
                float[] largurasColunas = { 20f, 30, 30, 20f };
                Table tabelaResp = new Table(UnitValue.CreatePercentArray(largurasColunas)).UseAllAvailableWidth().SetBorder(Border.NO_BORDER);

                tabelaResp.AddHeaderCell(new Cell().Add(new Paragraph("Hora").SetFont(fontNegrito).SetTextAlignment(TextAlignment.CENTER)).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBorder(Border.NO_BORDER));
                tabelaResp.AddHeaderCell(new Cell().Add(new Paragraph("").SetFont(fontNegrito).SetTextAlignment(TextAlignment.CENTER)).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBorder(Border.NO_BORDER));
                tabelaResp.AddHeaderCell(new Cell().Add(new Paragraph("").SetFont(fontNegrito).SetTextAlignment(TextAlignment.CENTER)).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBorder(Border.NO_BORDER));
                tabelaResp.AddHeaderCell(new Cell().Add(new Paragraph("Entrega").SetFont(fontNegrito).SetTextAlignment(TextAlignment.CENTER)).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBorder(Border.NO_BORDER));

                tabelaResp.AddCell(new Cell().Add(new Paragraph(DateTime.Now.ToString("HH:mm"))).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.CENTER));
                tabelaResp.AddCell(new Cell().Add(new Paragraph("")).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));
                tabelaResp.AddCell(new Cell().Add(new Paragraph("")).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.CENTER));
                tabelaResp.AddCell(new Cell().Add(new Paragraph(NomeGestor)).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.CENTER)); // AQUI ENTRA O NOME DE QUEM PROCESSOU!

                // =========================================================
                // 4. TABELAS DE ITENS
                // =========================================================
                int total = 0;
                int total2 = 0;
                int linhasFinais = 22; // Garantir que sempre terá 20 linhas no PDF

                Table tabelaItens = new Table(UnitValue.CreatePercentArray(largurasColunas)).UseAllAvailableWidth().SetBorder(Border.NO_BORDER);
                Table tabelaItens2 = new Table(UnitValue.CreatePercentArray(largurasColunas)).UseAllAvailableWidth().SetBorder(Border.NO_BORDER);
                Table tabelaItens3 = new Table(UnitValue.CreatePercentArray(largurasColunas)).UseAllAvailableWidth().SetBorder(Border.NO_BORDER);

                tabelaItens.AddHeaderCell(new Cell().Add(new Paragraph("Artigo").SetFont(fontNegrito).SetTextAlignment(TextAlignment.CENTER)).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBorder(Border.NO_BORDER));
                tabelaItens.AddHeaderCell(new Cell().Add(new Paragraph("Descrição").SetFont(fontNegrito).SetTextAlignment(TextAlignment.CENTER)).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBorder(Border.NO_BORDER));
                tabelaItens.AddHeaderCell(new Cell().Add(new Paragraph("Tamanho").SetFont(fontNegrito).SetTextAlignment(TextAlignment.CENTER)).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBorder(Border.NO_BORDER));
                tabelaItens.AddHeaderCell(new Cell().Add(new Paragraph("Quantidade Entregue").SetFont(fontNegrito).SetTextAlignment(TextAlignment.CENTER)).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBorder(Border.NO_BORDER));

                
                tabelaItens2.AddHeaderCell(new Cell().Add(new Paragraph("Artigo").SetFont(fontNegrito).SetTextAlignment(TextAlignment.CENTER)).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBorder(Border.NO_BORDER));
                tabelaItens2.AddHeaderCell(new Cell().Add(new Paragraph("Descrição").SetFont(fontNegrito).SetTextAlignment(TextAlignment.CENTER)).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBorder(Border.NO_BORDER));
                tabelaItens2.AddHeaderCell(new Cell().Add(new Paragraph("Tamanho").SetFont(fontNegrito).SetTextAlignment(TextAlignment.CENTER)).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBorder(Border.NO_BORDER));
                tabelaItens2.AddHeaderCell(new Cell().Add(new Paragraph("Quantidade Devolvida").SetFont(fontNegrito).SetTextAlignment(TextAlignment.CENTER)).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBorder(Border.NO_BORDER));

                tabelaItens.SetFixedLayout();
                tabelaItens2.SetFixedLayout();

                int usadas = 0;
                if (listaDevolver.Count > 0) usadas = 3;

                // PREENCHER ITENS A RECEBER
                foreach (var item in listaReceber)
                {
                    linhasFinais--;
                    total += item.Qtd;
                    tabelaItens.AddCell(new Cell().Add(new Paragraph("EPI" + item.ID)).SetFontSize(7).SetMinHeight(7).SetPadding(1).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE));
                    tabelaItens.AddCell(new Cell().Add(new Paragraph(item.Artigo)).SetMinHeight(13).SetPadding(1).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));
                    tabelaItens.AddCell(new Cell().Add(new Paragraph(item.Tamanho)).SetMinHeight(13).SetPadding(1).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.CENTER));
                    tabelaItens.AddCell(new Cell().Add(new Paragraph(item.Qtd.ToString())).SetMinHeight(13).SetPadding(1).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.RIGHT));
                }

                Table Tlinha = new Table(UnitValue.CreatePercentArray(new float[] { 20f, 30, 30, 20f })).UseAllAvailableWidth();

                // 1. A célula invisível que faz de linha separadora
                Tlinha.AddCell(new Cell(1, 4) // (1, 4) é mais seguro que (0, 4) no iText7
                    .SetBorderBottom(new SolidBorder(ColorConstants.LIGHT_GRAY, 0.5f))
                    .SetBorderTop(Border.NO_BORDER)
                    .SetBorderLeft(Border.NO_BORDER)
                    .SetBorderRight(Border.NO_BORDER));

                // 2. Célula com a palavra "Total:"
                Tlinha.AddCell(new Cell()
                    .Add(new Paragraph("Total: ").SetFont(fontNegrito).SetFontSize(9))
                    .SetPaddingTop(1f) // <--- O SEGREDO: Dá uma margem de 5 pontos em relação à linha de cima!
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetBorder(Border.NO_BORDER));

                // Células vazias do meio
                Tlinha.AddCell(new Cell().SetBorder(Border.NO_BORDER));
                Tlinha.AddCell(new Cell().SetBorder(Border.NO_BORDER));

                // 3. Célula com o Valor do Total
                Tlinha.AddCell(new Cell()
                    .Add(new Paragraph(total.ToString()).SetFont(fontNegrito).SetFontSize(9))
                    .SetPaddingTop(1f) // <--- Tem de ter o mesmo PaddingTop para ficar alinhado com a palavra "Total"
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetBorder(Border.NO_BORDER));
                // PREENCHER ITENS A DEVOLVER
                foreach (var item in listaDevolver)
                {
                    linhasFinais--;
                    total2 += item.Qtd;
                    tabelaItens2.AddCell(new Cell().Add(new Paragraph("EPI" + item.ID)).SetFontSize(7).SetMinHeight(7).SetPadding(1).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE));
                    tabelaItens2.AddCell(new Cell().Add(new Paragraph(item.Artigo)).SetMinHeight(13).SetPadding(1).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));
                    tabelaItens2.AddCell(new Cell().Add(new Paragraph(item.Tamanho)).SetMinHeight(13).SetPadding(1).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.CENTER));
                    tabelaItens2.AddCell(new Cell().Add(new Paragraph(item.Qtd.ToString())).SetMinHeight(13).SetPadding(1).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.RIGHT));
                }

                Table Tlinha2 = new Table(UnitValue.CreatePercentArray(new float[] { 20f, 30, 30, 20f })).UseAllAvailableWidth();

                // 1. A célula invisível que faz de linha separadora
                Tlinha2.AddCell(new Cell(1, 4) // (1, 4) é mais seguro que (0, 4) no iText7
                    .SetBorderBottom(new SolidBorder(ColorConstants.LIGHT_GRAY, 0.5f))
                    .SetBorderTop(Border.NO_BORDER)
                    .SetBorderLeft(Border.NO_BORDER)
                    .SetBorderRight(Border.NO_BORDER));

                // 2. Célula com a palavra "Total:"
                Tlinha2.AddCell(new Cell()
                    .Add(new Paragraph("Total: ").SetFont(fontNegrito).SetFontSize(9))
                    .SetPaddingTop(1f) // <--- O SEGREDO: Dá uma margem de 5 pontos em relação à linha de cima!
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetBorder(Border.NO_BORDER));

                // Células vazias do meio
                Tlinha2.AddCell(new Cell().SetBorder(Border.NO_BORDER));
                Tlinha2.AddCell(new Cell().SetBorder(Border.NO_BORDER));

                // 3. Célula com o Valor do Total
                Tlinha2.AddCell(new Cell()
                    .Add(new Paragraph(total2.ToString()).SetFont(fontNegrito).SetFontSize(9))
                    .SetPaddingTop(1f) // <--- Tem de ter o mesmo PaddingTop para ficar alinhado com a palavra "Total"
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .SetBorder(Border.NO_BORDER));
                // PREENCHER LINHAS VAZIAS PARA O LAYOUT FIXO
                for (int i = 0; i < linhasFinais - usadas; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        tabelaItens3.AddCell(new Cell().Add(new Paragraph("")).SetMinHeight(10).SetPadding(2).SetBorder(Border.NO_BORDER));
                    }
                }

                // =========================================================
                // 5. CAIXA DE ASSINATURA
                // =========================================================
                Table tabelaAssinatura = new Table(UnitValue.CreatePercentArray(new float[] { 60f, 40f })).UseAllAvailableWidth().SetMarginTop(10);
                tabelaAssinatura.AddCell(new Cell().Add(new Paragraph("Assinatura:").SetFont(fontNegrito).SetFontSize(9)).SetBorder(Border.NO_BORDER).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
                tabelaAssinatura.AddCell(new Cell().SetBorder(Border.NO_BORDER));

                if (AssinaturaFinal != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        // Como nós agora definimos o fundo da janela como Transparente, 
                        // ao guardar como PNG ele vai respeitar a transparência e mostrar o cinzento da tabela!
                        AssinaturaFinal.Save(ms, ImageFormat.Png);
                        byte[] assinaturaBytes = ms.ToArray();
                        iText.Layout.Element.Image assinaturaImg = new iText.Layout.Element.Image(ImageDataFactory.Create(assinaturaBytes))
                                                .ScaleToFit(150, 50).SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);

                        // O fundo LIGHT_GRAY aplica-se, mas como a imagem é Png Transparente, os traços ficam por cima do cinzento!
                        tabelaAssinatura.AddCell(new Cell(3, 1).Add(assinaturaImg).SetBorder(Border.NO_BORDER).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetMaxHeight(50).SetMinHeight(50));
                    }
                }
                else
                {
                    tabelaAssinatura.AddCell(new Cell(3, 1).Add(new Paragraph("")).SetBorder(Border.NO_BORDER).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetMinHeight(50));
                }

                tabelaAssinatura.AddCell(new Cell().SetBorder(Border.NO_BORDER));

                // =========================================================
                // 6. MONTAR O DOCUMENTO
                // =========================================================
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
                    "EPIs no final do contrato, o respetivo valor será descontado no último recibo de vencimento, dando desde já expressa autorização para esse efeito.")
                    .SetTextAlignment(TextAlignment.JUSTIFIED));

                doc.Add(tabelaAssinatura);

                // Texto de rodapé com o nome do PEPIDI
                doc.Add(new Paragraph("Processado por computador - PEPIDI").SetMultipliedLeading(0.8f).SetMargin(0.5f).SetFontSize(6).SetFontColor(ColorConstants.GRAY));

                doc.Close();
                return caminhoPDF;
            }
        }

        public static string GerarListaSeparacaoPorFuncionario(List<(string NMEC, string Nome, string Modelo, string Tamanho, int Qtd)> itensParaArmazem)
        {
            string caminhoBase = ObterCaminhoDoSQL("CaminhoRelatorios", "RelatoriosPEPIDI_Backup");

            if (!Directory.Exists(caminhoBase)) Directory.CreateDirectory(caminhoBase);

            string caminhoPDF = Path.Combine(caminhoBase, $"ListaSeparacao_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");

            using (PdfWriter writer = new PdfWriter(caminhoPDF))
            using (PdfDocument pdf = new PdfDocument(writer))
            {
                Document doc = new Document(pdf);

                // AS MESMAS MARGENS DO COMPROVATIVO OFICIAL
                doc.SetMargins(1f * 28.35f, 1.29f * 28.35f, 1.27f * 28.35f, 1.35f * 28.35f);

                PdfFont fontNegrito = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                PdfFont fontNormal = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                doc.SetFont(fontNormal).SetFontSize(8);

                // =========================================================
                // 1. CABEÇALHO (LOGO E TÍTULO)
                // =========================================================
                Table tabelaCabecalho = new Table(UnitValue.CreatePercentArray(new float[] { 50f, 50f })).UseAllAvailableWidth();

                Cell cellImagem = new Cell().SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT).SetPadding(0);
                System.Drawing.Image logoProjeto = Properties.Resources.logo; // O teu Logo
                if (logoProjeto != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        logoProjeto.Save(ms, ImageFormat.Png);
                        byte[] logoBytes = ms.ToArray();
                        iText.Layout.Element.Image img = new iText.Layout.Element.Image(ImageDataFactory.Create(logoBytes));
                        img.ScaleToFit(100, 100);
                        cellImagem.Add(img);
                    }
                }

                Table tabelaNota = new Table(UnitValue.CreatePercentArray(new float[] { 80f, 20f })).UseAllAvailableWidth();
                tabelaNota.AddCell(new Cell().Add(new Paragraph("LISTA DE SEPARAÇÃO POR FUNCIONÁRIO").SetMultipliedLeading(0.001f).SetFont(fontNegrito).SetFontSize(9)).SetTextAlignment(TextAlignment.LEFT).SetBorder(Border.NO_BORDER));
                tabelaNota.AddCell(new Cell().Add(new Paragraph("ARMAZÉM").SetMultipliedLeading(0.001f).SetFont(fontNegrito).SetFontSize(9).SetFontColor(ColorConstants.GRAY)).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER));
                tabelaNota.AddCell(new Cell(1, 2).SetBorderBottom(new SolidBorder(ColorConstants.LIGHT_GRAY, 1)).SetBorderTop(Border.NO_BORDER).SetBorderLeft(Border.NO_BORDER).SetBorderRight(Border.NO_BORDER));
                tabelaNota.AddCell(new Cell().Add(new Paragraph("Data de Processamento")).SetBorder(Border.NO_BORDER));
                tabelaNota.AddCell(new Cell().Add(new Paragraph(DateTime.Now.ToString("dd/MM/yyyy HH:mm"))).SetTextAlignment(TextAlignment.RIGHT).SetBorder(Border.NO_BORDER));

                tabelaCabecalho.AddCell(cellImagem);
                tabelaCabecalho.AddCell(new Cell().Add(tabelaNota).SetBorder(Border.NO_BORDER));

                // =========================================================
                // 2. INFORMAÇÃO EMPRESA
                // =========================================================
                Table tabelaInfo = new Table(UnitValue.CreatePercentArray(new float[] { 50f, 50f })).UseAllAvailableWidth().SetMarginBottom(15);

                Cell cellEmpresa = new Cell().SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT);
                cellEmpresa.Add(new Paragraph("Documento Interno - Uso Exclusivo").SetFontSize(8).SetFontColor(ColorConstants.GRAY).SetMultipliedLeading(1.1f));

                tabelaInfo.AddCell(cellEmpresa);
                tabelaInfo.AddCell(new Cell().SetBorder(Border.NO_BORDER)); // Célula vazia à direita

                doc.Add(tabelaCabecalho);
                doc.Add(tabelaInfo);

                // =========================================================
                // 3. AGRUPAR E LISTAR ROUPAS POR FUNCIONÁRIO
                // =========================================================
                var listaAgrupada = itensParaArmazem.GroupBy(x => new { x.NMEC, x.Nome });

                foreach (var grupo in listaAgrupada)
                {
                    // Barra de Destaque para o Nome do Funcionário
                    Table tableNome = new Table(UnitValue.CreatePercentArray(new float[] { 100f })).UseAllAvailableWidth().SetMarginTop(10);
                    tableNome.AddCell(new Cell().Add(new Paragraph($"NMEC: {grupo.Key.NMEC}   |   {grupo.Key.Nome}").SetFont(fontNegrito).SetFontSize(9))
                        .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                        .SetBorder(Border.NO_BORDER)
                        .SetPadding(1));
                    doc.Add(tableNome);

                    // Tabela com os Itens desse Funcionário
                    Table tableItens = new Table(UnitValue.CreatePercentArray(new float[] { 15f, 55f, 20f, 10f })).UseAllAvailableWidth();

                    foreach (var item in grupo)
                    {
                        // 1. CHECKBOX OFICIAL INTERATIVA
                        string checkID = $"chk_{Guid.NewGuid()}";

                        iText.Forms.Form.Element.CheckBox checkbox = new iText.Forms.Form.Element.CheckBox(checkID);
                        checkbox.SetWidth(12f);
                        checkbox.SetHeight(12f);

                        // O SEGREDO AQUI: Obrigar o PDF a desenhar o quadrado visual para a checkbox não ficar transparente!
                        checkbox.SetBorder(new iText.Layout.Borders.SolidBorder(iText.Kernel.Colors.ColorConstants.BLACK, 0.5f));
                        checkbox.SetBackgroundColor(iText.Kernel.Colors.ColorConstants.WHITE);

                        var cellCheck = new Cell().Add(checkbox)
                            .SetBorder(Border.NO_BORDER)
                            .SetPaddingLeft(5f)
                            .SetVerticalAlignment(VerticalAlignment.MIDDLE);


                        // 2. Quantidade
                        tableItens.AddCell(new Cell().Add(new Paragraph($"{item.Qtd} x").SetFont(fontNegrito)).SetBorder(Border.NO_BORDER).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetTextAlignment(TextAlignment.CENTER));

                        // 3. Artigo / Modelo
                        tableItens.AddCell(new Cell().Add(new Paragraph(item.Modelo)).SetBorder(Border.NO_BORDER).SetTextAlignment(TextAlignment.LEFT));

                        // 4. Tamanho
                        tableItens.AddCell(new Cell().Add(new Paragraph($"Tam: {item.Tamanho}").SetFont(fontNegrito)).SetBorder(Border.NO_BORDER).SetVerticalAlignment(VerticalAlignment.MIDDLE).SetTextAlignment(TextAlignment.CENTER));
                        
                        tableItens.AddCell(cellCheck).SetVerticalAlignment(VerticalAlign).SetTextAlignment(TextAlignment.RIGHT);
                    }
                    doc.Add(tableItens);

                }
                // 1. Crias o estilo da linha (espessura 0.5f e cor LIGHT_GRAY)
                iText.Kernel.Pdf.Canvas.Draw.SolidLine linha = new iText.Kernel.Pdf.Canvas.Draw.SolidLine(0.5f);
                linha.SetColor(ColorConstants.LIGHT_GRAY);

                // 2. Crias o separador e adicionas ao documento
                LineSeparator linhaSeparadora = new LineSeparator(linha);
                linhaSeparadora.SetMarginTop(3f);    // Espaço em cima da linha
                linhaSeparadora.SetMarginBottom(0f); // Espaço por baixo da linha

                doc.Add(linhaSeparadora);
                doc.Add(new Paragraph("\nGerado por computador - PEPIDI").SetMultipliedLeading(0.8f).SetMargin(0.5f).SetFontSize(6).SetFontColor(ColorConstants.GRAY));

                doc.Close();
            }

            return caminhoPDF;
        }
    }
}