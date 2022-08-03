using System.Text.RegularExpressions;
using Split.Util;

namespace Split.Model
{
    public class Field : BaseInotifyPropertyChanged
    {
        public Field()
        {
            _font = new Font();
            Position_x = 6;
            Position_y = 6;
            Rotation = 0;
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

        private string _type;
        public string Type
        {
            get { return _type; }
            set
            {
                _type = value;
                OnPropertyChanged("Type");
            }
        }


        private string _selectedValue;
        public string SelectedValue
        {
            get { return _selectedValue; }
            set
            {
                _selectedValue = value;
                OnPropertyChanged("SelectedValue");

                Type = value.Replace("System.Windows.Controls.ComboBoxItem: ", "");
            }
        }

        private int _display;
        public int Display
        {
            get { return _display; }
            set
            {
                _display = value;
                OnPropertyChanged("Display");
            }
        }

        private float _position_x;
        public float Position_x
        {
            get { return _position_x; }
            set
            {
                if (value < 6)
                    value = 6;

                    _position_x = value;
                    OnPropertyChanged("Position_x");
            }
        }

        private float _position_y;
        public float Position_y
        {
            get { return _position_y; }
            set
            {
                if (value < 6)
                    value = 6;

                _position_y = value;
                OnPropertyChanged("Position_y");
            }
        }

        private Font _font;
        public Font Font
        {
            get { return _font; }
            set
            {
                _font = value;
                OnPropertyChanged("Font");
            }
        }
       

        private int _rotation;
        public int Rotation
        {
            get { return _rotation; }
            set
            {
                _rotation = value;
                OnPropertyChanged("Rotation");
            }
        }


        private string _expression;
        public string Expression
        {
            get { return _expression; }
            set
            {
                    _expression = value;
                    OnPropertyChanged("Expression");
                    defineSample();
            }
        }

        private void defineSample()
        {
           Value =  Regexp.CreateLogicVariable(Expression);
        }

        private string _value;
        public string Value
        {
            get
            {
                return this._value;
            }

            set
            {
                this._value = value;
                OnPropertyChanged("Value");
            }
        }

     
    }
}
