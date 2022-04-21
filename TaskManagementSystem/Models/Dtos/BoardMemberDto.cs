using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TaskManagementSystem.Models
{
    public class BoardMemberDto
    {
        public BoardDto boardId { get; set; }

        [JsonConverter(typeof(IntToStringConverter))]
        public int board_id { get; set; }

        public AccountDto accountId { get; set; }
        [JsonConverter(typeof(IntToStringConverter))]
        public int account_id { get; set; }
    }
}
