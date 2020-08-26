using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System.Runtime.InteropServices;

namespace SignaturePad
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        SignaturePadCanvasView _SignaturePadCanvasView;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            _SignaturePadCanvasView = FindViewById<SignaturePadCanvasView>(Resource.Id.canvasView);

            Button clearButton = FindViewById<Button>(Resource.Id.clearButton);
            clearButton.Click += (sender, e) =>
            {
                _SignaturePadCanvasView.ClearAll();
            };

            Button saveButton = FindViewById<Button>(Resource.Id.saveButton);
            saveButton.Click += (sender, e) =>
            {
                //Koordinat Bilgileri.(Coordinates Values)
                var Coordinate = _SignaturePadCanvasView.Coordinates;
            };

            Button cancelButton = FindViewById<Button>(Resource.Id.cancelButton);
            cancelButton.Click += (sender, e) =>
            {
                //Cancel Signature Codes.
                _SignaturePadCanvasView.ClearAll();
                System.Environment.Exit(0);
            };
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}