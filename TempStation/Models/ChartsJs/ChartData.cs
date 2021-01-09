using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TempStation.Models.ChartsJs
{
    public class ChartData<T>
    {
        [JsonProperty("datasets")]
        public IList<ChartDataset<T>> Datasets { get; set; }

        [JsonProperty("labels")]
        public IList<string> Labels { get; set; }
    }
}
