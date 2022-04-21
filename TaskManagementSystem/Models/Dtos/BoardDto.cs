using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TaskManagementSystem.Models
{
    public class BoardDto
    {
        [JsonConverter(typeof(IntToStringConverter))]
        public int board_id { get; set; }
        public string name { get; set; }

        [JsonConverter(typeof(IntToStringConverter))]
        public int owner_id { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }

    }
}
