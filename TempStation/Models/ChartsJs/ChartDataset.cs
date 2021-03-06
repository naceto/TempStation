using Newtonsoft.Json;
using System.Collections.Generic;

namespace TempStation.Models.ChartsJs
{
    public class ChartDataset<T>
    {
        public ChartDataset()
        {
            Data = new List<T>();
        }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("data")]
        public IList<T> Data { get; set; }

        [JsonProperty("yAxisID")]
        public string YaxisID { get; set; }

        [JsonProperty("borderColor")]
        public string BorderColor { get; set; }

        [JsonProperty("backgroundColor")]
        public string BackgroundColor { get; set; }

        [JsonProperty("fill")]
        public bool Fill { get; set; }
    }
}
