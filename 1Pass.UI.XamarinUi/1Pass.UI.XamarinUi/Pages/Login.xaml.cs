using _1Pass.NetStandart.Libs.DBAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace _1Pass.UI.XamarinUi.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        private IDatabase _db;
        private int CountOfLogins;

        public Login(IDatabase db)
        {
            _db = db;
            CountOfLogins = 0;
            InitializeComponent();
        }

        private void Password_TextChanged(object sender, TextChangedEventArgs e)
        {
            var passNew = e.NewTextValue ?? String.Empty;
            var passOld = e.OldTextValue ?? String.Empty;
            if(passNew.Length>=4 && passNew != passOld)
            {
                Error.IsVisible = false;
                Submit.IsEnabled = true;
            }
            else
            {
                Error.Text = "Password length cant be less then 4 non white space symbols";
                Error.TextColor = Color.Red;
                Error.IsVisible = true;
                Submit.IsEnabled = false;
            }
        }


        private async void Submit_OnClick (object sender, EventArgs args)
        {

            var pass = Password.Text ?? String.Empty;
            pass = pass.Trim();
            if (pass.Length >= 4)
            {
                if (CountOfLogins >= 4)
                {

                    //await _db.DeleteDatabase();
                    Error.Text = "Database deleted";
                    Error.TextColor = Color.Red;
                    Error.IsVisible = true;

                }
                if (await _db.TryLogin(pass))
                {
                    App.Current.MainPage = new ServicesPage(Startup.ServiceProvider.GetService<ServiceRepo>());
                }
                else
                {
                    CountOfLogins++;
                    Error.Text = "Incorrect Pass";
#if DEBUG
                    Error.Text += $"\nCount of attemps: {CountOfLogins}";
#endif
                    Error.TextColor = Color.Red;
                    Error.IsVisible = true;

                }
            }
            else
            {
                Error.Text = "Password length cant be less then 4 non white space symbols";
                Error.TextColor = Color.Red;
                Error.IsVisible = true;
            }          
        }


    }
}