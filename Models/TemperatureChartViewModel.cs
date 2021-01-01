using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TempStation.Models
{
    public class TemperatureChartViewModel<T>
    {
        public TemperatureChartViewModel()
        {
            Labels = new List<string>();
        }

        [JsonProperty("datasets")]
        public IList<TemperatureDataset<T>> Datasets { get; set; }

        [JsonProperty("labels")]
        public IList<string> Labels { get; set; }
    }

    public class TemperatureDataset<T>
    {
        public TemperatureDataset()
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
