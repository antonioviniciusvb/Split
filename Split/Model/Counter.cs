using System.Text;
using Split.Util;

namespace Split.Model
{
    public class Counter : BaseInotifyPropertyChanged
    {

        private StringBuilder _format = new StringBuilder();

        private string _countField;

        public string Count_Field
        {
            get { return _countField; }
            set
            {
                _countField = value;
                OnPropertyChanged("Count_Field");
            }
        }

        private int _n_Digit;

        public int N_Digit
        {
            get { return _n_Digit; }
            set
            {
                if (value < 3)
                    _n_Digit = 3;
                else
                    if (value > 2 && value > 8)
                    _n_Digit = 8;
                else
                    _n_Digit = value;

                OnPropertyChanged("N_Digit");

                setMask();
            }
        }

        private void setMask()
        {
            _format.Clear();

            _format.Append("{0:");

            for (int i = 0; i < N_Digit; i++)
                _format.Append("0");

            _format.Append("}");

            MaskDigit = $"{_format}";
        }

        public Counter()
        {
            setDefault();
        }

        private void setDefault()
        {
            N_Digit = 7;
            Count_Field = "";
            setMask();
        }

        private string _maskDigit;
        public string MaskDigit
        {
            get { return this._maskDigit; }
            set
            {
                this._maskDigit = value;
                OnPropertyChanged("MaskDigit");

                setSample();
            }
        }

        private void setSample()
        {
            Sample = LogicVariable.Get("c", maskformat: MaskDigit);
        }

        private string _sample;
        public string Sample
        {
            get { return this._sample; }
            set
            {
                this._sample = value;
                OnPropertyChanged("Sample");
            }
        }

        public void Clear()
        {
            setDefault();
        }


    }
}
