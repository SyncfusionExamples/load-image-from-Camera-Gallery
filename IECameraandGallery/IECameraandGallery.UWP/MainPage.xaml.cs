using IECameraAndGallery.UWP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Xamarin.Forms;

[assembly: Dependency(typeof(ImageEditorService))]
namespace IECameraAndGallery.UWP
{

    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            LoadApplication(new IECameraAndGallery.App());
        }
    }

    public class ImageEditorService : CameraInterface
    {
        public ImageEditorService()
        {
        }

        public async void LaunchCamera(FileFormatEnum imageType, string imageId = null)
        {
            CameraCaptureUI captureUI = new CameraCaptureUI();
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.JpegXR;
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Png;
            StorageFile photo = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);
            if (photo != null)
            {
                var stream = await photo.OpenAsync(FileAccessMode.Read);

                var reader = new DataReader(stream.GetInputStreamAt(0));
                var bytes = new byte[stream.Size];
                await reader.LoadAsync((uint)stream.Size);
                reader.ReadBytes(bytes);
                MessagingCenter.Send<byte[]>(bytes, "ImageSelected");
            }
        }


        private byte[] GetImageStreamAsBytes(Stream input)
        {
            var buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }


        public async void LaunchGallery(FileFormatEnum imageType, string imageId = null)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".png");
            openPicker.FileTypeFilter.Add(".jpeg");
            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                var stream = await file.OpenAsync(FileAccessMode.Read);
                var reader = new DataReader(stream.GetInputStreamAt(0));
                var bytes = new byte[stream.Size];
                await reader.LoadAsync((uint)stream.Size);
                reader.ReadBytes(bytes);
                MessagingCenter.Send<byte[]>(bytes, "ImageSelected");
            }
        }
    }
}
