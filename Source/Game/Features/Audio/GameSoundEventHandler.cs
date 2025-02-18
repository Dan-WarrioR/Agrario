using Source.Engine.Systems;
using Source.Engine.Tools;
using Source.Game.Configs;

namespace Source.Game.Features.Audio
{
    public class GameSoundEventHandler
    {
        private EventBus EventBus => _eventBus ??= Dependency.Get<EventBus>();
        private EventBus _eventBus;
        
        private AudioManager AudioManager => _audioManager ??= Dependency.Get<AudioManager>();
        private AudioManager _audioManager;
        
        private string _restartSound;
        private string _swapSound;
        private string _eatSound;

        public GameSoundEventHandler()
        {
            _restartSound = AudioConfig.RestarGameSound;
            _swapSound = AudioConfig.SwapSound;
            _eatSound = AudioConfig.EatSound;
            
            EventBus.Register("OnGameRestart", OnRestartGame);
            EventBus.Register("OnPlayerSwapped", OnPlayersSwap);
            EventBus.Register("OnPlayerAteFood", OnPlayerAteFood);
        }

        private void OnRestartGame()
        {
            AudioManager.PlayOnced(_restartSound);
        }

        private void OnPlayersSwap()
        {
            AudioManager.PlayOnced(_swapSound);
        }

        private void OnPlayerAteFood()
        {
            AudioManager.PlayOnced(_eatSound);
        }
    }
}