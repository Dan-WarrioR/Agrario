using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Source.Game.Units;

namespace Source.Game
{
	public class GameCamera
	{
		private static bool IsDebugFactor = true;
		private static float DebugFactor = 1.5f;

		private View _gameView;
		private View _uiView;
		private RenderWindow _window;

		private Player _player;

		public GameCamera(RenderWindow window, Vector2f initialCenter, Vector2f size)
		{
			_window = window;

			_window.Resized += OnWindowResize;

			_gameView = new(initialCenter, size);

			_uiView = new(_window.DefaultView);
			_uiView.Size = _window.DefaultView.Size;
			_uiView.Center = new(_uiView.Size.X / 2, _uiView.Size.Y / 2);

			BeginGameView();
		}

		public void BeginGameView()
		{
			_window.SetView(_gameView);
		}

		public void BeginUIView()
		{
			_window.SetView(_uiView);
		}

		public void Update()
		{
			_gameView.Center = _player.Position;
		}

		public void Zoom(float factor)
		{
			//_gameView.Zoom(factor); //zoom change view on factor every method invoke. Need to set a value once

			factor = !IsDebugFactor ? factor : DebugFactor;

			_gameView.Size = new(_uiView.Size.X * factor, _uiView.Size.Y * factor);
		}

		public void SetViewSize(Vector2f size)
		{
			_gameView.Size = size;
		}

		public void SetFollowTarget(Player player)
		{
			_player = player;
		}

		private void OnWindowResize(object? sender, SizeEventArgs e)
		{
			uint width = _window.Size.X;
			uint height = _window.Size.Y;

			_uiView.Size = new(width, height);
			_uiView.Center = new(width / 2, height / 2);

			float aspectRatio = width / (float)height;
			_gameView.Size = new(_gameView.Size.Y * aspectRatio, _gameView.Size.Y);
		}
	}
}