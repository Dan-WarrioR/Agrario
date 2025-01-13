using SFML.Graphics;
using SFML.System;

public class GameCamera
{
	private View _gameView;
	private View _uiView;
	private RenderWindow _window;

	public GameCamera(RenderWindow window, Vector2f initialCenter, Vector2f size)
	{
		_window = window;

		// Initialize game view (follows player)
		_gameView = new View(initialCenter, size);

		// Initialize UI view (stays fixed)
		_uiView = new View(_window.DefaultView);
		_uiView.Size = _window.DefaultView.Size;
		_uiView.Center = new Vector2f(_uiView.Size.X / 2, _uiView.Size.Y / 2);
	}

	public void BeginGameView()
	{
		_window.SetView(_gameView);
	}

	public void BeginUIView()
	{
		_window.SetView(_uiView);
	}

	public void Update(Vector2f playerPosition)
	{
		_gameView.Center = playerPosition;
	}

	public void Zoom(float factor)
	{
		_gameView.Zoom(factor);
	}

	public void SetViewSize(Vector2f size)
	{
		_gameView.Size = size;
	}

	public Vector2f GetCameraPosition()
	{
		return _gameView.Center;
	}

	// Handle window resize if needed
	public void OnWindowResize(uint width, uint height)
	{
		_uiView.Size = new Vector2f(width, height);
		_uiView.Center = new Vector2f(width / 2, height / 2);
	}
}