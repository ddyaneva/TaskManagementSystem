using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TaskManagementSystem.Models
{
    public class AccountDto
    {
        [JsonConverter(typeof(IntToStringConverter))]
        public int account_id { get; set; }
        public string first_name { get; set; }

        public string last_name { get; set; }
        public string email { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
