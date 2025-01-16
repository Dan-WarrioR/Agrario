using SFML.System;

namespace Source.Engine.Tools
{
	public static class Vector2fExtensions
	{
		public static float DistanceTo(this Vector2f source, Vector2f second)
		{
			return MathF.Sqrt(DistanceToSquared(source, second));
		}

		public static float DistanceToSquared(this Vector2f source, Vector2f second)
		{
			float deltaX = second.X - source.X;
			float deltaY = second.Y - source.Y;

			return deltaX * deltaX + deltaY * deltaY;
		}

		public static Vector2f Normalize(this Vector2f source)
		{
			float magnitude = source.Magnitude();

			if (magnitude == 0)
			{
				return new(0, 0);
			}

			return source / magnitude;
		}

		public static float Magnitude(this Vector2f vector)
		{
			return MathF.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
		}
	}
}
