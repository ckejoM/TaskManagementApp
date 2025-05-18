namespace Application.Dtos.Category
{
    public class UpdateCategoryCommand
    {
        public string Name { get; set; } = string.Empty;
        public byte[] RowVersion { get; set; } = null!;
    }
}
