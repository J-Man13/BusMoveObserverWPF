using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BusMoveObserverWPF.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BusMoveObserverWPF.Services
{
    public class BakuBusDataApiJson : IBakuBusDataApi
    {
        public readonly string BAKUBUS_API_URL;

        public BakuBusDataApiJson()
        {
            BAKUBUS_API_URL = ConfigurationSettings.AppSettings["BakuBusWebApiURLString"];
        }

        public IEnumerable<BakuBusModel> GetBakuBusModels()
        {
            LinkedList<BakuBusModel> bakuBusModels = new LinkedList<BakuBusModel>();

            try
            {
                JObject jObject = GetJObject();
                LinkedList<JToken> jTokens = new LinkedList<JToken>(jObject.GetValue("BUS").ToArray());
                foreach (JToken jt in jTokens)
                {
                    BakuBusModel bakuBusModel = JsonConvert.DeserializeObject<BakuBusModel>(jt.ToArray().FirstOrDefault().ToArray().FirstOrDefault().ToString());

                    if (bakuBusModel.LATITUDE.Trim().Length == 0 ||
                        bakuBusModel.LONGITUDE.Trim().Length == 0 ||
                        bakuBusModel.LATITUDE.Trim().ToString().StartsWith("0") ||
                        bakuBusModel.LONGITUDE.Trim().ToString().StartsWith("0"))
                        continue;

                    if (bakuBusModel.LATITUDE.ToString().Contains(","))
                        bakuBusModel.LATITUDE = bakuBusModel.LATITUDE.Replace(',', '.');

                    if (bakuBusModel.LONGITUDE.ToString().Contains(","))
                        bakuBusModel.LONGITUDE = bakuBusModel.LONGITUDE.Replace(',', '.');


                    if (!bakuBusModel.LATITUDE.ToString().Contains("."))
                        bakuBusModel.LATITUDE = bakuBusModel.LATITUDE.ToString().Substring(0, 2) + "." + bakuBusModel.LATITUDE.ToString().Substring(2, bakuBusModel.LATITUDE.ToString().Length - 3);

                    if (!bakuBusModel.LONGITUDE.ToString().Contains("."))
                        bakuBusModel.LONGITUDE = bakuBusModel.LONGITUDE.ToString().Substring(0, 2) + "." + bakuBusModel.LONGITUDE.ToString().Substring(2, bakuBusModel.LONGITUDE.ToString().Length - 3);

                    bakuBusModels.AddLast(bakuBusModel);
                }
            }
            catch (Exception)
            {
                return null;
            }
            return bakuBusModels;
        }

        private JObject GetJObject()
        {
            WebClient webClient = new WebClient();
            string jsonString = webClient.DownloadString(BAKUBUS_API_URL);
            return JsonConvert.DeserializeObject(jsonString) as JObject;
        }


    }
}
