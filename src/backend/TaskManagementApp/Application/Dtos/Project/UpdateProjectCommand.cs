﻿namespace Application.Dtos.Project
{
    public class UpdateProjectCommand
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public byte[] RowVersion { get; set; } = null!;
    }
}
