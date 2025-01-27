using SFML.Graphics;
using Source.Engine.Tools;

namespace Source.Engine.GameObjects.Components
{
	public class Animator : BaseComponent
	{
		private string _spritePath;

		private List<Texture> _frames = new();
		private ShapeObject _target;

		private float _frameDuration;

		private int _currentFrameIndex;
		private float _elapsedTime;

		public void Initialize(string spritePath, float frameDuretion)
		{
			_spritePath = spritePath;
			_frameDuration = frameDuretion;
		}

		public override void Start()
		{
			_target = (ShapeObject)Owner;

			SetupFrames();
		}
	
		public override void Update(float deltaTime)
		{
			if (_frames.Count <= 1)
			{
				return;
			}

			_elapsedTime += deltaTime;

			if (_elapsedTime < _frameDuration)
			{
				return;
			}

			_elapsedTime -= _frameDuration;

			SetFrame();
		}

		private void SetFrame()
		{
			_currentFrameIndex = (_currentFrameIndex + 1) % _frames.Count;
			_target.SetTexture(_frames[_currentFrameIndex]);
		}

		private void SetupFrames()
		{
			var files = TryLoadFrames();

			if (files == null || files.Length <= 0)
			{
				return;
			}

			foreach (var file in files)
			{
				_frames.Add(new Texture(file));
			}

			_target.SetTexture(_frames[0]);
		}

		private string[]? TryLoadFrames()
		{
			var path = Path.Combine(PathHelper.ResourcesPath, _spritePath);
			
			try
			{
				return Directory.GetFiles(path);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}

			return null;
		}
	}
}