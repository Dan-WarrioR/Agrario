using Source.Engine;
using Source.Engine.Systems.SceneSystem;
using Source.SeaBattle.Objects;

namespace Source.SeaBattle.Scenes
{
    public class SeaBattleGameScene : BaseScene
    {
		public override void Load()
        {
			ObjectFactory.Instantiate<GameManager>();
		}

		public override void Unload()
        {
            
        }
    }
}