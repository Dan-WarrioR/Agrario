using SFML.Graphics;
using Source.Engine.Tools;

namespace Source.Engine.Systems.Animation
{
    public static class AnimationLoader
    {
        private static readonly Dictionary<string, List<Texture>> _loadedSprites = new();

        public static List<Texture> GetTextures(string spritePath)
        {
            if (_loadedSprites.TryGetValue(spritePath, out var textures))
            {
                return textures;
            }

            var loadedTextures = LoadTextures(spritePath);

            if (loadedTextures != null)
            {
                _loadedSprites[spritePath] = loadedTextures;
                return loadedTextures;
            }

            return new();
        }

        private static List<Texture> LoadTextures(string spritePath)
        {
            var path = Path.Combine(PathHelper.ResourcesPath, spritePath);
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
    }
}