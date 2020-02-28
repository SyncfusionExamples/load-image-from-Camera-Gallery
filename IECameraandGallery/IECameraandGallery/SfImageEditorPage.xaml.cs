using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IECameraandGallery
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SfImageEditorPage : ContentPage
    {
        private string _fileName;

        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                OnPropertyChanged("FileName");
                OpenImageEditor(_fileName);
            }
        }

        private ImageSource _imageSource;
        public ImageSource ImageSource
        {
            get { return _imageSource; }
            set
            {
                _imageSource = value;
                OnPropertyChanged();
                OpenImageEditor(_imageSource);
            }
        }
        private Stream _stream;
        public Stream Stream
        {
            get { return _stream; }
            set
            {
                _stream = value;
                OnPropertyChanged();
                OpenImageEditor(_stream);
            }
        }

        public SfImageEditorPage()
        {
            InitializeComponent();
        }
        void OpenImageEditor(string file)
        {
            editor.Source = ImageSource.FromFile(file);
        }
        void OpenImageEditor(Stream stream)
        {
            editor.Source = ImageSource.FromStream(() => stream);
        }
        void OpenImageEditor(ImageSource imageSource)
        {
            editor.Source = imageSource;
        }
    }

    public interface IImageEditorDependencyService
    {
        void UploadFromCamera(MainPage editor);

        void UploadFromGallery(MainPage editor);
    }
}