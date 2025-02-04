using System.Globalization;
using System.Reflection;
using Source.Engine.Tools;
using Source.Engine.Tools.Parsers;

namespace Source.Engine.Configs
{
	public static class ConfigLoader
	{
		public static void SaveConfig(Type config)
		{
			var filePath = $"{config.Name}.ini";

			using StreamWriter writer = new(filePath);

			foreach (var field in config.GetFields(BindingFlags.Public | BindingFlags.Static))
			{
				object value = field.GetValue(null) ?? "";
				writer.WriteLine($"{field.Name}={value}");
			}
		}

		public static void LoadConfig(Type type)
		{
			var filePath = $"{type.Name}.ini";

			if (!File.Exists(filePath))
			{
				SaveConfig(type);

				return;
			}

			StreamReader file = new(filePath);

			while (!file.EndOfStream)
			{
				string line = file.ReadLine().Trim();

				if (string.IsNullOrEmpty(line) || line.StartsWith("#"))
				{
					continue;
				}

				if (line.Contains('='))
				{
					string[] parts = line.Split('=', 2);
					string fieldName = parts[0].Trim();
					string fieldValue = parts[1].Trim();

					SetupField(type, fieldName, fieldValue);
				}
			}
		}

		private static void SetupField(Type type, string fieldName, string value)
		{
			FieldInfo field = type.GetField(fieldName);
			
			if (field == null)
			{
				Debug.LogWarning($"No field in {type.Name} with name {fieldName}!");
			}

			try
			{
				var parsedValue = GetParsedValue(field.FieldType, value);

				field.SetValue(null, parsedValue);
			}
			catch (Exception)
			{

			}		
		}

		private static object? GetParsedValue(Type type, string value)
		{
			return type switch
			{
				_ when type == typeof(int) => int.Parse(value),
				_ when type == typeof(float) => float.Parse(value, NumberStyles.Float),
				_ when type == typeof(bool) => bool.Parse(value),
				_ when type == typeof(string) => value,
				_ => CustomConverter.TryParse(type, value, out object parsedValue) ? parsedValue : default
			};
		}
	}
}