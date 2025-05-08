using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Task : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Priority Priority { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? DueDate { get; set; }
        public Guid ProjectId { get; set; }
        public Guid? CategoryId { get; set; }
    }
    public enum Priority
    {
        Low,
        Medium,
        High
    }
}
