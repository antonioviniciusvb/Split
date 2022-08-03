using System.Text;
using Split.Util;

namespace Split.Model
{
    public class File : BaseInotifyPropertyChanged
    {

        private StringBuilder regex = new StringBuilder();

        private bool _extension;

        public bool Extension
        {
            get { return _extension; }
            set
            {
                _extension = value;
                OnPropertyChanged("Extension");
                setMask();
            }
        }

        private void setMask()
        {
            regex.Clear();

            if (Extension)
                regex.Append("S");
            else
                regex.Append("N");

            if (!string.IsNullOrWhiteSpace(SensitiveCase))
                regex.Append(SensitiveCase[0]);
            else
                regex.Append("N");

            MaskFile = $"{regex}";
        }

        private string _sensitiveCase;

        public string SensitiveCase
        {
            get { return _sensitiveCase; }
            set
            {
                _sensitiveCase = value;
                OnPropertyChanged("SensitiveCase");
                setMask();
            }
        }

        private string _maskFile;
        public string MaskFile
        {
            get { return _maskFile; }
            set
            {
                _maskFile = value;
                OnPropertyChanged("MaskFile");

                setSample();
            }
        }

        private void setSample()
        {
            Sample = LogicVariable.Get("fileName", maskformat: MaskFile);
        }

        private string _sample;
        public string Sample
        {
            get { return _sample; }
            set
            {
                _sample = value;
                OnPropertyChanged("Sample");
            }
        }

        public File()
        {
            setDefault();
        }

        private void setDefault()
        {
            Extension = true;
            SensitiveCase = "";

        }

        public void Clear()
        {
            setDefault();
            setSample();
        }
    }
}
