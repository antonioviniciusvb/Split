using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;

namespace Split.Model
{
    public class Layout:TreeViewItemBase
    {
        public Layout()
        {
            this.Fields = new BindingList<Field>();
        }

        private int _id;
        public int Id
        {
            get { return _id; }

            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                //Limite de 25 caracteres para o nome do campo 
                //Permitir apenas letras e underscore
                if (value?.Length < 26)
                {
                    _name = Regex.Replace(value, @"[^A-Za-z_0-9]", "").ToUpper();
                    OnPropertyChanged("Name");
                }
            }
        }

        private string _file;
        public string File
        {
            get { return _file; }

            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _file = new  FileInfo(value).Name;
                    OnPropertyChanged("File");
                }
            }
        }

        private float _height;
        public float Height
        {
            get { return _height; }
            set
            {
                _height = value;
                OnPropertyChanged("Height");
            }
        }

        private float _width;
        public float Width
        {
            get { return _width; }
            set
            {
                _width = value;
                OnPropertyChanged("Width");
            }
        }

        public BindingList<Field> Fields { get; set; }
    }
}
