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

        [StringLength(100)]
        [Required]
        public string Name { get; set; }

        [Required]
        public string TempStationUserId { get; set; }

        [StringLength(17)]
        [Required]
        public string MacAddress { get; set; }
    }
}
