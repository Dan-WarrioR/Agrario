using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Source.Engine.Tools.Parsers
{
	public static class CustomConverter
	{
		private static Dictionary<Type, Func<string, object>> _converters = new()
		{
			{typeof(Vector2f), ConvertToVector2f},
			{typeof(FloatRect), ConvertToFloatRect},
			{typeof(Color), ConvertToColor},
			{typeof(VideoMode), ConvertToVideoMode},
		};

		public static bool TryParse<T>(string data, out T result)
		{
			if (!_converters.TryGetValue(typeof(T), out var converter))
			{
				result = default;
				return false;
			}

			try
			{
				result = (T)converter(data);
				return true;
			}
			catch (Exception)
			{
				Console.WriteLine($"No converter found for type {typeof(T)}");
			}

			result = default;
			return false;
		}

		private static object ConvertToVector2f(string data)
		{
			var values = data.Split(',').Select(float.Parse).ToArray();
			return new Vector2f(values[0], values[1]);
		}

		public static object ConvertToFloatRect(string data)
		{
			var values = data.Split(',').Select(float.Parse).ToArray();
			return new FloatRect(values[0], values[1], values[2], values[3]);
		}

		public static object ConvertToColor(string data)
		{
			var values = data.Split(',').Select(byte.Parse).ToArray();
			return new Color(values[0], values[1], values[2], values.Length > 3 ? values[3] : (byte)255);
		}

		public static object ConvertToVideoMode(string data)
		{
			var values = data.Split(',').Select(uint.Parse).ToArray();
			return new VideoMode(values[0], values[1], values.Length > 2 ? values[2] : 32);
		}
	}
}