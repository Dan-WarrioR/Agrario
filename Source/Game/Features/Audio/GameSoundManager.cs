using Source.Engine.Systems;
using Source.Engine.Tools;
using Source.Game.Configs;

namespace Source.Game.Features.Audio
{
    public class GameSoundManager
    {
        private EventBus EventBus => _eventBus ??= Dependency.Get<EventBus>();
        private EventBus _eventBus;
        
        private AudioManager AudioManager => _audioManager ??= Dependency.Get<AudioManager>();
        private AudioManager _audioManager;
        
        private string _restartSound;
        private string _swapSound;
        private string _eatSound;
        private string _musicSound;

        public GameSoundManager()
        {
            _restartSound = AudioConfig.RestarGameSound;
            _swapSound = AudioConfig.SwapSound;
            _eatSound = AudioConfig.EatSound;
            _musicSound = AudioConfig.Music2Sound;
            
            EventBus.Register("OnGameRestart", OnRestartGame);
            EventBus.Register("OnPlayerSwapped", OnPlayersSwap);
            EventBus.Register("OnPlayerAteFood", OnPlayerAteFood);
        }

        public void PlayMainMenuMusic()
        {
            AudioManager.SetVolume(_musicSound, 20f);
            AudioManager.PlayLooped(_musicSound);
        }

        public void StopAllSounds()
        {
            _audioManager.StopAllSounds();
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