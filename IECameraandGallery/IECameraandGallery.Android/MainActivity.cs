using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using IECameraandGallery.Droid;
using Android.Content;
using Android.Provider;
using Java.IO;
using System.Threading.Tasks;


[assembly: Dependency(typeof(ImageEditorService))]
namespace IECameraandGallery.Droid
{
    [Activity(Label = "IECameraandGallery", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }
        public event EventHandler<ActivityResultEventArgs> ActivityResult;

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (resultCode == Result.Ok)
            {
                ActivityResult?.Invoke(this, new ActivityResultEventArgs { Intent = data });
            }
        }
    }

    public class ActivityResultEventArgs : EventArgs
    {
        public Intent Intent
        {
            get;
            set;
        }
    }

    public class ImageEditorService : IImageEditorDependencyService
    {
        private static int SELECT_FROM_GALLERY = 0;
        private static int SELECT_FROM_CAMERA = 1;
        static Intent mainIntent;
        private Android.Net.Uri mImageCaptureUri;
        MainActivity activity;
        bool isCamera = false;
        private MainPage page;

        public void UploadFromCamera(MainPage editor)
        {
            isCamera = true;
            page = editor;
            activity = Xamarin.Forms.Forms.Context as MainActivity;
            activity.ActivityResult -= LoadCamera;
            activity.ActivityResult += LoadCamera;
            var intent = new Intent(MediaStore.ActionImageCapture);
            mImageCaptureUri = Android.Net.Uri.FromFile(new File(CreateDirectoryForPictures(),
                string.Format("ImageEditor_Photo_{0}.jpg", DateTime.Now.ToString("yyyyMMddHHmmssfff"))));
            intent.PutExtra(MediaStore.ExtraOutput, mImageCaptureUri);
            Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
            mediaScanIntent.SetData(mImageCaptureUri);
            activity.SendBroadcast(mediaScanIntent);
            try
            {
                mainIntent = intent;
                intent.PutExtra("return-data", false);
                activity.StartActivityForResult(mainIntent, SELECT_FROM_CAMERA);
            }
            catch (ActivityNotFoundException e)
            {
                Toast.MakeText(activity, "Unable to Load Image", ToastLength.Short);
            }
        }

        void LoadImage(object sender, ActivityResultEventArgs e)
        {
            if (!isCamera)
            {
                var imagePath = GetPathToImage(e.Intent.Data);
                page.SwitchView(imagePath);
            }
            else
            {
                mainIntent.PutExtra("image-path", mImageCaptureUri.Path);
                mainIntent.PutExtra("scale", true);
                page.SwitchView(mImageCaptureUri.Path);
            }
        }

        void LoadCamera(object sender, ActivityResultEventArgs e)
        {
            if (!isCamera)
            {
                var imagePath = GetPathToImage(e.Intent.Data);
                page.SwitchView(imagePath);
            }
            else
            {
                mainIntent.PutExtra("image-path", mImageCaptureUri.Path);
                mainIntent.PutExtra("scale", true);
                page.SwitchView(mImageCaptureUri.Path);
            }
        }

        public void UploadFromGallery(MainPage editor)
        {
            isCamera = false;
            page = editor;
            activity = Xamarin.Forms.Forms.Context as MainActivity;
            activity.ActivityResult -= LoadImage;
            activity.ActivityResult += LoadImage;
            activity.Intent = new Intent();
            activity.Intent.SetType("image/*");
            activity.Intent.SetAction(Intent.ActionGetContent);
            activity.StartActivityForResult(Intent.CreateChooser(activity.Intent, "Select Picture"), SELECT_FROM_GALLERY);
        }

        private string GetPathToImage(Android.Net.Uri uri)
        {
            string imgId = "";
            string[] proj = { MediaStore.Images.Media.InterfaceConsts.Data };
            using (var c1 = activity.ContentResolver.Query(uri, null, null, null, null))
            {
                try
                {
                    if (c1 == null) return "";
                    c1.MoveToFirst();
                    string imageId = c1.GetString(0);
                    imgId = imageId.Substring(imageId.LastIndexOf(":") + 1);
                }
                catch (System.Exception e)
                {
                    Toast.MakeText(Xamarin.Forms.Forms.Context, "Unable To Load Image", ToastLength.Short);
                }
            }

            string path = null;

            string selection = MediaStore.Images.Media.InterfaceConsts.Id + " =? ";
            using (var cursor = activity.ContentResolver.Query(MediaStore.Images.Media.ExternalContentUri, null, selection, new string[] { imgId }, null))
            {
                try
                {
                    if (cursor == null) return path;
                    var columnIndex = cursor.GetColumnIndexOrThrow(MediaStore.Images.Media.InterfaceConsts.Data);
                    cursor.MoveToFirst();
                    path = cursor.GetString(columnIndex);
                }
                catch (System.Exception e)
                {
                    Toast.MakeText(Xamarin.Forms.Forms.Context, "Unable To Load Image", ToastLength.Short);
                    return "";
                }
            }
            return path;

        }

        string GetPathFromFile(Android.Net.Uri contentUri)
        {
            string res = null;
            string[] proj = { MediaStore.Images.Media.InterfaceConsts.Data };
            var cursor = activity.ContentResolver.Query(contentUri, null, null, null, null);
            if (cursor == null) return contentUri.ToString();
            if (cursor.MoveToFirst())
            {
                int column_index = cursor.GetColumnIndexOrThrow(MediaStore.Images.Media.InterfaceConsts.Data);
                res = cursor.GetString(column_index);
            }
            cursor.Close();
            return res;
        }

        private File CreateDirectoryForPictures()
        {
            var dir = new File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures), "ImageEditor");
            if (!dir.Exists())
            {
                dir.Mkdirs();
            }

            return dir;
        }

    }
}