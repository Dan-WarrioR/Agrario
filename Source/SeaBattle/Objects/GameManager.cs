using Source.Engine.GameObjects;
using Source.Engine.Systems;
using Source.Engine.Tools;
using Source.SeaBattle.Factories;
using Source.SeaBattle.Users;

namespace Source.SeaBattle.Objects
{
    public class GameManager : GameObject
    {
		private EventBus EventBus => _eventBus ??= Dependency.Get<EventBus>();
		private EventBus _eventBus;

		private Player _player1;
		private Player _player2;
		private UnitFactory _unitFactory;
		private Player _currentPlayer;

		//////////////////////////////////////////////////

		#region Life Cycle

		public override void OnAwake()
        {
			Dependency.Register(this);

			var seaBattleAuidoManager = new SeaBattleSoundManager();
			seaBattleAuidoManager.PlayMainMenuMusic();

			GenerateUnits();
		}

		public override void OnDestroy()
		{
			Dependency.Unregister(this);
		}

		public override void OnUpdate(float deltaTime)
		{
			MakePlayerShoot();

			CheckForWin();
		}

		#endregion

		//////////////////////////////////////////////////

		#region Spawn

		private void GenerateUnits()
		{
			var unitFactory = new UnitFactory();

			_player1 = unitFactory.SpawnPlayer(new(100, 100));
			_player2 = unitFactory.SpawnEnemy(new(800, 100));

			_currentPlayer = _player1;
		}

		#endregion

		//////////////////////////////////////////////////	
		
		private void MakePlayerShoot()
		{
			var state = _currentPlayer.Controller.TryShoot();

			if (state == LevelMap.BombState.Miss)
			{
				SwitchTurn();
			}	
		}

		private void SwitchTurn()
		{
			_currentPlayer = _currentPlayer == _player1 ? _player2 : _player1;
		}

		private void CheckForWin()
		{
			if (_player1.Map.ShipsCount > 0 && _player2.Map.ShipsCount > 0)
			{
				return;
			}

			EventBus.Invoke("OnStopGame");
		}
	}
}