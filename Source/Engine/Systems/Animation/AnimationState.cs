using SFML.Graphics;

namespace Source.Engine.Systems.Animation
{
	public class AnimationState
	{
		public string Name { get; }
		public bool Loop { get; }

		private readonly List<Texture> _frames;
		private readonly float _frameDuration;

		private int _currentFrameIndex;
		private float _elapsedTime;

		public AnimationState(string name, List<Texture> frames, float frameDuration, bool loop = true)
		{
			Name = name;
			Loop = loop;
			_frames = frames;
			_frameDuration = frameDuration;
		}

		public void Update(float deltaTime)
		{
			_elapsedTime += deltaTime;

			if (_frames.Count <= 0)
			{
				return;
			}

			if (_elapsedTime >= _frameDuration)
			{
				_elapsedTime -= _frameDuration;

				if (Loop || _currentFrameIndex < _frames.Count - 1)
				{
					_currentFrameIndex = (_currentFrameIndex + 1) % _frames.Count;
				}
			}
		}

		public Texture? GetCurrentFrame()
		{
			if (_frames.Count <= 0)
			{
				return null;
			}

			return _frames[_currentFrameIndex];
		}

		public void Reset()
		{
			_currentFrameIndex = 0;
			_elapsedTime = 0;
		}
	}
}