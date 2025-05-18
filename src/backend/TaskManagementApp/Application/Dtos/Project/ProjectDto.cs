namespace Application.Dtos.Project
{
    public class ProjectDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public byte[] RowVersion { get; set; } = null!;

        public static ProjectDto FromEntity(Domain.Entities.Project project)
        {
            return new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                CreatedBy = project.CreatedBy,
                CreatedOn = project.CreatedOn,
                ModifiedBy = project.ModifiedBy,
                ModifiedOn = project.ModifiedOn,
                RowVersion = project.RowVersion
            };
        }
    }
}
