using _1Pass.NetStandart.Libs.DBAPI;
using _1Pass.NetStandart.Libs.Encryption;
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
    public partial class Register : ContentPage
    {
        private IDatabase _db;

        public Register(IDatabase db)
        {
            _db = db;
            InitializeComponent();
        }

        private void Password_TextChanged(object sender, TextChangedEventArgs e)
        {
            var passNew = e.NewTextValue ?? String.Empty;
            passNew = passNew.Trim();
            var passOld = e.OldTextValue ?? String.Empty;
            if (passNew.Length >= 4 && passNew != passOld)
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
        
        private void Submit_OnClick(object sender, EventArgs args)
        {
            var pass = Password.Text ?? String.Empty;
            pass = pass.Trim();

            _db.Password = pass;
            var res = _db.CreateDatabase();
            if (res>0)
            {
                Error.Text = "Database Created";
                Error.TextColor = Color.LightGreen;
                Error.IsVisible = true;
            }
        }
        private void GeneratePassword_OnClick(object sender, EventArgs args)
        {
            var isEnabled = UseSymbols.IsChecked;
            var pass = PasswordGenerator.Generate(12,isEnabled);

            Password.Text = pass;

            GeneratedPassLabel.Text = $"Generated pass : {pass}\nPlease remember it";
            GeneratedPassLabel.IsVisible = true;
        }
    }
}