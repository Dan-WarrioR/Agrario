using Source.Engine;
using Source.Game;
using Source.Game.GameStates;

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
