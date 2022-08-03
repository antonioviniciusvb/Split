namespace Split.Model
{
    public class TreeViewItemBase : BaseInotifyPropertyChanged
    {
        private bool isSelected;
        public bool IsSelected
        {
            get { return this.isSelected; }
            set
            {
                if (value != this.isSelected)
                {
                    this.isSelected = value;
                    OnPropertyChanged("IsSelected");
                }
            }
        }
    }
}
