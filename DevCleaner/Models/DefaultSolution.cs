using System.Collections.Generic;
using DevCleaner.Models.Projects;

namespace DevCleaner.Models
{
    public class DefaultSolution : ISolution
    {
        public string Name { get; set; }
        private bool _isChecked;

        public bool IsSelected
        {
            get => _isChecked;
            set
            {
                _isChecked = value;
                Projects.ForEach(t => t.IsSelected = value);
            }
        }

        public List<Project> Projects { get; set; } = new List<Project>();
    }
}