using SFML.Graphics;
using Source.Engine.Tools;

namespace Source.Engine.Systems.Animation
{
    public static class TextureLoader
    {
		private static readonly Dictionary<string, Texture> _loadedTextures = new();
		private static readonly Dictionary<string, List<Texture>> _loadedSpritesheets = new();

		public static Texture? GetTexture(string texturePath)
		{
			if (_loadedTextures.TryGetValue(texturePath, out var texture))
			{
				return texture;
			}

			texture = LoadTexture(texturePath);
			if (texture != null)
			{
				_loadedTextures[texturePath] = texture;
			}

			return texture;
		}

		public static List<Texture> GetSpritesheetTextures(string spritesheetPath)
		{
			if (_loadedSpritesheets.TryGetValue(spritesheetPath, out var textures))
			{
				return textures;
			}

			var loadedTextures = LoadSpritesheet(spritesheetPath);

			if (loadedTextures != null)
			{
				_loadedSpritesheets[spritesheetPath] = loadedTextures;
				return loadedTextures;
			}

			return new();
		}

		private static List<Texture> LoadSpritesheet(string spritesheetPath)
		{
			var path = Path.Combine(PathHelper.ResourcesPath, spritesheetPath);
			var textures = new List<Texture>();

			try
			{
				var files = Directory.GetFiles(path);

				foreach (var file in files)
				{
					textures.Add(new Texture(file));
				}
			}
			catch (Exception e)
			{
				Debug.LogError(e.Message);
			}

			return textures;
		}

		private static Texture? LoadTexture(string spritePath)
        {
			Texture? texture = null;

            try
            {
				var path = Path.Combine(PathHelper.ResourcesPath, spritePath);

				if (!File.Exists(path))
				{
					Debug.LogWarning($"Texture not found: {path}");					
				}

				texture = new(path);
			}
            catch (Exception e)
            {
				Debug.LogError($"Failed to load texture {spritePath}: {e.Message}");
			}

			return texture;
        }
    }
}