using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IECameraAndGallery
{
    public partial class MainPage : ContentPage
    {
        ImageModel model;
        public MainPage()
        {
            model = new ImageModel();
            BindingContext = model;
            InitializeComponent();
            MessagingCenter.Subscribe<byte[]>(this, "ImageSelected", (args) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    var source = ImageSource.FromStream(() => new MemoryStream(args));
                    SwitchView(source);
                });

            });
        }

        private async void SwitchView(ImageSource source)
        {
          await Navigation.PushAsync(new SfImageEditorPage() { ImageSource = source });
        }

        void OpenGalleryTapped(object sender, System.EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var fileName = SetImageFileName();
                DependencyService.Get<CameraInterface>().LaunchGallery(FileFormatEnum.JPEG, fileName);
            });
        }

        void ImageTapped(object sender, System.EventArgs e)
        {
            LoadFromStream((sender as Image).Source);
        }

       private async void LoadFromStream(ImageSource source)
        {
            await Navigation.PushAsync(new SfImageEditorPage() { ImageSource = source });
        }
        void TakeAPhotoTapped(object sender, System.EventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                var fileName = SetImageFileName();
                DependencyService.Get<CameraInterface>().LaunchCamera(FileFormatEnum.JPEG, fileName);
            });
        }

        private string SetImageFileName(string fileName = null)
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                if (fileName != null)
                    App.ImageIdToSave = fileName;
                else
                    App.ImageIdToSave = App.DefaultImageId;

                return App.ImageIdToSave;
            }
            else
            {
                if (fileName != null)
                {
                    App.ImageIdToSave = fileName;
                    return fileName;
                }
                else
                    return null;
            }
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
            ChooseImage = ImageSource.FromResource("IECameraAndGallery.Icons.Gallery_S.png", typeof(App).GetTypeInfo().Assembly);//GallerySelected
            TakePic = ImageSource.FromResource("IECameraAndGallery.Icons.Take_Photo_W.png", typeof(App).GetTypeInfo().Assembly);
            ChooseImageSelected = ImageSource.FromResource("IECameraAndGallery.Icons.Gallery_W.png", typeof(App).GetTypeInfo().Assembly);
            TakePicSelected = ImageSource.FromResource("IECameraAndGallery.Icons.Take_Photo_S.png", typeof(App).GetTypeInfo().Assembly);
            BroweImage1 = ImageSource.FromResource("IECameraAndGallery.Icons.image2.png", typeof(App).GetTypeInfo().Assembly);
            BroweImage2 = ImageSource.FromResource("IECameraAndGallery.Icons.image3.png", typeof(App).GetTypeInfo().Assembly);
            BroweImage3 = ImageSource.FromResource("IECameraAndGallery.Icons.image4.png", typeof(App).GetTypeInfo().Assembly);
        }
    }
}
