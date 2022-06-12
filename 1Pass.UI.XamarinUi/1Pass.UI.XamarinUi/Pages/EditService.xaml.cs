using _1Pass.NetStandart.Libs.DBAPI;
using _1Pass.NetStandart.Libs.Entities;
using System;
using Microsoft.Extensions.DependencyInjection;
using Xamarin.Forms;
//using Xamarin.Forms.Xaml;

namespace _1Pass.UI.XamarinUi.Pages
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditService : ContentPage
    {
        Service oldService;
        ServiceRepo repo;
        public EditService(Service service)
        {
            InitializeComponent();
            oldService = service;
            labl.Text += $" \"{service.Name}\"";
            serviceNew.Text = service.Name;
            repo = Startup.ServiceProvider.GetService<ServiceRepo>();
        }

        private async void saveBtn_Clicked(object sender, EventArgs e)
        {
            var changed = serviceNew.Text;
            if (changed != oldService.Name)
            {
                var serv = new Service() { Id = oldService.Id, Name = changed };
                var res = await repo.UpdateServiceAsync(serv);
                if(res != null)
                {
                    var index = (App.Current.MainPage as ServicesPage).Services.IndexOf(oldService);
                    (App.Current.MainPage as ServicesPage).Services[index] = res;
                    await (App.Current.MainPage as ServicesPage).ListView_Refreshing(null, null);
                    await App.Current.MainPage.Navigation.PopModalAsync();
                }
            }
        }

        private async void cancelBtn_Clicked(object sender, EventArgs e)
        {
            await App.Current.MainPage.Navigation.PopModalAsync();
        }
    }
}