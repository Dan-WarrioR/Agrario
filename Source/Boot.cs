using Source.Engine;
using Source.Game;

namespace Source
{
	public class Boot
	{
		public static void Main()
		{
			var application = new Application();
			var game = new AgarioGame();
			var renderer = new SFMLRenderer();
			
			application.Run(game, renderer);
		}
	}
}
