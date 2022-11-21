﻿using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using ep.Mobile.Data;
using ep.Mobile.Helpers;
using ep.Mobile.Interfaces.IAPIs;
using ep.Mobile.Interfaces.IRepos;
using ep.Mobile.Interfaces.IServices;
using ep.Mobile.PageModels.Base;
using ep.Mobile.Repos;
using ep.Mobile.Services;
using ep.Mobile.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ep.Mobile
{
    public partial class App : Application
    {
        //private static SQLiteHelper _sqlite;

        //public static SQLiteHelper SQLiteHelper 
        //{ 
        //    get 
        //    {
        //        if (_sqlite == null)
        //        {
        //            _sqlite = new SQLiteHelper();
        //        }
        //        return _sqlite;
        //    } 
        //}

        private static Database database;
        public static Database Database
        {
            get
            {
                if (database == null)
                {
                    var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ep.db3");
                    database = new Database(path);
                }
                return database;
            }
        }

        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NzYzOTk5QDMyMzAyZTMzMmUzMFZrVmdEcU1hWVNrNnJTb254MkMvNU5qUXFoTDMvdlpyOHptU2FydVFLMjQ9");

            InitializeComponent();
            DependencyService.Register<MockDataStore>();
            DependencyService.Register<IAPIService, APIService>();
            DependencyService.Register<ICustomerRepo, CustomerRepo>();
            DependencyService.Register<ICustomerService, CustomerService>();
            DependencyService.Register<IMessageRepo, MessageRepo>();            
            DependencyService.Register<IMessageService, MessageService>();
            DependencyService.Register<IShopRepo, ShopRepo>();
            DependencyService.Register<IShopService, ShopService>();
            DependencyService.Register<IPageService, PageService>();
            
            //var service = new SQLiteHelper();
            //DependencyService.RegisterSingleton<ISQLiteDb>(service);

            MainPage = new AppShell();
        }

        //Task InitNavigation()
        //{ 
        //    var navService = PageModelLocator.Resolve<
        //}

        protected override void OnStart()
        {
            //await InitNavigation();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
