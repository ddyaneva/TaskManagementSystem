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

    public enum Priorities
    {
        Low,
        Medium,
        High
    }
    public enum Types
    {
        Story,
        Bug,
        Epic,
        NewFeature,
        TechnicalDebt
    }
    public enum Status
    {
        New,
        InDevelopment,
        InQA,
        Done
    }

    public class Task
    {
       
        [Key]
        public int task_id { get; set; }

        [Required]
        [Column(TypeName="VARCHAR(255)")]
        public string title { get; set; }

        public Project project { get; set; }
        [ForeignKey("project")]
        [JsonConverter(typeof(IntToStringConverter))]
        public int project_id { get; set; }

        public Board board { get; set; }
        [ForeignKey("board")]
        [JsonConverter(typeof(IntToStringConverter))]
        public int board_id { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(50)")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Types type { get; set; }

        [Required]
        [Column(TypeName="VARCHAR(50)")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        
        public Status status { get; set; } = Status.New;
                
        public string description { get; set; }

        public Account reportedBy{ get; set; }

        [ForeignKey("reportedBy")]
        [JsonConverter(typeof(IntToStringConverter))]
        public int reported_by { get; set; }

#nullable enable
        public Account? assigedTo { get; set; }
        [ForeignKey("assignedTo")]
     //   [JsonConverter(typeof(IntToStringConverter))]
        public int? assigned_to { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR(50)")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Priorities priority { get; set; } = Priorities.Low;

      //  [JsonConverter(typeof(IntToStringConverter))]
        public int? story_points { get; set; }

        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
}
