namespace Source.Engine.Tools.ProjectUtilities
{
	public static class PathHelper
	{
		public static string ProjectPath = AppDomain.CurrentDomain.BaseDirectory;

		public static string ResourcesPath => Path.Combine(ProjectPath, "Resources");
	}
}