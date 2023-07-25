using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;
using Android.Widget;
using Android.Content;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;
using AlertDialog = Android.App.AlertDialog;

namespace IT391U10
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private string translatedNumber = string.Empty;
        private EditText phoneNumberText;
        private TextView translatedPhoneWord;
        private Button translateButton, callButton, exitButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            phoneNumberText = FindViewById<EditText>(Resource.Id.PhoneNumberText);
            translatedPhoneWord = FindViewById<TextView>(Resource.Id.TranslatedPhoneword);
            translateButton = FindViewById<Button>(Resource.Id.TranslateButton);
            callButton = FindViewById<Button>(Resource.Id.btnCall);
            exitButton = FindViewById<Button>(Resource.Id.ExitBut);
            

            callButton.Enabled = false;
            translateButton.Click += TranslateButtonClick;
            callButton.Click += CallButton_Click;
            exitButton.Click += ExitApplicationButton;

        }

        private void ExitApplicationButton(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }
        private void TranslateButtonClick (object sender, EventArgs e)
        {
            translatedNumber = Core.PhonewordTranslator.ToNumber(phoneNumberText.Text);
            if (string.IsNullOrWhiteSpace(translatedNumber))
            {
                translatedPhoneWord.Text = string.Empty;
                callButton.Text = "Call";
                callButton.Enabled = false;
            }
            else
            {
                translatedPhoneWord.Text = translatedNumber;
                callButton.Text = "Call" + translatedNumber;
                callButton.Enabled = true;
            }
        }

        private void CallButton_Click(object sender, System.EventArgs e)
        {
            var callDialog = new AlertDialog.Builder(this);
            callDialog.SetMessage("Call" + translatedNumber + "?");
            callDialog.SetNeutralButton("Call", delegate
            {
                var callIntent = new Intent(Intent.ActionCall);
                callIntent.SetData(Android.Net.Uri.Parse("tel:" + translatedNumber));
                StartActivity(callIntent);
            });

            callDialog.SetNegativeButton("Cancel", delegate { });
            callDialog.Show();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View) sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (View.IOnClickListener)null).Show();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
	}
}
