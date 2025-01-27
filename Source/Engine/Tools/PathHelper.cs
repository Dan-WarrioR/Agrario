namespace Source.Engine.Tools
{
	public static class PathHelper
	{
		public static string ProjectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;

		public static string ResourcesPath => Path.Combine(ProjectPath, "Resources");
	}
}