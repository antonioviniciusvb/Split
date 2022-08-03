using System.IO;

namespace Split.Model
{
    public class Test:BaseInotifyPropertyChanged
    {
        public Test()
        {
            Page = 1;
        }

        private bool _testAllFields;

        public bool TestAllFields
        {
            get { return _testAllFields; }
            set
            {
                _testAllFields = value;
                OnPropertyChanged("TestAllFields");
            }
        }

        private int _page;
        public int Page
        {
            get { return _page; }
            set
            {
                _page = value > 0 && value < 101 ? value : _page;
                OnPropertyChanged("Page");
            }
        }


        public string _file_full;
        public string File_Full
        {
            get { return _file_full; }

            set
            {
                _file_full = value;
                OnPropertyChanged("File_Full");

                File = new FileInfo(_file_full).Name;
            }
        }

        public string _file;
        public string File
        {
            get { return _file; }

            set
            {
                _file = value;
                OnPropertyChanged("File");
            }
        }

    }
}
