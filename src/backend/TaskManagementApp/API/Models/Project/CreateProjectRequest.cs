﻿namespace API.Models.Project
{
    public class CreateProjectRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
