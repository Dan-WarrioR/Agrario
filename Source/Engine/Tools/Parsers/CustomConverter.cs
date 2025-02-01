using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Source.Engine.Tools.Parsers
{
	public static class CustomConverter
	{
		public static Vector2f Parse(Vector2f _, string data)
		{
			var values = data.Split(',').Select(float.Parse).ToArray();
			return new Vector2f(values[0], values[1]);
		}

		public static FloatRect Parse(FloatRect _, string data)
		{
			var values = data.Split(',').Select(float.Parse).ToArray();
			return new FloatRect(values[0], values[1], values[2], values[3]);
		}

		public static Color Parse(Color _, string data)
		{
			var values = data.Split(',').Select(byte.Parse).ToArray();
			return new Color(values[0], values[1], values[2], values.Length > 3 ? values[3] : (byte)255);
		}

		public static VideoMode Parse(VideoMode _, string data)
		{
			var values = data.Split(',').Select(uint.Parse).ToArray();
			return new VideoMode(values[0], values[1], values.Length > 2 ? values[2] : 32);
		}
	}
}