namespace API.Models.Task
{
    public class UpdateTaskRequest
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid ProjectId { get; set; }
        public Guid? CategoryId { get; set; }
        public byte[] RowVersion { get; set; } = null!;
    }
}
