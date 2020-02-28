using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IECameraandGallery
{
    public partial class MainPage : ContentPage
    {
        bool isTakePhoto = false, isOpenGallery = false;
        ImageModel model;
        public MainPage()
        {
            model = new ImageModel();
            BindingContext = model;
            InitializeComponent();
        }

        public void SwitchView(Stream file)
        {
            Navigation.PushModalAsync(new SfImageEditorPage() { Stream = file });
        }

        public void SwitchView(string file)
        {
            Navigation.PushModalAsync(new SfImageEditorPage() { FileName = file });
        }

        void OpenGalleryTapped(object sender, System.EventArgs e)
        {
            if (!isOpenGallery)
            {
                OpenGallery.Source = model.ChooseImage;
                isOpenGallery = true;
            }
            else
            {
                OpenGallery.Source = model.ChooseImageSelected;
                isOpenGallery = false;
            }
            if (Device.OS == TargetPlatform.Android || Device.OS == TargetPlatform.iOS || Device.OS==TargetPlatform.Windows)
                DependencyService.Get<IImageEditorDependencyService>().UploadFromGallery(this);
        }

        void ImageTapped(object sender, System.EventArgs e)
        {
            LoadFromStream((sender as Image).Source);
        }

        void LoadFromStream(ImageSource source)
        {
            if (Device.OS == TargetPlatform.iOS)
            {
                Navigation.PushModalAsync((new SfImageEditorPage() { ImageSource = source }));
            }
            if (Device.OS == TargetPlatform.Windows || Device.OS == TargetPlatform.WinPhone)
            {
                Navigation.PushModalAsync((new SfImageEditorPage() { ImageSource = source }));
            }
            else
            {
                Navigation.PushModalAsync((new SfImageEditorPage() { ImageSource = source }));
            }
        }
        void TakeAPhotoTapped(object sender, System.EventArgs e)
        {
            if (!isTakePhoto)
            {
                TakePhoto.Source = model.TakePicSelected;
                isTakePhoto = true;
            }
            else
            {
                TakePhoto.Source = model.TakePic;
                isTakePhoto = false;
            }
            if (Device.OS == TargetPlatform.Android ||Device.OS==TargetPlatform.iOS || Device.OS==TargetPlatform.Windows)
                DependencyService.Get<IImageEditorDependencyService>().UploadFromCamera(this);
        }
    }

    class ImageModel
    {
        public ImageSource TakePic { get; set; }
        public ImageSource TakePicSelected { get; set; }
        public ImageSource ChooseImage { get; set; }
        public ImageSource ChooseImageSelected { get; set; }
        public ImageSource BroweImage1 { get; set; }
        public ImageSource BroweImage2 { get; set; }
        public ImageSource BroweImage3 { get; set; }

        public ImageModel()
        {
            ChooseImage = ImageSource.FromResource("IECameraandGallery.Icons.Gallery_S.png", typeof(App).GetTypeInfo().Assembly);//GallerySelected
            TakePic = ImageSource.FromResource("IECameraandGallery.Icons.Take_Photo_W.png", typeof(App).GetTypeInfo().Assembly);
            ChooseImageSelected = ImageSource.FromResource("IECameraandGallery.Icons.Gallery_W.png", typeof(App).GetTypeInfo().Assembly);
            TakePicSelected = ImageSource.FromResource("IECameraandGallery.Icons.Take_Photo_S.png", typeof(App).GetTypeInfo().Assembly);
            BroweImage1 = ImageSource.FromResource("IECameraandGallery.Icons.image2.png", typeof(App).GetTypeInfo().Assembly);
            BroweImage2 = ImageSource.FromResource("IECameraandGallery.Icons.image3.png", typeof(App).GetTypeInfo().Assembly);
            BroweImage3 = ImageSource.FromResource("IECameraandGallery.Icons.image4.png", typeof(App).GetTypeInfo().Assembly);
        }
    }
}