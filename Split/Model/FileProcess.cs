namespace Split.Model
{
    public class FileProcess : BaseInotifyPropertyChanged
    {
        private string _name;
        public string Name
        {
            get { return _name; }

            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private int _pgs;
        public int Pgs
        {
            get { return _pgs; }

            set
            {
                _pgs = value;
                OnPropertyChanged("Pgs");
            }
        }

    }
}
