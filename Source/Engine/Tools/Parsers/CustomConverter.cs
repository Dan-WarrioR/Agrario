using SFML.Graphics;
using SFML.System;
using System.Globalization;

namespace Source.Engine.Tools.Parsers
{
	public static class CustomConverter
	{
		private static Dictionary<Type, Func<string, object>> _converters = new()
		{
			{typeof(Vector2f), ConvertToVector2f},
			{typeof(FloatRect), ConvertToFloatRect},
			{typeof(Color), ConvertToColor},
		};

		public static bool TryParse<T>(Type type, string data, out T result)
		{
			if (!_converters.TryGetValue(type, out var converter))
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
			var parts = data.Split(new[] { "[Vector2f]", "X", "Y", "(", ")", " " }, StringSplitOptions.RemoveEmptyEntries);

			if (parts.Length == 2 &&
				float.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out var x) &&
				float.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out var y))
			{
				return new Vector2f(x, y);
			}
			return default;
		}

		private static object ConvertToFloatRect(string data)
		{
			var parts = data.Split(new[] { "[FloatRect]", "Width", "Height", "Top", "Left", "X", "Y", "(", ")", " " }, StringSplitOptions.RemoveEmptyEntries);

			if (parts.Length == 4 &&
				float.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out var left) 
				&& float.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out var top) 
				&& float.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out var width) 
				&& float.TryParse(parts[3], NumberStyles.Float, CultureInfo.InvariantCulture, out var height))
			{
				return new FloatRect(left, top, width, height);
			}
			return default;
		}

		private static object ConvertToColor(string data)
		{
			var parts = data.Split(new[] { "[Color]", "R", "G", "B", "A", "(", ")", " " }, StringSplitOptions.RemoveEmptyEntries);

			if (parts.Length == 4 &&
				byte.TryParse(parts[0], out var r) &&
				byte.TryParse(parts[1], out var g) &&
				byte.TryParse(parts[2], out var b) &&
				byte.TryParse(parts[3], out var a))
			{
				return new Color(r, g, b, a);
			}

			return default;
		}
	}
}