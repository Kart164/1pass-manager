using _1Pass.NetStandart.Libs.DBAPI;
using _1Pass.NetStandart.Libs.Entities;
using _1Pass.UI.XamarinUi.Pages;
using Microsoft.Extensions.Configuration;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace _1Pass.UI.XamarinUi
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            var db = Startup.ServiceProvider.GetService<IDatabase>();
            if (!db.CheckExistence)
            {
                MainPage = new Register(db);
            }
            else
            {
                MainPage = new Login(db);
            }
            

        }
        public App(Config config)
        {
            InitializeComponent();
            Startup.Init(config);
            var db = Startup.ServiceProvider.GetService<IDatabase>();
            if (!db.CheckExistence)
            {
                MainPage = new Register(db);
            }
            else
            {
                MainPage = new Login(db);
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
