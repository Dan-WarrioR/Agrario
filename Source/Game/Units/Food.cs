using SFML.Graphics;
using Source.Engine.GameObjects;
using Source.Engine.Tools.Math;

namespace Source.Game.Units
{
	public interface IEater
	{
		public bool TryEat(IFood food);
	}

	public interface IFood
	{
		public float Mass { get; }

		public event Action OnBeingEaten;

		public void EatMe();

		public bool CanBeEatenBy(Player player);
	}

	public class Food : CircleObject, IFood
	{
		private static readonly Color FoodColor = new(230, 57, 70);

		private const float MassMultiplier = 1f;

		protected override Color FillColor => FoodColor;

		public float Mass => Radius * Radius * MathF.PI * MassMultiplier;

		public event Action OnBeingEaten;

		public void EatMe()
		{
			SetActive(false);

			OnBeingEaten?.Invoke();
		}

		public bool CanBeEatenBy(Player player)
		{
			float distance = Position.DistanceTo(player.Position);

			return player.Radius > Radius && distance + Radius < player.Radius && IsActive;
		}
	}
}