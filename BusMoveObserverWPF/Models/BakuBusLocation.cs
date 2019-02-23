using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusMoveObserverWPF.Models
{
    public class BakuBusLocation
    {
        public BakuBusModel BakuBusModel{get;set;}
        public Location Location { get; set; }
        public string BakuBusRouteCode { get; set; }

        public BakuBusLocation(BakuBusModel bakuBusModel, Location location)
        {
            BakuBusModel = bakuBusModel;
            Location = location;
            BakuBusRouteCode = bakuBusModel.DISPLAY_ROUTE_CODE;
        }
    }
}
