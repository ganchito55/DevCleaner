using System.Collections.Generic;
using DevCleaner.Models.Projects;

namespace DevCleaner.Models
{
    public interface ISolution
    {
        string Name { get; set; }
        bool IsSelected { get; set; }
        List<Project> Projects { get; set; }
    }
}