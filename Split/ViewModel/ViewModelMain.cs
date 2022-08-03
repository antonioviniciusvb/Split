using Split.Model;
using System.Collections.ObjectModel;
using System.Text;

namespace Split.ViewModel
{
    public class ViewModelMain:BaseInotifyPropertyChanged
    {
        #region Config
        private ObservableCollection<Layout> _layouts;
        public ObservableCollection<Layout> Layouts
        {
            get { return _layouts; }

            set
            {
                _layouts = value;
                OnPropertyChanged("Layouts");
            }
        }

        #endregion

        #region Process

        private ObservableCollection<FileProcess> _fileProcess;
        public ObservableCollection<FileProcess> FileProcess
        {
            get { return _fileProcess; }

            set
            {
                _fileProcess = value;
                OnPropertyChanged("FileProcess");
            }
        }

        #endregion

        public Command AddPDF { get; set; }

        public ViewModelMain()
        {
            _layouts = new ObservableCollection<Layout>();
            _fileProcess = new ObservableCollection<FileProcess>();

            //Criar Pasta que conterá os layouts
            Util.IO.createDirectory(Util.Global_Variable.PathLayout);

            var filesLayout = Util.Query.GetLayouts();

            _layouts = Util.Json.FileJsonToLayout(filesLayout);

            AddPDF = new Command(addPDF, () => { return true; });

        }

        private void addPDF()
        {
            var files = Util.Process.addFilesPDF();
            StringBuilder stringBuilder = new StringBuilder();

            if (files != null)
            {
                //Retirar os arquivos duplicados
                for (int i = 0; i < FileProcess.Count; i++)
                {
                    var find = files.RemoveAll(x => x == FileProcess[i].Name);

                    if (find > 0)
                        stringBuilder.AppendLine(FileProcess[i].Name);
                }

                //Adicionar
                for (int index = 0; index < files.Count; index++)
                {
                    var pgs = PDF.Itext.ExtractNumerOfPages(files[index]);

                    FileProcess.Add(new FileProcess { Name = files[index], Pgs = pgs });
                }

                if (stringBuilder.Length > 0)
                    System.Windows.Forms.MessageBox.Show($"{stringBuilder}", "O(s) seguinte(s) arquivo(s) já foram adicionado(s) anteriormente:");
                else 
                    if(files.Count > 0)
                    System.Windows.Forms.MessageBox.Show("Arquivo(s) adicionado(s) com Sucesso!", "", 
                         System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            }
        }
    }
}
