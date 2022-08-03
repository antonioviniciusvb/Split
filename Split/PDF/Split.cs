using System;
using System.IO;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Diagnostics;
using Split.Model;
//using BarcodeLib;
using Split.Util;
using System.Text;
using System.Drawing;

namespace Split.PDF
{
    public static class Split
    {
        public static Bitmap GetBarcode(string code)
        {
            Barcode128 barcode = new Barcode128();
            barcode.CodeType = Barcode128.CODE128;
            barcode.Code = code;
            barcode.AltText = code;
            barcode.TextAlignment = Barcode128.SHIFT;
            Bitmap barcode_full = new Bitmap(barcode.CreateDrawingImage(Color.Black, Color.White));
            return barcode_full;
        }

        public static void IncludeFields(string source, Layout layout)
        {
            try
            {
                // Barcode
                iTextSharp.text.pdf.Barcode128 bc = new Barcode128();
                bc.X = 1f;
                bc.BarHeight = 30f;
                bc.AltText = "";
                bc.CodeType = Barcode128.CODE128;

                FontFactory.RegisterDirectory("C:\\WINDOWS\\Fonts");

                LogicVariable.time = LogicVariable.GetTime();

                Stopwatch sw = new Stopwatch();
                sw.Start();

                LogicVariable.ResetVariable();

                LogicVariable.file = layout.File;

                StringBuilder expression_Tmp = new StringBuilder();

                var fileInfo = new FileInfo(source);
                var reader = Itext.getReader(fileInfo.FullName);

                var pageRotation = reader.GetPageRotation(1);
                var pageWidth = reader.GetPageSizeWithRotation(1).Width;
                var pageHeight = reader.GetPageSizeWithRotation(1).Height;

                var doc = Itext.getDocument(reader);

                PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream($"OUTPUT_TESTE.PDF", FileMode.Create));
                doc.Open();

                var cb = writer.DirectContent;

                doc.NewPage();

                //Pré-Processamento das Variaveis  de data, hora e nome do arquivo
                for (int indexField = 0; indexField < layout.Fields.Count; indexField++)
                {
                    expression_Tmp.Append(layout.Fields[indexField].Expression);

                    Regexp.CalculateLogics(ref expression_Tmp);

                    layout.Fields[indexField].Value = $"{expression_Tmp}";

                    expression_Tmp.Clear();

                    float pos_x = Utilities.MillimetersToPoints(layout.Fields[indexField].Position_x + 2);
                    float pos_y = Utilities.MillimetersToPoints(layout.Fields[indexField].Position_y);

                    if (layout.Fields[indexField].Type == "Barcode")
                    {
                        if (layout.Fields[indexField].Rotation == 90 || layout.Fields[indexField].Rotation == 270)
                        {
                            layout.Fields[indexField].Rotation = 270;
                            pos_x = pos_x - bc.BarHeight;
                            pos_y = pageHeight - (pos_y + 90);
                        }
                        else
                        {
                            layout.Fields[indexField].Rotation = 180;
                            pos_y = pageHeight - (pos_y + bc.BarHeight);
                        }
                    }
                    else
                    {
                 
                    }

                    layout.Fields[indexField].Position_x = pos_x;
                    layout.Fields[indexField].Position_y = pos_y;

                }

                #region Query All Pages

                //Todas as páginas
                var all_Logic = layout.Fields.Where(
                                             x =>
                                             x.Display == 0 &&
                                             x.Type == "Lógico")
                                             .ToList<Field>();

                //Barcode em todas as páginas
                var all_Barcode = layout.Fields.Where(
                                             x =>
                                             x.Display == 0 &&
                                             x.Type == "Barcode")
                                             .ToList<Field>();

                #endregion

                #region Query Odd Pages

                //Páginas impares
                var odd_Logic = layout.Fields.Where(
                                           x =>
                                           x.Display == 1 &&
                                           x.Type == "Lógico")
                                          .ToList<Field>();


                //Barcode nas impares
                var odd_Barcode = layout.Fields.Where(x => x.Display == 1 && x.Type == "Barcode").ToList<Field>();


                #endregion

                #region Query Even Pages

                //Todas as páginas pares
                var even_Logic = layout.Fields.Where(x =>
                                           x.Display == 2 &&
                                           x.Type == "Lógico")
                                           .ToList<Field>();

                //Todos os Barcode pares
                var even_Barcode = layout.Fields.Where(x => x.Display == 2 && x.Type == "Barcode").ToList<Field>();

                #endregion


                for (int indexPage = 1; indexPage <= reader.NumberOfPages; indexPage++)
                {

                    #region All Pages

                    LogicVariable.count_pages++;
                    LogicVariable.count_field = LogicVariable.count_pages;

                    var page = writer.GetImportedPage(reader, indexPage);

                    cb.AddTemplate(page, 0, 0);

                    cb.BeginText();

                    for (int i = 0; i < all_Logic.Count; i++)
                    {
                        var font = FontFactory.GetFont(all_Logic[i].Font.Family, all_Logic[i].Font.Size,
                                   new BaseColor(all_Logic[i].Font.Red, all_Logic[i].Font.Green, all_Logic[i].Font.Blue));

                        cb.SetFontAndSize(font.BaseFont, font.Size);
                        cb.SetColorFill(font.Color);

                        cb.ShowTextAligned(

                                PdfContentByte.ALIGN_LEFT,
                                string.Format(all_Logic[i].Value, LogicVariable.count_pages, LogicVariable.count_field, LogicVariable.count_split),
                                all_Logic[i].Position_x,
                                page.Height - all_Logic[i].Position_y,
                                all_Logic[i].Rotation);
                    }

                    for (int i = 0; i < all_Barcode.Count; i++)
                    {

                        bc.Code = string.Format(all_Barcode[i].Value, LogicVariable.count_pages, LogicVariable.count_field, LogicVariable.count_split);
                        iTextSharp.text.Image image = bc.CreateImageWithBarcode(cb, BaseColor.BLACK, BaseColor.BLACK);
                        image.RotationDegrees = all_Barcode[i].Rotation;

                        image.SetAbsolutePosition(all_Barcode[i].Position_x, all_Barcode[i].Position_y);
                        cb.AddImage(image);
                    }

                    #endregion

                    #region Pares

                    if ((LogicVariable.count_pages & 1) == 0)
                    {
                        LogicVariable.count_even++;
                        LogicVariable.count_field = LogicVariable.count_even;

                        for (int i = 0; i < even_Logic.Count; i++)
                        {
                            var font = FontFactory.GetFont(even_Logic[i].Font.Family, even_Logic[i].Font.Size,
                                       new BaseColor(even_Logic[i].Font.Red, even_Logic[i].Font.Green, even_Logic[i].Font.Blue));

                            cb.SetFontAndSize(font.BaseFont, font.Size);
                            cb.SetColorFill(font.Color);

                            cb.SetFontAndSize(font.BaseFont, font.Size);
                            cb.SetColorFill(font.Color);

                            cb.ShowTextAligned(

                                    PdfContentByte.ALIGN_LEFT,
                                    string.Format(even_Logic[i].Value, LogicVariable.count_pages, LogicVariable.count_field, LogicVariable.count_split),
                                    even_Logic[i].Position_x,
                                    page.Height - even_Logic[i].Position_y,
                                    even_Logic[i].Rotation);
                        }

                        for (int i = 0; i < even_Barcode.Count; i++)
                        {
                            bc.Code = string.Format(even_Barcode[i].Value, LogicVariable.count_pages, LogicVariable.count_field, LogicVariable.count_split);
                            iTextSharp.text.Image image = bc.CreateImageWithBarcode(cb, BaseColor.BLACK, BaseColor.BLACK);
                            image.RotationDegrees = even_Barcode[i].Rotation;

                            image.SetAbsolutePosition(even_Barcode[i].Position_x, even_Barcode[i].Position_y);
                            cb.AddImage(image);
                        }

                    }
                    #endregion

                    #region Impares
                    else
                    {
                        
                        LogicVariable.count_odd++;
                        LogicVariable.count_field = LogicVariable.count_odd;

                        for (int i = 0; i < odd_Logic.Count; i++)
                        {

                            var font = FontFactory.GetFont(odd_Logic[i].Font.Family, odd_Logic[i].Font.Size,
                                       new BaseColor(odd_Logic[i].Font.Red, odd_Logic[i].Font.Green, odd_Logic[i].Font.Blue));

                            cb.SetFontAndSize(font.BaseFont, font.Size);
                            cb.SetColorFill(font.Color);

                            cb.ShowTextAligned(

                                    PdfContentByte.ALIGN_LEFT,
                                    string.Format(odd_Logic[i].Value, LogicVariable.count_pages, LogicVariable.count_field, LogicVariable.count_split),
                                    odd_Logic[i].Position_x,
                                    page.Height - odd_Logic[i].Position_y,
                                    odd_Logic[i].Rotation);
                        }


                        for (int i = 0; i < odd_Barcode.Count; i++)
                        {
                            bc.Code = string.Format(odd_Barcode[i].Value, LogicVariable.count_pages, LogicVariable.count_field, LogicVariable.count_split);
                            iTextSharp.text.Image image = bc.CreateImageWithBarcode(cb, BaseColor.BLACK, BaseColor.BLACK);
                            image.RotationDegrees = odd_Barcode[i].Rotation;

                            image.SetAbsolutePosition(odd_Barcode[i].Position_x, odd_Barcode[i].Position_y);
                            cb.AddImage(image);
                        }

                    }

                    #endregion

                    cb.EndText();

                    doc.NewPage();

                }

                doc.Close();

                sw.Stop();


                TimeSpan ts = sw.Elapsed;

                Debug.WriteLine($"{ts.Minutes}:{ts.Seconds}:{ts.Milliseconds}");

                System.Diagnostics.Process.Start($"OUTPUT_TESTE.PDF");
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public static void Extract(string file, string output)
        {
            try
            {
                var fileInfo = new FileInfo(file);
                var reader = Itext.getReader(fileInfo.FullName);

                //Caso o PDF de layout só tenha uma página
                var pgsExtract = reader.NumberOfPages > 1 ? 3 : 2;

                var doc = Itext.getDocument(reader);

                PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream($"{output}", FileMode.Create));

                doc.Open();

                PdfContentByte cb = writer.DirectContent;

                for (int i = 1; i < pgsExtract; i++)
                {
                    var page = writer.GetImportedPage(reader, i);

                    cb.AddTemplate(page, 0, 0);

                    doc.NewPage();
                }

                doc.Close();
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
