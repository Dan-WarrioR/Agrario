using Source.Game.Units;
using SFML.Graphics;
using SFML.System;
using Source.Engine.GameObjects;

namespace Source.Engine.Factory
{
    public class UnitFactory
	{
		private const float MinPlayerRadius = 20f;

		private const float FoodRadius = 5f;

		private GameLoop _gameLoop;

		private FloatRect _bounds;

		private Random _random = new();

		public UnitFactory(GameLoop gameLoop, FloatRect bounds)
		{
			_gameLoop = gameLoop;
			_bounds = bounds;
		}

		public Player CreatePlayer(bool isAi)
		{
			var player = new Player(isAi, _bounds, MinPlayerRadius, GetRandomPosition());

			RegisterObject(player);

			return player;
		}

		public Food CreateFood()
		{
			var food = new Food(FoodRadius, GetRandomPosition());

			RegisterObject(food);	

			return food;
		}

		public TextObject CreateText(string text)
		{
			var textObject = new TextObject(text, new(_bounds.Width - 200, _bounds.Top + 20));

			//RegisterObject(textObject);

			return textObject;
		}

		private Vector2f GetRandomPosition()
		{
			float x = _random.Next((int)_bounds.Left, (int)(_bounds.Left + _bounds.Width));
			float y = _random.Next((int)_bounds.Top, (int)(_bounds.Top + _bounds.Height));
			
			return new(x, y);
		}

		private void RegisterObject(GameObject gameObject)
		{
			_gameLoop.Register(gameObject);

			gameObject.OnDisposed += UnregisterObject;
		}

		private void UnregisterObject(GameObject gameObject)
		{
			gameObject.OnDisposed -= UnregisterObject;

			_gameLoop.UnRegister(gameObject);
		}
	}
}
