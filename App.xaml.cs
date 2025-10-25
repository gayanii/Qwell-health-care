using AutoMapper;
using Microsoft.EntityFrameworkCore.Infrastructure;
using QWellApp.DBConnection;
using QWellApp.Mappers;
using QWellApp.Views.Pages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace QWell
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IMapper Mapper { get; private set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //DatabaseFacade facade = new DatabaseFacade(new AppDataContext());
            //facade.EnsureCreated();
            // AutoMapper Configuration
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            Mapper = mapperConfig.CreateMapper();
        }

        protected  void ApplicationStart(object sender, StartupEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            loginWindow.IsVisibleChanged += (s, ev) =>
            {
                if (loginWindow.IsVisible == false && loginWindow.IsLoaded)
                {
                    var mainWindow = new MainWindow();
                    loginWindow.Close();
                }
            };
        }
    }
}
