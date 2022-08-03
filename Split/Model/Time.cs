using Split.Util;
using System.Text.RegularExpressions;

namespace Split.Model
{
    public class Time: BaseInotifyPropertyChanged
    {
        public Time()
        {
            setDefault();
        }


        public void Clear()
        {
            setDefault();
        }

        private void setDefault()
        {
            Order = "Horas Minutos Segundos";
            MaskFormat = "HH:mm:ss";
            Separator = ":";
            Format = false;
        }


        private string _order;

        public string Order
        {
            get { return _order; }
            set
            {
                _order = value;
                OnPropertyChanged("Order");

                setMask();
                time_Format();
            }
        }

        private string _separator;

        public string Separator
        {
            get { return _separator; }
            set
            {
                _separator = "";

                if (value != "none")
                    _separator = value;

                OnPropertyChanged("Separator");

                setMask();
                time_Format();
            }
        }

        private string _maskFormat;
        public string MaskFormat
        {
            get { return this._maskFormat; }
            set
            {
                this._maskFormat = value;
                OnPropertyChanged("MaskFormat");

                time_Format();
            }
        }

        private bool _fomart;
        public bool Format
        {
            get
            {
                return this._fomart;
            }

            set
            {
                this._fomart = value;
                OnPropertyChanged("Format");

                setMask();
                time_Format();

            }
        }

        private string _sample;
        public string Sample
        {
            get
            {
                return this._sample;
            }

            set
            {
                this._sample = value;
                OnPropertyChanged("Sample");
            }
        }

        private void time_Format()
        {
            Sample = LogicVariable.GetTime(MaskFormat);
        }


        private void setMask()
        {

            var hora = Format ? "hh" : "HH";

            if (Regex.IsMatch(Order, "Horas Minutos Segundos"))
                MaskFormat = $"{hora}{Separator}mm{Separator}ss";
            else
                if (Regex.IsMatch(Order, "Horas Minutos"))
                MaskFormat = $"{hora}{Separator}mm";
        }

    }

}

