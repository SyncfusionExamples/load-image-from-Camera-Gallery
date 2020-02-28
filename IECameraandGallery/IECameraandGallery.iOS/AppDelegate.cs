using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using IECameraandGallery.iOS;
using Syncfusion.SfImageEditor.XForms.iOS;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(ImageEditorService))]
namespace IECameraandGallery.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());
            SfImageEditorRenderer.Init();
            return base.FinishedLaunching(app, options);
        }
    }

    public class ImageEditorService : IImageEditorDependencyService
    {
        UIImagePickerController imagePicker;

        void IImageEditorDependencyService.UploadFromCamera(MainPage editor)
        {
            imagePicker = new UIImagePickerController();
            imagePicker.SourceType = UIImagePickerControllerSourceType.Camera;
            imagePicker.MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.Camera);
            imagePicker.FinishedPickingMedia += (object sender, UIImagePickerMediaPickedEventArgs e) =>
            {
                imagePicker.DismissModalViewController(true);
                editor.SwitchView(e.OriginalImage.AsPNG().AsStream());
            };
            imagePicker.Canceled += (sender, evt) =>
            {
                imagePicker.DismissModalViewController(true);
            };
            var topController = UIApplication.SharedApplication.KeyWindow.RootViewController;
            while (topController.PresentedViewController != null)
            {
                topController = topController.PresentedViewController;
            }
            topController.PresentViewController(imagePicker, true, null);
        }

        void IImageEditorDependencyService.UploadFromGallery(MainPage editor)
        {
            imagePicker = new UIImagePickerController();
            imagePicker.AllowsEditing = true;
            imagePicker.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;
            imagePicker.MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary);
            imagePicker.FinishedPickingMedia += (object sender, UIImagePickerMediaPickedEventArgs e) =>
            {
                imagePicker.DismissModalViewController(true);
                var image = e.OriginalImage ?? e.EditedImage;
                editor.SwitchView(image.AsPNG().AsStream());
            };
            imagePicker.Canceled += (sender, evt) =>
            {
                imagePicker.DismissModalViewController(true);
            };
            var topController = UIApplication.SharedApplication.KeyWindow.RootViewController;
            while (topController.PresentedViewController != null)
            {
                topController = topController.PresentedViewController;
            }
            topController.PresentViewController(imagePicker, true, null);
        }
    }
}