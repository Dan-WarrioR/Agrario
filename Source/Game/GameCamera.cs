﻿using SFML.Graphics;
using SFML.System;
using SFML.Window;

public class GameCamera
{
	private View _gameView;
	private View _uiView;
	private RenderWindow _window;

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