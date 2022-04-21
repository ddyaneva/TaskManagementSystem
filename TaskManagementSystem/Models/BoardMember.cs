using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TaskManagementSystem.Models
{
    public class BoardMember
    {
        public Board boardId { get; set; }
        [Key]
        [ForeignKey("boardId")]
        [JsonConverter(typeof(IntToStringConverter))]
        public int board_id { get; set; }

        public Account accountId { get; set; }

        [ForeignKey("accountId")]
        [JsonConverter(typeof(IntToStringConverter))]
        public int account_id { get; set; }
    }
}
