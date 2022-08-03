using Split.Model;
using System;
using Split.PDF;
using iTextSharp.text;

namespace Split.ViewModel
{
    public class VmLayout : BaseInotifyPropertyChanged
    {
        #region Config
        private Layout _config_layout;
        public Layout ConfigLayout
        {
            get { return _config_layout; }
        }

        #endregion

        #region Commands

        public Command CreateLayout { get; set; }

        public Command DefinePDF { get; set; }

        public Command Close { get; set; }

        #endregion

        private string _file;
        public string File
        {
            get { return _file; }

            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _file = value;
                    OnPropertyChanged("File");
                }
            }
        }

        private View.EditLayout _windowLayout;

        public VmLayout(View.EditLayout window)
        {
            _windowLayout = window;
            _config_layout = new Layout();
            _config_layout.Id = Util.Query.NextIdLayout();
           
            CreateLayout = new Command(this.createLayout, () => { return (!string.IsNullOrWhiteSpace(ConfigLayout.File)) &&
                                                                          (!string.IsNullOrWhiteSpace(ConfigLayout.Name)); });
            DefinePDF = new Command(this.definePDF, () => {return true; });


            Close = new Command(this.close, () => { return true; });
        }

        private void definePDF()
        {
            File = Util.Process.defineFilePDF();
            _config_layout.File = File;

            if (!string.IsNullOrWhiteSpace(File))
            {
                var docPdf = Itext.getDocument(Itext.getReader(File));

                float height = Utilities.PointsToMillimeters(docPdf.PageSize.Height);
                float width = Utilities.PointsToMillimeters(docPdf.PageSize.Width);

                var h_convert = (float) Math.Round(height, 1);
                var w_convert = (float) Math.Round(width, 1);

                _config_layout.Height = h_convert;
                _config_layout.Width = w_convert;
            }
        }


        private void close()
        {
            _windowLayout.Close();
        }

        /// <summary>
        /// Metodo para criar os arquivos layout
        /// Passo 1 - Cria a Pasta para o Layout
        /// Passo 2 - Cria o Arquivo de PDF com apenas 1 página para modelo
        /// Passo 3 - Cria o Arquivo de Layout
        /// </summary>
        private void createLayout()
        {
            var path = Util.IO.createDirectory(Util.Query.NextDirLayout());

            var fullNameLayout = $"{Util.Query.LastDirLayout()}\\{_config_layout.Name}.lyt";

            var fullNameLayoutPDF = $"{Util.Query.LastDirLayout()}\\{_config_layout.File}";

            PDF.Split.Extract(File, fullNameLayoutPDF);

            Util.Json.SaveObject(fullNameLayout, _config_layout);

            _windowLayout.layout.Add(Util.Json.FileJsonToLayout(fullNameLayout));

            _windowLayout.msgSave(_config_layout);
        }

    }

}
