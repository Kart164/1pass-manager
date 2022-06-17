using _1Pass.NetStandart.Libs.DBAPI;
using _1Pass.NetStandart.Libs.Encryption;
using _1Pass.NetStandart.Libs.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace _1Pass.UI.XamarinUi.Pages
{
    public partial class AccountsPage : ContentPage
    {
        public ServiceWithAccounts _service { get; set; }
        public ObservableCollection<Account> accounts { get; set; }
        AccountRepo repo { get; set; }
        public AccountsPage(ServiceWithAccounts service)
        { 
            InitializeComponent();
            repo = Startup.ServiceProvider.GetService<AccountRepo>();
            _service = service;
            lbl.Text += service.Name;
            accounts = new ObservableCollection<Account>(_service.Accounts);
            listview.ItemsSource = accounts;
            this.BindingContext = this;
        }

        public async Task Refresh()
        {
            accounts = new ObservableCollection<Account>((await Startup.ServiceProvider.GetService<ServiceRepo>().GetServiceWithAccounts(_service.Id)).Accounts);
            listview.ItemsSource = accounts;
        }

        private void genPass_Clicked(object sender, EventArgs e)
        {
            var password = PasswordGenerator.Generate(12, true);
            newPass.Text = password;
        }

        private async void addAcc_Clicked(object sender, EventArgs e)
        {
            var acc = new Account()
            {
                Username = newLogin.Text,
                Password = newPass.Text,
                ServiceId = _service.Id
            };            
            var res = await repo.CreateAccountAsync(acc);
            acc.Id = res;
            accounts.Add(acc);
            _service.Accounts.Add(acc);
        }

        //private async void listview_ItemTapped(object sender, ItemTappedEventArgs e)
        //{
        //    if (e.Item == null)
        //        return;

        //    await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

        //    //Deselect Item
        //    ((ListView)sender).SelectedItem = null;
        //}

        private async void goBack_Clicked(object sender, EventArgs e)
        {
            await App.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}