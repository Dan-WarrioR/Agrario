using SFML.Graphics;
using SFML.System;
using Source.Engine;
using Source.Engine.GameObjects;
using Source.Engine.GameObjects.UI;
using Source.Engine.Systems.Tools.Animations;
using Source.Engine.Tools.ProjectUtilities;
using Source.SeaBattle.Configs;
using Source.SeaBattle.LevelMap;
using Source.SeaBattle.Users;

namespace Source.SeaBattle.Factories
{
	public class MapFactory : ObjectFactory
	{
		private Vector2f CellSize = new(60, 60);

		public UIShapeObject[,] SpawnMap(Player player, bool isVisibleShips)
		{
			for (int i = 1; i < 11; i++)
			{
				var cellPosition = new Vector2f(i * CellSize.X, 0);
				var position = player.Position + cellPosition;

				SpawnDigit(position, i);
			}

			for (int i = 0; i < 10; i++)
			{
				var cellPosition = new Vector2f(0, (i + 1) * CellSize.Y);
				var position = player.Position + cellPosition;

				SpawnLetter(position, (char)(i + 'A'));
			}

			return SpawnSeaCells(player.Map, player.Position + CellSize, isVisibleShips);
		}

		private UIShapeObject[,] SpawnSeaCells(Map map, Vector2f startPosition, bool isVisibleShips)
		{
			var cells = map.GetCells();
			var buttonObejcts = new UIShapeObject[map.Size, map.Size];

			for (int i = 0; i < map.Size; i++)
			{
				for (int j = 0; j < map.Size; j++)
				{
					var cell = cells[j, i];

					var position = startPosition + new Vector2f(j * CellSize.X, i * CellSize.Y);

					UIShapeObject cellObject;

					if (cell.IsShip && isVisibleShips)
					{
						cellObject = SpawnShip(position);
					}
					else
					{
						cellObject = SpawnSea(position);
					}

					buttonObejcts[j, i] = cellObject;
				}
			}

			return buttonObejcts;
		}



		private void SpawnDigit(Vector2f position, int digit)
		{
			SpawnUnit<UIShapeObject>(position, $"SeaBattle\\Field\\Numbers\\{digit}.png");
		}

		private void SpawnLetter(Vector2f position, char letter)
		{
			SpawnUnit<UIShapeObject>(position, $"SeaBattle\\Field\\Alphabet\\{letter}.png");
		}

		private UIShapeObject SpawnSea(Vector2f position)
		{
			return SpawnCell<UIShapeObject>(position, IconsConfig.SeaIconPath);
		}

		private UIShapeObject SpawnShip(Vector2f position)
		{
			return SpawnCell<UIShapeObject>(position, IconsConfig.ShipIconPath);
		}

		private T SpawnCell<T>(Vector2f position, string path) where T : UIShapeObject, new()
		{
			return SpawnUnit<T>(position, path);
		}

		private T SpawnUnit<T>(Vector2f position, string path) where T : ShapeObject, new()
		{
			var unit = Instantiate<T>();
			var icon = GetTextureForCell(path);

			unit.Initialize(new RectangleShape(CellSize), position);
			unit.SetTexture(icon);

			return unit;
		}

		private Texture? GetTextureForCell(string name)
		{
			string fullPath = Path.Combine(PathHelper.ResourcesPath, name);

			return TextureLoader.GetTexture(fullPath);
		}
	}
}