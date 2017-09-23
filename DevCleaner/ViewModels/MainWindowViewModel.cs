using System.Data;
using System.IO;
using System.Windows.Forms;
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
            }
        }

        private string _scanPath = "Introduce your path";

        public string ScanPath
        {
            get => _scanPath;
            set => SetProperty(ref _scanPath, value);
        }

        public DelegateCommand ScanCommand { get; }
    }
}
