namespace DevCleaner.Models.Projects
{
    public class CSharpProject : Project
    {
        public CSharpProject(string projectPath) : base(projectPath)
        {
            IconPath = "/Icons/csharp.png";
        }
    }
}