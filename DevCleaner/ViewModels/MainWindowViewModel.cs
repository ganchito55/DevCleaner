using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevCleaner.Models;
using DevCleaner.Models.Projects;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Prism.Commands;
using Prism.Mvvm;
using Application = System.Windows.Application;

namespace DevCleaner.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private bool _cleanAllSolutions;

        private int _cleanProgress;

        private string _scanPath;

        public MainWindowViewModel()
        {
            ScanCommand = new DelegateCommand(Scan);
            CleanCommand = new DelegateCommand(Clean);
            AboutCommand = new DelegateCommand(About);
        }

        private async void About()
        {
            await ((MetroWindow) App.Current.MainWindow).ShowMessageAsync("About",
                "Created by Jorge Durán aka ganchito55\n" +
                "Icons by FlatIcon\n" +
                "Theme by MahApps.Metro\n" +
                "With PRISM as MVVM Framework");
        }

        public string ScanPath
        {
            get => _scanPath;
            set => SetProperty(ref _scanPath, value);
        }

        public bool CleanAllSolutions
        {
            get => _cleanAllSolutions;
            set
            {
                SetProperty(ref _cleanAllSolutions, value);
                foreach (var solution in Solutions)
                    solution.IsSelected = true;
            }
        }

        public int CleanProgress
        {
            get => _cleanProgress;
            set => SetProperty(ref _cleanProgress, value);
        }

        public ObservableCollection<ISolution> Solutions { get; set; } = new ObservableCollection<ISolution>();

        public DelegateCommand ScanCommand { get; }
        public DelegateCommand CleanCommand { get; }
        public DelegateCommand AboutCommand { get; }

        private void Clean()
        {
            new Task(() =>
            {
                foreach (var solution in Solutions)
                {
                    solution.CleanSolution();
                    CleanProgress++;
                }

                CleanProgress = 0;
            }).Start();
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
            var projectRegex = new Regex(@"((\w| |\.)+\\)+(\w| |\.)+\.csproj");
            foreach (var solution in solutions)
            {
                ISolution solutionType = new DefaultSolution(solution);
                solutionType.Name = Path.GetFileNameWithoutExtension(solution);

                var content = File.ReadAllText(solution);
                var projects = projectRegex.Matches(content);
                foreach (Match project in projects)
                {
                    //Get full path to the csproj file
                    var projectType =
                        Project.IdentifyProject(Path.Combine(Path.GetDirectoryName(solution), project.Value));
                    if (projectType != null)
                        solutionType.Projects.Add(projectType);
                }
                Application.Current.Dispatcher.Invoke(() => { Solutions.Add(solutionType); });
            }
        }
    }
}