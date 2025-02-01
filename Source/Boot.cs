using Source.Engine;
using Source.Engine.Configs;

namespace Source
{
	public class Boot
	{
		public static void Main()
		{
			Application application = new();

			application.Run(new List<BaseConfig>()
			{
				new WindowConfig(),
			});
		}
	}
}
