using _1Pass.NetStandart.Libs.DBAPI;
using _1Pass.NetStandart.Libs.Entities;
using _1Pass.UI.XamarinUi.Pages;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace _1Pass.UI.XamarinUi.PartialPages
{
    public class AccountCell : ViewCell
    {
        Label idLabel,serviceLabel;
        Entry login, password;
        Button edit, delete, save, cancel;
        Grid grid;
        StackLayout stack2, stack3;


        AccountRepo repo;

        Account oldvalue;
        public AccountCell()
        {
            repo = Startup.ServiceProvider.GetService<AccountRepo>();
            idLabel = new Label() { IsVisible = false };
            serviceLabel = new Label() { IsVisible = false };

            login = new Entry()
            {
                IsEnabled = false,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Entry)),
            };
            password = new Entry()
            {
                IsEnabled = false,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Entry)),
            };

            edit = new Button()
            {
                BackgroundColor = Color.CadetBlue,
                Text = "Edit",
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button)),
            };
            edit.Clicked += edit_OnClick;

            save = new Button()
            {
                BackgroundColor = Color.Green,
                Text = "Save",
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button)),
            };
            save.Clicked += save_OnClick;

            delete = new Button()
            {
                BackgroundColor = Color.DarkRed,
                Text = "Delete",
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button)),
            };
            delete.Clicked += delete_OnClick;

            cancel = new Button()
            {
                BackgroundColor = Color.DarkRed,
                Text = "Cancel",
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button)),
            };
            cancel.Clicked+=cancel_OnClick;

            var cell = new StackLayout();

            grid = new Grid()
            {
                ColumnDefinitions =
                {
                new ColumnDefinition { Width = new GridLength(0.7, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(0.3, GridUnitType.Star) }
                }
            };
            var stack = new StackLayout() { Orientation= StackOrientation.Vertical };
            stack.Children.Add(login);
            stack.Children.Add(password);

            grid.Children.Add(stack, 0, 0);

            stack2 = new StackLayout() { Orientation = StackOrientation.Vertical};
            stack2.Children.Add(edit);
            stack2.Children.Add(delete);

            stack3 = new StackLayout() { Orientation = StackOrientation.Vertical};
            stack3.Children.Add(save);
            stack3.Children.Add(cancel);

            grid.Children.Add(stack2,1,0);

            cell.Children.Add(grid);
            cell.Children.Add(idLabel);
            cell.Children.Add(serviceLabel);
            
            View = cell;
        }

        public static readonly BindableProperty AccountIdProperty = BindableProperty.Create("AccountId", typeof(int), typeof(ServiceCell), 0);
        public static readonly BindableProperty AccountNameProperty = BindableProperty.Create("AccountName", typeof(string), typeof(ServiceCell), "");
        public static readonly BindableProperty AccountPasswordProperty = BindableProperty.Create("AccountPassword", typeof(string), typeof(ServiceCell), "");
        public static readonly BindableProperty AccountServiceProperty = BindableProperty.Create("AccountService", typeof(int), typeof(ServiceCell), 0);


        public int AccountId
        {
            get => (int)GetValue(AccountIdProperty);
            set { SetValue(AccountIdProperty, value); }
        }
        public int AccountService
        {
            get => (int)GetValue(AccountServiceProperty);
            set { SetValue(AccountServiceProperty, value); }
        }
        public string AccountName
        {
            get => (string)GetValue(AccountNameProperty);
            set
            {
                SetValue(AccountNameProperty, value);
            }
        }
        public string AccountPassword
        {
            get => (string)GetValue(AccountPasswordProperty);
            set
            {
                SetValue(AccountPasswordProperty, value);
            }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext != null)
            {
                login.Text = AccountName;
                password.Text = AccountPassword;

                serviceLabel.Text = AccountService.ToString();
                idLabel.Text = AccountId.ToString();
            }
        }

        private void edit_OnClick(object sender, EventArgs e)
        {
            oldvalue = (sender as Xamarin.Forms.Button).BindingContext as Account;
            login.IsEnabled = true;
            password.IsEnabled = true;
            grid.Children.RemoveAt(1);
            grid.Children.Add(stack3, 1, 0);
        } 

        private void cancel_OnClick(object sender, EventArgs e)
        {
            login.Text = oldvalue.Username;
            password.Text = oldvalue.Password;

            login.IsEnabled = false;
            password.IsEnabled = false;

            grid.Children.RemoveAt(1);
            grid.Children.Add(stack2, 1, 0);
        }

        private async void delete_OnClick(object sender, EventArgs e)
        {
            var value = (sender as Xamarin.Forms.Button).BindingContext as Account;
            
            var answer = await App.Current.MainPage.DisplayAlert($"Delete {value.Username}", $"Are you really want to delete {value.Username}?", "Yes", "No");
            if (answer)
            {
                var res = await repo.DeleteAccountAsync(value.Id);
                await (App.Current.MainPage.Navigation.ModalStack.Last() as AccountsPage).Refresh();
                                
            }
        }

        private async void save_OnClick(object sender, EventArgs e)
        {
            var acc = (sender as Xamarin.Forms.Button).BindingContext as Account;

            acc.Username = login.Text;
            acc.Password = password.Text;

            var res = await repo.UpdateAccountAsync(acc);

            var a = (App.Current.MainPage.Navigation.ModalStack.Last() as AccountsPage).accounts.FirstOrDefault(p => p.Id == res.Id);

            a.Username = res.Username;
            a.Password = res.Password;

            login.IsEnabled = false;
            password.IsEnabled = false;

            grid.Children.RemoveAt(1);
            grid.Children.Add(stack2, 1, 0);
        }
    }
}
