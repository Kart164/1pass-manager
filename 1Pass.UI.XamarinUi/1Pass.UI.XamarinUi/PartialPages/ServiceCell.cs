using _1Pass.NetStandart.Libs.DBAPI;
using _1Pass.NetStandart.Libs.Entities;
using _1Pass.UI.XamarinUi.Pages;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xamarin.Forms;

namespace _1Pass.UI.XamarinUi.PartialPages
{
    public class ServiceCell : ViewCell
    {
        Label nameLabel, idLabel;
        Button editButton, deleteButton;
        ServiceRepo repo;
        public ServiceCell()
        {
            repo = Startup.ServiceProvider.GetService<ServiceRepo>();

            nameLabel = new Label() { FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)), HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center };
            idLabel = new Label() { IsVisible = false };
            editButton = new Button() { Text = "Edit", BackgroundColor = Color.DarkGreen};
            deleteButton = new Button() { Text = "Delete", BackgroundColor = Color.DarkRed};

            editButton.Clicked += Edit_OnClick;
            deleteButton.Clicked += Delete_OnClick;

            var cell = new StackLayout();
            var grid = new Grid()
            {
                ColumnDefinitions =
                {
                new ColumnDefinition { Width = new GridLength(0.7, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(0.15, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(0.15, GridUnitType.Star) }
                }
            };
            grid.Children.Add(nameLabel,0,0);
            grid.Children.Add(editButton,1,0);
            grid.Children.Add(deleteButton,2,0);

            cell.Children.Add(idLabel);
            cell.Children.Add(grid);
            View = cell;
        }

        public static readonly BindableProperty ServiceIdProperty = BindableProperty.Create("ServiceId", typeof(int), typeof(ServiceCell), 0);
        public static readonly BindableProperty ServiceNameProperty = BindableProperty.Create("ServiceName", typeof(string), typeof(ServiceCell), "");
        public int ServiceId
        {
            get => (int)GetValue(ServiceIdProperty);
            set { SetValue(ServiceIdProperty, value); }
        }
        public string ServiceName
        {
            get => (string)GetValue(ServiceNameProperty);
            set
            {
                SetValue(ServiceNameProperty, value);
            }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext != null)
            {
                nameLabel.Text = ServiceName;
                idLabel.Text = ServiceId.ToString();
            }
        }

        public async void Edit_OnClick (object sender, EventArgs e)
        {
            var serv = (sender as Xamarin.Forms.Button).BindingContext as Service;
            var page = new EditService(serv);
            await App.Current.MainPage.Navigation.PushModalAsync(page);

        }

        public async void Delete_OnClick(object sender, EventArgs e)
        {
            var serv = (sender as Xamarin.Forms.Button).BindingContext as Service;
            var answer = await App.Current.MainPage.DisplayAlert($"Delete {serv.Name}", $"Are you really want to delete {serv.Name}? Account from this service you can find in No service", "Yes", "No");
            if (answer)
            {
                await repo.DeleteServicesAsync(serv.Id);
                await (App.Current.MainPage as ServicesPage).ListView_Refreshing(null, null);
            }
        }
    }
}
