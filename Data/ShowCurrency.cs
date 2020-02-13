using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;

namespace CurrencyConverter.Data
{
    public class Currency
    {
        public DateTime Date { get; set; }

        public DateTime PreviousDate { get; set; }

        public string PreviousURL { get; set; }

        public DateTime Timestamp { get; set; }

        public ObservableDictionary<string,ValuteType> Valute { get; set; }
    }


    public class ValuteType
    {
        public string ID { get; set; }

        public string NumCode { get; set; }

        public string CharCode { get; set; }

        public int Nominal { get; set; }

        public string Name { get; set; }

        public double Value { get; set; }

        public double Previous { get; set; }
    }


    public static class ShowCurrency
    {
        public static async Task<Currency> _download_serialized_json_data(string url) 
        {
            using (var w = new WebClient())
            {
                var json_data = string.Empty;
                try
                {
                    json_data = Encoding.UTF8.GetString( await w.DownloadDataTaskAsync(url));
                }
                catch (Exception) { }
                return !string.IsNullOrEmpty(json_data) ? JsonConvert.DeserializeObject<Currency>(json_data) : new Currency();
            }
        }
    }

    
}
