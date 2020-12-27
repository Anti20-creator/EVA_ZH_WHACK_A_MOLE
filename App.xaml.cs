using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using ZH_Mole.ViewModel;

namespace ZH_Mole
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        Model _model;
        GameViewModel _viewModel;
        MainWindow _view;
        DispatcherTimer _timer;

        private void App_Startup(object sender, StartupEventArgs e)
        {
            _model = new Model();
            _viewModel = new GameViewModel(_model);

            _viewModel.StartGame += ViewModel_StartGame;

            _view = new MainWindow();
            _view.DataContext = _viewModel;
            _view.Show();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _model.AdvanceTime();
        }

        private void ViewModel_StartGame(object sender, EventArgs e)
        {
            _model.StartGame();
            _timer.Start();
        }

        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }
    }
}
