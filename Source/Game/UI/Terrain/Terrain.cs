using SFML.Graphics;
using Source.Engine.Configs;
using Source.Engine.GameObjects;
using Source.Engine.Systems.Tools.Animations;
using Source.Engine.Tools;
using Source.Engine.Tools.ProjectUtilities;

namespace Source.Game.UI.Terrain
{
	public class Terrain : ShapeObject
	{
		private static TextureLoader TextureLoader => _textureLoader ??= Dependency.Get<TextureLoader>();
		private static TextureLoader _textureLoader;
		
		private Sprite _sprite;

		private FloatRect _windowSize;

		private string _terrainPath;

		public void Initialize(string terrainPath)
		{
			_windowSize = WindowConfig.Bounds;

			_terrainPath = terrainPath;
		}

		public void LoadTerrain()
		{
			string fullPath = Path.Combine(PathHelper.ResourcesPath, _terrainPath);
			var texture = TextureLoader.GetTexture(fullPath);

			if (texture == null)
			{
				return;
			}

			texture.Repeated = true;

			int width = (int)_windowSize.Width;
			int height = (int)_windowSize.Height;

			var textureRect = new IntRect(0, 0, width, height);

			_sprite = new Sprite(texture, textureRect);
		}

		public override void Draw(RenderTarget target, RenderStates states)
		{
			_sprite?.Draw(target, states);
		}
	}
}