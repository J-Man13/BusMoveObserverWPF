using BusMoveObserverWPF.Models;
using BusMoveObserverWPF.Services;
using GalaSoft.MvvmLight;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Timers;
using System.Device.Location;
using GalaSoft.MvvmLight.Command;

namespace BusMoveObserverWPF.ViewModels
{
    public class BakuBusMainViewModel : ViewModelBase
    {
        public ObservableCollection<BakuBusLocation> ObservableBakuBusesLocations
        {
            get { return observableBakuBusesLocations; }
            set { Set(ref observableBakuBusesLocations, value); }
        }
        private ObservableCollection<BakuBusLocation> observableBakuBusesLocations;

        public string DesiredRouteCode
        {
            get { return desiredRouteCode; }
            set { Set(ref desiredRouteCode, value); }

        }
        private string desiredRouteCode;

        public Location CenteringLocation
        {
            get { return centeringLocation; }
            set { Set(ref centeringLocation, value); }
        }
        private Location centeringLocation;

        private IBakuBusDataApi bakuBusDataApi;
        private Timer timer;

        public RelayCommand FilterByRouteCode
        {
            get { return filterByRouteCode; }
            set { Set(ref filterByRouteCode, value); }
        }
        private RelayCommand filterByRouteCode;

        public BakuBusMainViewModel()
        {
            DesiredRouteCode = "Input bus route code as filter";
            ObservableBakuBusesLocations = new ObservableCollection<BakuBusLocation>();
            CenteringLocation = new Location(40.391646, 49.858368);

            bakuBusDataApi = new BakuBusDataApiJson();

            try
            {
                List<BakuBusModel> list;
                try
                {
                    list = new List<BakuBusModel>(bakuBusDataApi.GetBakuBusModels());
                }
                catch (Exception)
                {
                    MessageBox.Show("Check your internet connection");
                    list = null;
                }
                InitBusesLocations(list);
            }
            catch (Exception)
            {
                MessageBox.Show("Check your internet connection");
            }


            timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler((object source, ElapsedEventArgs e) =>
            {
                List<BakuBusModel> list;
                try
                {
                     list = new List<BakuBusModel>(bakuBusDataApi.GetBakuBusModels());
                }
                catch (Exception)
                {
                    MessageBox.Show("Check your internet connection");
                    list = null;
                }

                Application.Current.Dispatcher.Invoke(() =>
                {
                    try
                    {
                        if (DesiredRouteCode.Equals("Input bus route code as filter"))
                            InitBusesLocations(list);
                        else
                            InitBusesLocations(list, DesiredRouteCode);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Check your internet connection");
                    }
                });
            });
            timer.Interval = 10000;
            timer.Start();

            FilterByRouteCode = new RelayCommand(
                () => 
                    {
                        List<BakuBusModel> list;
                        try
                        {
                            list = new List<BakuBusModel>(bakuBusDataApi.GetBakuBusModels());
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Check your internet connection");
                            list = null;
                        }

                        try
                        {
                            if (DesiredRouteCode.Equals("Input bus route code as filter"))
                                InitBusesLocations(list);
                            else
                                InitBusesLocations(list,DesiredRouteCode);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Check your internet connection");
                        }

                    });
        }

        private void InitBusesLocations(List<BakuBusModel> list,string DesiredCode = "")
        {
            ObservableBakuBusesLocations = new ObservableCollection<BakuBusLocation>();
            foreach (BakuBusModel b in list)
                if (b.DISPLAY_ROUTE_CODE.Equals(DesiredCode))
                    ObservableBakuBusesLocations.Add(new BakuBusLocation(b, new Location(Double.Parse(b.LATITUDE), Double.Parse(b.LONGITUDE))));
                else if(DesiredCode.Equals(""))
                    ObservableBakuBusesLocations.Add(new BakuBusLocation(b, new Location(Double.Parse(b.LATITUDE), Double.Parse(b.LONGITUDE))));

        }
    }
}
