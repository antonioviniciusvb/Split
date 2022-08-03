using System.Text.RegularExpressions;
using Split.Util;

namespace Split.Model
{
    public class Date : BaseInotifyPropertyChanged
    {
        public Date()
        {
            setDefault();
        }


        public void Clear()
        {
            setDefault();
        }

        private void setDefault()
        {
            Order = "Dia Mês Ano";
            MaskFormat = "dd/MM/yyyy";
            Separator = "/";
            IncDays = 0;
            WorkDay = false;
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
                date_Format();
            }
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
                date_Format();
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

                date_Format();
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

        private bool _workDay;
        public bool WorkDay
        {
            get
            {
                return this._workDay;
            }

            set
            {
                    this._workDay = value;
                    OnPropertyChanged("WorkDay");

                    date_Format();
            }
        }


        private int _incDays;
        public int IncDays
        {
            get
            {
                return this._incDays;
            }

            set
            {
                //Permite adicionar até 30 dias na data do atual do sistena
                if (value > -1 && value < 31)
                {
                    this._incDays = value;
                    OnPropertyChanged("IncDays");

                    date_Format();
                }
            }
        }

        private void date_Format()
        {
            Sample = LogicVariable.GetDate(MaskFormat, IncDays, WorkDay);
        }


        private void setMask()
        {
            if (Regex.IsMatch(Order, ": +Dia|^Dia"))
                MaskFormat = $"dd{Separator}MM{Separator}yyyy";

            if (Regex.IsMatch(Order, ": +Ano|^Ano"))
                MaskFormat =  $"yyyy{Separator}MM{Separator}dd";

            if (Regex.IsMatch(Order, ": +Mês|^Mês"))
                MaskFormat = $"MM{Separator}dd{Separator}yyyy";
        }

    }
}
