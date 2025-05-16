namespace API.Models.Project
{
    public class UpdateProjectRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
