using System;
using System.Collections.Generic;

namespace Split.Util
{
    public static class Process
    {
        public static string defineFilePDF()
        {
            Microsoft.Win32.OpenFileDialog openFile = new Microsoft.Win32.OpenFileDialog();
            openFile.Title = "Escolha um arquivo PDF";
            openFile.ValidateNames = true;
            openFile.InitialDirectory = @"c:\";
            openFile.Filter = "Arquivo PDF|*.pdf";

            Nullable<bool> result = openFile.ShowDialog();

            if (result == true &&  Regexp.ExpressionExists(openFile.FileName, Regexp.isPdf))
                return openFile.FileName;
            else
                return string.Empty;
        }


        public static List<string> addFilesPDF()
        {
            Microsoft.Win32.OpenFileDialog openFile = new Microsoft.Win32.OpenFileDialog();

            List<string> filesPDF = new List<string>();

            openFile.Title = "Escolha os arquivos PDF";
            openFile.ValidateNames = true;
            openFile.InitialDirectory = @"c:\";
            openFile.Filter = "Arquivos PDF|*.pdf";
            openFile.Multiselect = true;

            Nullable<bool> result = openFile.ShowDialog();

            if (result == true)
            {
                for (int indexFile = 0; indexFile < openFile.FileNames.Length; indexFile++)
                {
                    if(Regexp.ExpressionExists(openFile.FileNames[indexFile], Regexp.isPdf))
                    {
                        filesPDF.Add(openFile.FileNames[indexFile]);
                    }
                }

                return filesPDF;
            }
            else
                return null;
        }
    }
}
