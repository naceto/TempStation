using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TempStation.Data.Models
{
    [Index(nameof(MacAddress), IsUnique = true)]
    public class UserSensor
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string TempStationUserId { get; set; }

        [StringLength(17)]
        public string MacAddress { get; set; }
    }
}
