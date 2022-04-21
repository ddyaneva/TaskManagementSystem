using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Text.Json;

namespace TaskManagementSystem.Models
{

    public class TaskDto
    {
        [JsonConverter(typeof(IntToStringConverter))]
        public int task_id { get; set; }
        public string title { get; set; }

        public ProjectDto project { get; set; }

        [JsonConverter(typeof(IntToStringConverter))]
        public int project_id { get; set; }

        public BoardDto board { get; set; }

        [JsonConverter(typeof(IntToStringConverter))]
        public int board_id { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Types type { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]

        public Status status { get; set; } = Status.New;
                
        public string description { get; set; }

        public AccountDto reportedBy{ get; set; }

        [JsonConverter(typeof(IntToStringConverter))]
        public int reported_by { get; set; }

        public int? assigned_to { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Priorities priority { get; set; } = Priorities.Low;

        public int? story_points { get; set; }

        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
