using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TaskManagementSystem.Models
{
    public class ProjectDto
    {
        [Key]
        [JsonConverter(typeof(IntToStringConverter))]
        public int project_id { get; set; }

        public string key { get; set; }
        public string title { get; set; }

        public AccountDto owner { get; set; }

        [JsonConverter(typeof(IntToStringConverter))]
        public int owner_id { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
