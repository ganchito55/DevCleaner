namespace DevCleaner.Models.Projects
{
    public class UwpProject : Project
    {
        public UwpProject(string projectPath) : base(projectPath)
        {
            IconPath = "/Icons/windows.png";
        }
    }
}