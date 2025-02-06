namespace EZSave.Progress.Models
{
    using System.ComponentModel;

    public class ProgressModel : INotifyPropertyChanged
    {
        private int _filesLeft;
        private float _totalFileSizeLeft;
        private string _source;
        private string _destination;

        public int FilesLeft
        {
            get => _filesLeft;
            set { _filesLeft = value; OnPropertyChanged(nameof(FilesLeft)); }
        }

        public float TotalFileSizeLeft
        {
            get => _totalFileSizeLeft;
            set { _totalFileSizeLeft = value; OnPropertyChanged(nameof(TotalFileSizeLeft)); }
        }

        public string Source
        {
            get => _source;
            set { _source = value; OnPropertyChanged(nameof(Source)); }
        }

        public string Destination
        {
            get => _destination;
            set { _destination = value; OnPropertyChanged(nameof(Destination)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
