using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Prism.Mvvm;

namespace DevCleaner.Models.Projects
{
    public class Project : BindableBase
    {
        public string IconPath { get; set; }
        public string Name { get; set; }

        private bool _isSelected;

        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        private string _projectPath;

        public Project(string projectPath)
        {
            _projectPath = Path.GetDirectoryName(projectPath);
            Name = Path.GetFileNameWithoutExtension(projectPath);
        }

        /// <summary>
        /// Remove debug files, dlls...
        /// </summary>
        public virtual void CleanProject()
        {
            //Delete bin folder
            if (Directory.Exists(Path.Combine(_projectPath, "bin")))
            {
                Directory.Delete(Path.Combine(_projectPath, "bin"), true);
            }

            //Delete obj folder
            if (Directory.Exists(Path.Combine(_projectPath, "obj")))
            {
                Directory.Delete(Path.Combine(_projectPath, "obj"), true);
            }


        }

        /// <summary>
        /// Identify project due to the GUID of the project
        /// https://www.codeproject.com/Reference/720512/List-of-Visual-Studio-Project-Type-GUIDs?msg=4996054#xx4996054xx
        /// </summary>
        /// <param name="projectPath"></param>
        /// <returns></returns>
        public static Project IdentifyProject(string projectPath)
        {
            try
            {
                var guids = XDocument
                    .Load(projectPath)
                    .Descendants()
                    .Single(t => t.Name.LocalName == "ProjectTypeGuids")
                    .Value;
                var firstGuid = guids.Split(';')[0];
                switch (firstGuid)
                {
                    case "{EFBA0AD7-5A72-4C68-AF49-83D382785DCF}":
                        return new AndroidProject(projectPath);
                    case "{FEACFBD2-3405-455C-9665-78FE426C6842}":
                        return new IosProject(projectPath);
                    case "{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}":
                        return new CSharpProject(projectPath);
                    case "{A5A43C5B-DE2A-4C0C-9213-0A381AF9435A}":
                        return new UwpProject(projectPath);
                    default:
                        return new Project(projectPath);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
                //TODO Add logs
            }
        }
    }
}