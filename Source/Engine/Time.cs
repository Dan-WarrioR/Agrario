using SFML.System;

namespace Source.Engine
{
	public class Time
	{
		public float DeltaTime { get; private set; } = 0f;

		public float BeforeUpdateTime { get; private set; } = 0f;

		private Clock _clock;

		private float _previousTimeElapsed = 0f;

		public Time()
		{
			_clock = new();
		}

		public void Update()
		{
			var currentTime = _clock.ElapsedTime.AsSeconds();

			DeltaTime = currentTime - _previousTimeElapsed;

			_previousTimeElapsed = currentTime;

			BeforeUpdateTime += DeltaTime;
		}

		public void Reset()
		{
			BeforeUpdateTime = 0f;
		}
	}
}
