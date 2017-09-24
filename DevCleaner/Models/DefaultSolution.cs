using System.Collections.Generic;
using System.IO;
using DevCleaner.Models.Projects;
using Prism.Mvvm;

namespace DevCleaner.Models
{
    public class DefaultSolution : BindableBase,ISolution
    {
        public string Name { get; set; }
        private bool _isChecked;

        public bool IsSelected
        {
            get => _isChecked;
            set
            {
                SetProperty(ref _isChecked, value);
                Projects.ForEach(t => t.IsSelected = value);
            }
        }

        public List<Project> Projects { get; set; } = new List<Project>();

        private string _solutionPath;

        public DefaultSolution(string solutionPath)
        {
            _solutionPath = Path.GetDirectoryName(solutionPath);
        }

        public void CleanSolution()
        {
            //Clean the packages folder
            var packagesFolder = Path.Combine(_solutionPath, "packages");
            if (Directory.Exists(packagesFolder))
            {
                Directory.Delete(packagesFolder,true);
            }

            Projects.ForEach(t=>t.CleanProject());
        }
    }
}