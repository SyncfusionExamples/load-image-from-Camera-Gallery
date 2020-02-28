using IECameraandGallery.UWP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Xamarin.Forms;

[assembly: Dependency(typeof(ImageEditorService))]
namespace IECameraandGallery.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            LoadApplication(new IECameraandGallery.App());
        }

    }
    public class ImageEditorService : IImageEditorDependencyService
    {
        public ImageEditorService()
        {
        }

        async void IImageEditorDependencyService.UploadFromCamera(IECameraandGallery.MainPage editor)
        {
            CameraCaptureUI captureUI = new CameraCaptureUI();
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.JpegXR;
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Png;
            StorageFile photo = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);
            if (photo != null)
            {
                var stream = await photo.OpenAsync(FileAccessMode.Read);
                if(stream!=null)
                editor.SwitchView(stream.AsStream());
            }
        }

        async void IImageEditorDependencyService.UploadFromGallery(IECameraandGallery.MainPage editor)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".png");
            openPicker.FileTypeFilter.Add(".jpeg");
            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                var stream = await file.OpenAsync(FileAccessMode.Read);
                if(stream!=null)
                editor.SwitchView(stream.AsStream());
            }
        }
    }
}