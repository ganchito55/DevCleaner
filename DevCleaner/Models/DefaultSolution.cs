using System.Collections.Generic;
using DevCleaner.Models.Projects;

namespace DevCleaner.Models
{
    public class DefaultSolution : ISolution
    {
        public string Name { get; set; }
        public bool IsSelected { get; set; } = false;
        public List<Project> Projects { get; set; } = new List<Project>();
    }
}