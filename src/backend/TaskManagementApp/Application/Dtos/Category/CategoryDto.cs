namespace Application.Dtos.Category
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public byte[] RowVersion { get; set; } = null!;

        public static CategoryDto FromEntity(Domain.Entities.Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                CreatedBy = category.CreatedBy,
                CreatedAt = category.CreatedOn,
                ModifiedBy = category.ModifiedBy,
                ModifiedAt = category.ModifiedOn,
                RowVersion = category.RowVersion,
            };
        }
    }
}
