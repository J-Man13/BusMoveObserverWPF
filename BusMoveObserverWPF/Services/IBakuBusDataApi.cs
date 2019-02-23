using BusMoveObserverWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusMoveObserverWPF.Services
{
    public interface IBakuBusDataApi
    {
        IEnumerable<BakuBusModel> GetBakuBusModels();
    }
}
