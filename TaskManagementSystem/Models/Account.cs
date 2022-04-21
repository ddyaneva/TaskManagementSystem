using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementSystem.Models
{
    public class Account
    {
        [Key]
        public int account_id { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(255)")]
        public string first_name { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(255)")]
        public string last_name { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(255)")]
        public string email { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
