using Source.Engine;
using Source.SeaBattle;

namespace Source
{
	public class Boot
	{
		public static void Main()
		{
			var application = new Application();
			var game = new SeaBattleGameRules();
			var renderer = new SeaBattleRenderer();
			
			application.Run(game, renderer);
		}
	}
}
