namespace Split.Model
{
    public class Font:BaseInotifyPropertyChanged
    {

        public Font()
        {
            Family = "Arial";
            Size = 8;
            Red = Green = Blue = 0;
        }


        private string _family;
        public string Family
        {
            get { return _family; }
            set
            {
                _family = value;
                OnPropertyChanged("Family");
                defineFontString();
            }
        }

        private int _red;
        public int Red
        {
            get { return _red; }
            set
            {
                _red = value;
                OnPropertyChanged("Red");
                defineFontString();
            }
        }

        private int _green;
        public int Green
        {
            get { return _green; }
            set
            {
                _green = value;
                OnPropertyChanged("Green");
                defineFontString();
            }
        }

        private int _blue;
        public int Blue
        {
            get { return _blue; }
            set
            {
                _blue = value;
                OnPropertyChanged("Blue");
                defineFontString();
            }
        }

        private float _size;
        public float Size
        {
            get { return _size; }
            set
            {
                _size = value;
                OnPropertyChanged("Size");
                defineFontString();
            }
        }

        public override string ToString()
        {
            return $"{Family}, {Size} - R: {Red} - G: {Green} - B: {Blue}";
        }

        private void defineFontString()
        {
            FontString = ToString();
        }

        private string _fontString;
        public string FontString
        {
            get { return _fontString; }
            set
            {
                _fontString = value;
                OnPropertyChanged("FontString");
            }
        }


        
    }
}
