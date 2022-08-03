using System;
using System.ComponentModel;

namespace Split.Model
{
    public class Process : BaseInotifyPropertyChanged
    {

        public Process()
        {
            Files = new BindingList<FileProcess>();
        }

        private string _id;
        public string Id
        {
            get { return _id; }

            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        private TimeSpan _time;
        public TimeSpan Time
        {
            get { return _time; }

            set
            {
                _time = value;
                OnPropertyChanged("Time");
            }
        }

        private int _split;
        public int Split
        {
            get { return _split; }

            set
            {
                _split = value;
                OnPropertyChanged("Split");
            }
        }

        private string _layout;
        public string Layout
        {
            get { return _layout; }

            set
            {
                _layout = value;
                OnPropertyChanged("Layout");
            }
        }

        public BindingList<FileProcess> Files { get; set; }
    }
}
