namespace DevCleaner.Models.Projects
{
    public class IosProject : Project
    {
        public IosProject(string projectPath) : base(projectPath)
        {
            IconPath = "/Icons/apple.png";
        }
    }
}