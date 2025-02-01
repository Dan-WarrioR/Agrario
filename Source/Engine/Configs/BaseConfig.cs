namespace Source.Engine.Configs
{
    public abstract class BaseConfig
	{
		public abstract string Category { get; }

		public abstract void Initialize(ConfigManager manager);

		public abstract Dictionary<string, object> GetConfigValues();
	}
}