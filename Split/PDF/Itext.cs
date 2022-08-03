using iTextSharp.text;
using iTextSharp.text.pdf;
using System;

namespace Split.PDF
{
    public static class Itext
    {
        public static Document getDocument(PdfReader reader)
        {
            //Size Page 
            Rectangle sizePage = reader.GetPageSizeWithRotation(1);

            //Document
            Document doc = new Document(new Rectangle(sizePage.Width, sizePage.Height));

            return doc;
        }

        public static PdfReader getReader(string file)
        {
            //Reader
            PdfReader reader = new PdfReader($"{file}");

            return reader;
        }


        public static int ExtractNumerOfPages(string file)
        {
            int pgs = 0;

            try
            {
                    using (PdfReader reader = new PdfReader(file))
                    {
                        pgs = reader.NumberOfPages;
                        reader.Close();
                    }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return pgs;
        }

    }
}
