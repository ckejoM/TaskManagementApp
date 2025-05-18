namespace Application.Dtos.Task
{
    public class CreateTaskCommand
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid ProjectId { get; set; }
        public Guid? CategoryId { get; set; }
    }
}
