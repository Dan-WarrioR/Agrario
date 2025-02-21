using Source.Engine.Tools;
using Newtonsoft.Json;
using Source.Engine.Tools.ProjectUtilities;

namespace Source.Game.Features.SaveSystem
{
    public class SaveService //Dummy Save & Load system
    {
        private const string GameSettignsPath = "GameSettings.{0}";
		private static readonly string saveDirectory = Path.Combine(PathHelper.ProjectPath, "Saves");

		public SaveService()
        {
            Dependency.Register(this);

			if (!Directory.Exists(saveDirectory))
			{
				Directory.CreateDirectory(saveDirectory);
			}
		}

        ~SaveService()
        {
            Dependency.Unregister(this);
        }

        public T Load<T>() where T : new()
        {
            var filePath = GetPath<T>();

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);

                var file = JsonConvert.DeserializeObject<T>(json);

                if (file != null)
                {
                    return file;
                }
            }

            Debug.Log($"File {filePath} not found. Creating default one!");

            var data = new T();

            Save(data);

            return data;
        }

        public void Save<T>(T data)
        {
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);

            var filePath = GetPath<T>();

            File.WriteAllText(filePath, json);
        }

        private string GetPath<T>()
        {
            var dataName = string.Format(GameSettignsPath, typeof(T).Name);

			return Path.Combine(saveDirectory, dataName); 
        }
    }
}