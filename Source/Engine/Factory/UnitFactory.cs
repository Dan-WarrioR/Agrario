using SFML.Graphics;
using SFML.System;
using Source.Engine.GameObjects;
using Source.Game.Units;

namespace Source.Engine.Factory
{
	public static class UnitFactory
	{
		private const float MinPlayerRadius = 20f;

		private const float FoodRadius = 5f;

		private static Random _random = new();

		public static Player CreatePlayer(bool isAi, FloatRect bounds)
		{
			var player = new Player(isAi, bounds, MinPlayerRadius, GetRandomPosition(bounds));

			RegisterObject(player);

			return player;
		}

		public static Food CreateFood(FloatRect bounds)
		{
			var food = new Food(FoodRadius, GetRandomPosition(bounds));

			RegisterObject(food);	

			return food;
		}

		public static TextObject CreateText(string text, FloatRect bounds)
		{
			var textObject = new TextObject(text, new(bounds.Width - 200, bounds.Top + 20));

			RegisterObject(textObject);

			return textObject;
		}

		private static Vector2f GetRandomPosition(FloatRect bounds)
		{
			float x = _random.Next((int)bounds.Left, (int)(bounds.Left + bounds.Width));
			float y = _random.Next((int)bounds.Top, (int)(bounds.Top + bounds.Height));
			
			return new(x, y);
		}

		private static void RegisterObject(GameObject gameObject)
		{
			GameLoop.Register(gameObject);

			gameObject.OnDisposed += InregisterObject;
		}

		private static void InregisterObject(GameObject gameObject)
		{
			gameObject.OnDisposed -= InregisterObject;

			GameLoop.UnRegister(gameObject);
		}
	}
}
