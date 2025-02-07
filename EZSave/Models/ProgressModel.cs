//namespace EZSave.Progress.Models
//{
//    using System.ComponentModel;

//    public class ProgressModel : INotifyPropertyChanged
//    {
//        private int _filesLeft;                         // nombres de fichiers restants
//        private float _totalFileSizeLeft;               // taille des fichiers
//        private string _source;
//        private string _destination;

//        public int FilesLeft
//        {
//            get => _filesLeft;
//            set { _filesLeft = value; OnPropertyChanged(nameof(FilesLeft)); }     //onpropertychanged --> chaque fois qu'une propriété change c appellé ( c ca qui rends dynamique données et affichage)
//                                                                                  //cela permet à une interface graphique de réagir aux changements et maj l'affichage
//        }

//        public float TotalFileSizeLeft
//        {
//            get => _totalFileSizeLeft;
//            set { _totalFileSizeLeft = value; OnPropertyChanged(nameof(TotalFileSizeLeft)); }
//        }

//        public string Source
//        {
//            get => _source;
//            set { _source = value; OnPropertyChanged(nameof(Source)); }
//        }

//        public string Destination
//        {
//            get => _destination;
//            set { _destination = value; OnPropertyChanged(nameof(Destination)); }
//        }

//        public event PropertyChangedEventHandler PropertyChanged;             // c la que on fait l'alerte ca réagis automatiquement
//        protected void OnPropertyChanged(string propertyName) =>
//            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); // un binding ( à revoir car sugggestionde gpt )
//    }
//}
