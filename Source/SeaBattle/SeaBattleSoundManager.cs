using Source.Engine.Systems;
using Source.Engine.Tools;
using Source.SeaBattle.Configs;

namespace Source.SeaBattle
{
	public class SeaBattleSoundManager
	{
		private EventBus EventBus => _eventBus ??= Dependency.Get<EventBus>();
		private EventBus _eventBus;

		private AudioManager AudioManager => _audioManager ??= Dependency.Get<AudioManager>();
		private AudioManager _audioManager;

		private string _restartSound;
		private string _shootInShipSound;
		private string _shootInSeaSound;
		private string _musicSound;

		public SeaBattleSoundManager()
		{
			Dependency.Register(this);

			_shootInShipSound = AudioConfig.ShootInShipSound;
			_shootInSeaSound = AudioConfig.ShootInSeaSound;
			_musicSound = AudioConfig.MusicSound;

			EventBus.Register("OnPlayerMissed", OnPlayerMissed);
			EventBus.Register("OnPlayerShoot", OnPlayerShoot);
		}

		~SeaBattleSoundManager()
		{
			Dependency.Unregister(this);

			EventBus.Unregister("OnPlayerMissed", OnPlayerMissed);
			EventBus.Unregister("OnPlayerShoot", OnPlayerShoot);
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

		private void OnPlayerMissed()
		{
			AudioManager.PlayOnced(_shootInSeaSound, true);
		}

		private void OnPlayerShoot()
		{
			AudioManager.PlayOnced(_shootInShipSound, true);
		}
	}
}