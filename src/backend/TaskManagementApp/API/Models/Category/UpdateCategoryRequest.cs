namespace API.Models.Category
{
    public class UpdateCategoryRequest
    {
        public string Name { get; set; } = string.Empty;
        public byte[] RowVersion { get; set; } = null!;
    }
}
