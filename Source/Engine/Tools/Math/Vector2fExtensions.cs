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

		public static Vector2f Lerp(this Vector2f start, Vector2f end, float t)
		{
			t = Math.Clamp(t, 0, 1);

			float x = start.X + (end.X - start.X) * t;
			float y = start.Y + (end.Y - start.Y) * t;

			return new (x, y);
		}

		public static Vector2f LerpUnclamped(this Vector2f start, Vector2f end, float t)
		{
			float x = start.X + (end.X - start.X) * t;
			float y = start.Y + (end.Y - start.Y) * t;

			return new(x, y);
		}
	}
}
