using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Microsoft.Extensions.Configuration;
using Android.Content;
using System.IO;
using _1Pass.NetStandart.Libs.Entities;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace _1Pass.UI.XamarinUi.Droid
{
    [Activity(Label = "_1Pass.UI.XamarinUi", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            var conf = GetConfig().Result;
            LoadApplication(new App(conf));
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private async Task<Config> GetConfig()
        {
            string configstr;
            var _context = Application.Context;
            using (var asset = _context.Assets.Open("config.json"))
            using (var streamReader = new StreamReader(asset))
            {
                configstr = await streamReader.ReadToEndAsync();
            }
            return JsonConvert.DeserializeObject<Config>(configstr);
        }
    }
}