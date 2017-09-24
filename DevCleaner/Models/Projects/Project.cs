using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace DevCleaner.Models.Projects
{
    public class Project
    {
        public string IconPath { get; set; }
        public string Type { get; set; }
        public bool IsSelected { get; set; } = false;

        private string _projectPath;

        public Project(string projectPath)
        {
            _projectPath = projectPath;
            Type = Path.GetFileNameWithoutExtension(projectPath);
        }

        /// <summary>
        /// Remove debug files, dlls...
        /// </summary>
        public virtual void CleanProject()
        {
            //TODO add default clean code
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