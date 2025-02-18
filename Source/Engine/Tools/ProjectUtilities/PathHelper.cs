namespace Source.Engine.Tools
{
	public static class PathHelper
	{
		public static string ProjectPath = AppDomain.CurrentDomain.BaseDirectory;

		public static string ResourcesPath => Path.Combine(ProjectPath, "Resources");
	}
}