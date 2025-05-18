namespace API.Models.Category
{
    public class CategoryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
