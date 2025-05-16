using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Project
{
    public class CreateProjectCommand
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
