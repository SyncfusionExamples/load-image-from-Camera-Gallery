using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;
using Android.Provider;
using Android.Support.V4.Content;
using Xamarin.Forms;

[assembly: Dependency(typeof(IECameraAndGallery.Droid.CameraAndroid))]
namespace IECameraAndGallery.Droid
{
    public class CameraAndroid : CameraInterface
    {
        public Java.IO.File cameraFile;
        Java.IO.File dirFile;

        public void LaunchCamera(FileFormatEnum fileType, string imageId)
        {

            var intent = new Intent(MediaStore.ActionImageCapture);

            Intent takePictureIntent = new Intent(MediaStore.ActionImageCapture);
            // Ensure that there is a camera activity to handle the intent
            if (takePictureIntent.ResolveActivity(MainActivity.ActivityContext.PackageManager) != null)
            {
                try
                {
                    var documentsDirectry = MainActivity.ActivityContext.GetExternalFilesDir(Android.OS.Environment.DirectoryPictures);
                    cameraFile = new Java.IO.File(documentsDirectry, imageId + "." + fileType.ToString());

                    if (cameraFile != null)
                    {
                        using (var mediaStorageDir = new Java.IO.File(documentsDirectry, string.Empty))
                        {
                            if (!mediaStorageDir.Exists())
                            {
                                if (!mediaStorageDir.Mkdirs())
                                    throw new IOException("Couldn't create directory, have you added the WRITE_EXTERNAL_STORAGE permission?");
                            }
                        }
                    }
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex);
                }

                //NOTE: Make sure the authority matches what you put in the AndroidManifest.xml file
                Android.Net.Uri photoURI = FileProvider.GetUriForFile(((Activity)MainActivity.ActivityContext), "com.yourcompany.IECameraAndGallery.fileprovider", cameraFile);
                takePictureIntent.PutExtra(MediaStore.ExtraOutput, photoURI);
                ((Activity)MainActivity.ActivityContext).StartActivityForResult(takePictureIntent, 0);
            }
        }

        public void LaunchGallery(FileFormatEnum fileType, string imageId)
        {
            var imageIntent = new Intent();
            imageIntent.SetType("image/*");

            if (imageId != null)
            {
                imageIntent.PutExtra("fileName", imageId + "." + fileType.ToString());
            }
            imageIntent.SetAction(Intent.ActionGetContent);

            ((Activity)MainActivity.ActivityContext).StartActivityForResult(Intent.CreateChooser(imageIntent, "Select photo"), 1);
        }
    }
}