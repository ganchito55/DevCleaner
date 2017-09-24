using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;
using DevCleaner.Models;
using DevCleaner.Models.Projects;
using Prism.Commands;
using Prism.Mvvm;

namespace DevCleaner.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public MainWindowViewModel()
        {
            ScanCommand = new DelegateCommand(Scan);
        }

        private void Scan()
        {
            var folderBrowserDialog = new FolderBrowserDialog();
            var result = folderBrowserDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                ScanPath = folderBrowserDialog.SelectedPath;
                Solutions.Clear();
                new Task(GetAllProjects).Start();
            }
        }

        private void GetAllProjects()
        {
            var solutions = Directory.GetFiles(_scanPath, "*.sln", SearchOption.AllDirectories);
            Regex projectRegex = new Regex(@"((\w| |\.)+\\)+(\w| |\.)+\.csproj");
            foreach (var solution in solutions)
            {
                ISolution solutionType = new DefaultSolution();
                solutionType.Name = Path.GetFileNameWithoutExtension(solution);

                var content = File.ReadAllText(solution);
                var projects = projectRegex.Matches(content);
                foreach (Match project in projects)
                {
                    //Get full path to the csproj file
                    var projectType = Project.IdentifyProject(Path.Combine(Path.GetDirectoryName(solution),project.Value));
                    if (projectType != null)
                    {
                        solutionType.Projects.Add(projectType);
                    }
                }
                App.Current.Dispatcher.Invoke(()=>
                {
                    Solutions.Add(solutionType);
                });
            }
          
        }

        private string _scanPath;

        public string ScanPath
        {
            get => _scanPath;
            set => SetProperty(ref _scanPath, value);
        }

        public ObservableCollection<ISolution> Solutions { get; set; } = new ObservableCollection<ISolution>();

        public DelegateCommand ScanCommand { get; }
    }
}
