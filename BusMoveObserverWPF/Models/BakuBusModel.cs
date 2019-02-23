using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusMoveObserverWPF.Models
{
    public class BakuBusModel
    {
        public string BUS_ID { get; set; }
        public string PLATE { get; set; }
        public string DRIVER_NAME { get; set; }
        public string CURRENT_STOP { get; set; }
        public string PREV_STOP { get; set; }
        public string SPEED { get; set; }
        public string BUS_MODEL { get; set; }
        public string LATITUDE { get; set; }
        public string LONGITUDE { get; set; }
        public string ROUTE_NAME { get; set; }
        public string LAST_UPDATE_TIME { get; set; }
        public string DISPLAY_ROUTE_CODE { get; set; }
        public string SVCOUNT { get; set; }
        

        public override string ToString()
        {
            return DISPLAY_ROUTE_CODE + " " + DRIVER_NAME;
        }
    }
}
