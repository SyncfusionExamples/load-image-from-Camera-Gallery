using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IECameraAndGallery
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SfImageEditorPage : ContentPage
    {

        public SfImageEditorPage()
        {
            InitializeComponent();
        }

        public SfImageEditorPage(ImageSource source)
        {
            editor.Source = source;
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

        void OpenImageEditor(ImageSource imageSource)
        {
            editor.Source = imageSource;
        }
    }
}