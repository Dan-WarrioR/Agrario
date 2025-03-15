using Source.Engine;
using Source.Engine.Rendering;
using Source.Game;
using Source.SeaBattle;

namespace Source
{
	public class Boot
	{
		public static void Main()
		{
			var application = new Application();
			var game = new AgarioGameRules();
			var renderer = new BaseRenderer();
			
			application.Run(game, renderer);
		}
	}
}
