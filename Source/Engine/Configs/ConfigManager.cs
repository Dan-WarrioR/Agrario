using Source.Engine.Tools;
using Source.Engine.Tools.Parsers;
using System.Text;

namespace Source.Engine.Configs
{
    public class ConfigManager
    {
        private static readonly string ConfigPath = Path.Combine(PathHelper.ResourcesPath, "configs.ini");

        private static readonly Dictionary<string, Dictionary<string, string>> _configsData = new();

        private static readonly List<BaseConfig> _registeredConfigs = new();

        public ConfigManager()
        {
            Dependency.Register(this);
        }

        public void RegisterConfig(BaseConfig config)
        {
            if (!_registeredConfigs.Contains(config))
            {
				_registeredConfigs.Add(config);
			}      
        }
        
        public void RegisterConfigs(IEnumerable<BaseConfig> configs)
        {
            foreach (var config in configs)
            {
				if (!_registeredConfigs.Contains(config))
				{
					_registeredConfigs.Add(config);
				}
			}
        }

        public void InitializeCofigs()
        {
            foreach (var config in _registeredConfigs)
            {
                config.Initialize(this);
            }
        }

        public void LoadConfigs()
        {
            if (!TryLoad())
            {
                SaveConfigs();
            }

            InitializeCofigs();
        }

        public T GetValue<T>(string category, string key, T defaultValue)
        {
            if (!_configsData.TryGetValue(category, out var categoryData)
                || !categoryData.TryGetValue(key, out var fieldValue))
            {
                return defaultValue;
            }

            try
            {
                if (typeof(T).IsEnum)
                {
                    return (T)Enum.Parse(typeof(T), fieldValue);
                }

				if (CustomConverter.TryParse(fieldValue, out T result))
				{
                    return result;
				}

                return (T)Convert.ChangeType(fieldValue, typeof(T));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to convert '{fieldValue}' to {typeof(T)}: {ex.Message}");
            }

            return defaultValue;
        }

        public void SaveConfigs()
        {
            StringBuilder builder = new StringBuilder();

            foreach (var config in _registeredConfigs)
            {
                builder.AppendLine($"[{config.Category}]");

                foreach (var kvp in config.GetConfigValues())
                {
                    builder.AppendLine($"{kvp.Key}={kvp.Value}");
                }

                builder.AppendLine();
            }

            File.WriteAllText(ConfigPath, builder.ToString());
        }

        private bool TryLoad()
        {
            if (!File.Exists(ConfigPath))
            {
                return false;
            }

            var configFile = File.ReadAllLines(ConfigPath);
            var currentCategory = "";

            foreach (var line in configFile)
            {
                var currentLine = line.Trim();

                if (string.IsNullOrEmpty(currentLine) || currentLine.StartsWith("#"))
                {
                    continue;
                }

                if (currentLine.StartsWith('[') && currentLine.EndsWith(']'))
                {
                    currentCategory = currentLine.Trim('[', ']');

                    if (!_configsData.ContainsKey(currentCategory))
                    {
                        _configsData[currentCategory] = new();
                    }
                }
                else if (!string.IsNullOrEmpty(currentCategory) && currentLine.Contains('='))
                {
                    string[] parts = currentLine.Split('=', 2);
                    string key = parts[0].Trim();
                    string value = parts[1].Trim();
                    _configsData[currentCategory][key] = value;
                }
            }

            return true;
        }
    }
}