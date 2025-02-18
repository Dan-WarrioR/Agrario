using Source.Engine.Systems.Animation;
using Source.Engine.Systems.Tools.Animations;
using Source.Engine.Tools;
using Source.Game.Configs;
using Source.Game.Units;

namespace Source.Game.Data.Animations
{
    public class PlayerAnimationData : AnimationData
    {
        public PlayerAnimationData(Player player)
        {
            var textureLoader = Dependency.Get<TextureLoader>();

            var idle = new AnimationState("Idle", textureLoader.GetSpritesheetTextures(PlayerConfig.SkullIdleSpritePath), 0.1f);
            var run = new AnimationState("Run", textureLoader.GetSpritesheetTextures(PlayerConfig.SkullAggresiveSpritePath), 0.1f);			

            InitialState = idle;
            
            States.Add(idle);
            States.Add(run);
            
            Transitions.Add(new AnimationTransition(idle, run, player.IsMoving));
            Transitions.Add(new AnimationTransition(run, idle, () => !player.IsMoving()));
        }
    }
}