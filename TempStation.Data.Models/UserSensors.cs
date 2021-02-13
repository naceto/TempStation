using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TempStation.Data.Models
{
    [Index(nameof(UserSensor.MacAddress))]
    public class UserSensor
    {
        public string Id { get; set; }

        public string TempStationUserId { get; set; }

        [StringLength(17)]
        public string MacAddress { get; set; }
    }
}
