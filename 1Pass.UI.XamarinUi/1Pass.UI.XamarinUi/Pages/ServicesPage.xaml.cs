using _1Pass.NetStandart.Libs.DBAPI;
using _1Pass.NetStandart.Libs.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace _1Pass.UI.XamarinUi.Pages
{
    public partial class ServicesPage : ContentPage
    {
        public ServiceRepo Repo { get; set; }
        public ObservableCollection<Service> Services { get; set; }
        public ServicesPage()
        {
            InitializeComponent();
            Repo = Startup.ServiceProvider.GetService<ServiceRepo>();
   
            var services = Repo.GetServicesAsync().Result;
            Services = new ObservableCollection<Service>(services);

            listview.ItemsSource = Services;
            this.BindingContext = this;
        }
        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            var service = e.Item as Service;
            var res = await Repo.GetServiceWithAccounts(service.Id);
            res.Id = service.Id;
            res.Name = service.Name;
            var page = new AccountsPage(res);

            await App.Current.MainPage.Navigation.PushModalAsync(page);
            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
        private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            listview.ItemsSource = await SearchRequest(e.NewTextValue);
        }
        public async Task ListView_Refreshing(object sender, EventArgs e)
        {
            var services = await Repo.GetServicesAsync();
            var res = new ObservableCollection<Service>(services);

            Services = res;
            listview.ItemsSource = Services;
            listview.EndRefresh();
        }
        private async Task<ObservableCollection<Service>> SearchRequest(string searchText = null)
        {
            var result = new ObservableCollection<Service>();
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                foreach (var service in Services.Where(p => p.Name.ToLowerInvariant().Contains(searchText)))
                {
                    result.Add(service);
                }
                return result;
            }
            else
            {
                return Services;
            }   
        }
        private async void AddBtn_Clicked(object sender, EventArgs e)
        {
            var name = newService.Text;
            if (!string.IsNullOrWhiteSpace(name))
            {
                var service = new Service() { Name = name };
                int id = 0;
                try
                {
                    id = await Repo.CreateServiceAsync(service);
                }
                catch (Exception)
                {

                }
                if (id>0)
                {
                    service.Id = id;
                    Services.Add(service);
                    newService.Text = null;
                }                
            }
        }
    }
}
