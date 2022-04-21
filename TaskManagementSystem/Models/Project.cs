using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TaskManagementSystem.Models
{
    public class Project
    {
        [Key]
        public int project_id { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(50)")]
        public string key { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(250)")]
        public string title { get; set; }

        public Account owner { get; set; }
        [ForeignKey("owner")]
        [JsonConverter(typeof(IntToStringConverter))]
        public int owner_id { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
