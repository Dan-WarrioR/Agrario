using SFML.Graphics;
using SFML.System;
using Source.Engine.GameObjects;
using Source.Engine.GameObjects.UI;
using Source.Engine.Systems;
using Source.Engine.Systems.Tools.Animations;
using Source.Engine.Tools;
using Source.Engine.Tools.ProjectUtilities;
using Source.SeaBattle.Configs;
using Source.SeaBattle.Factories;
using Source.SeaBattle.LevelMap;
using Source.SeaBattle.Users.Controllers;

namespace Source.SeaBattle.Users
{
	public class Player : GameObject
	{
		private EventBus EventBus => _eventBus ??= Dependency.Get<EventBus>();
		private EventBus _eventBus;

		private SeaBattleSoundManager AudioManager => _audioManager ??= Dependency.Get<SeaBattleSoundManager>();
		private SeaBattleSoundManager _audioManager;

		public Map Map { get; private set; }

		public bool IsVisibleShips { get; set; } = false;

		public BaseSeaBattleController Controller { get; private set; }

		private UIShapeObject[,] _cellObjects;

		public override void OnAwake()
		{
			Map = new();

			var mapFactory = new MapFactory();

			_cellObjects = mapFactory.SpawnMap(this, IsVisibleShips);
		}

		public void SetController(BaseSeaBattleController controller)
		{
			Controller = controller;
		}

		public BombState TryBombCell(Vector2i position)
		{
			for (int i = 0; i < Map.Size; i++)
			{
				for (int j = 0; j < Map.Size; j++)
				{
					var cellObject = _cellObjects[j, i];

                    if (cellObject.IsMouseOver(position) && Map.TryGetCell(j, i, out Cell cell))
                    {
						var state = cell.BombCell();

						if (state != BombState.None)
						{
							UpdateCellAfterBomb(cell, cellObject);

							if (state == BombState.Shot)
							{
								EventBus.Invoke("OnPlayerShoot");
							}
							else
							{
								EventBus.Invoke("OnPlayerMissed");
							}
						}									

						return state;
					}
				}
			}

			return BombState.None;
		}

		private void UpdateCellAfterBomb(Cell cell, UIShapeObject cellObject)
		{
			string name = cell switch
			{
				Cell when cell.IsShip && !cell.IsDestroyed => IconsConfig.SeaIconPath,
				Cell when cell.IsDestroyed && !cell.IsShip => IconsConfig.MissShotIconPath,
				Cell when cell.IsDestroyed && cell.IsShip => IconsConfig.DestroyedShipIconPath,
				Cell when cell.IsShip => IconsConfig.ShipIconPath,
				_ => IconsConfig.SeaIconPath,
			};

			var texture = GetTextureForCell(name);

			cellObject.SetTexture(texture);
		}

		private Texture? GetTextureForCell(string name)
		{
			string fullPath = Path.Combine(PathHelper.ResourcesPath, name);

			return TextureLoader.GetTexture(fullPath);
		}
	}
}