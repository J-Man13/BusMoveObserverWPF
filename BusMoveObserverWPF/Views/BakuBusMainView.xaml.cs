using BusMoveObserverWPF.ViewModels;
using Microsoft.Maps.MapControl.WPF;
using System.Timers;
using System.Threading;
using System.Windows;
using System.Device.Location;
using System;

namespace BusMoveObserverWPF.Views
{
    /// <summary>
    /// Interaction logic for BakuBusMainView.xaml
    /// </summary>
    public partial class BakuBusMainView : Window
    {
        private BakuBusMainViewModel BakuBusMainViewModel;
        private System.Timers.Timer timer;

        public BakuBusMainView()
        {
            try {
                Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
                FindMe();
            }
            catch (Exception)
            {
                MessageBox.Show("Check your internet connection");
            }

            InitializeComponent();
            BakuBusMainViewModel = new BakuBusMainViewModel();
            DataContext = BakuBusMainViewModel;
            timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler((object source, ElapsedEventArgs e) =>
            {
                try
                {
                    GeoCoordinateWatcher watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
                    watcher.Start();

                    watcher.PositionChanged += (s, ev) =>
                    {
                        var coordinate = ev.Position.Location;
                        Dispatcher.Invoke(() => PushPinMe.Location = new Location(coordinate.Latitude, coordinate.Longitude));
                        watcher.Stop();
                    };
                }
                catch (Exception)
                {
                    MessageBox.Show("Check your internet connection");
                }
            });
            timer.Interval = 5000;
            timer.Start();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try {
                FindMe();
            }
            catch (Exception)
            {
                MessageBox.Show("Check your internet connection");
            }
        }

        private void FindMe()
        {
            GeoCoordinateWatcher watcher = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
            watcher.Start();

            watcher.PositionChanged += (s, ev) =>
            {
                var coordinate = ev.Position.Location;
                theMap.Center = new Location(coordinate.Latitude, coordinate.Longitude);
                theMap.ZoomLevel = 18;
                PushPinMe.Location = new Location(coordinate.Latitude, coordinate.Longitude);
                watcher.Stop();
            };
        }
    }
}
