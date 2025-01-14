﻿using Source.Game.Units;
using SFML.Graphics;
using SFML.System;
using Source.Engine.GameObjects;
using Source.Game.Units.Components.Input;
using Source.Game.Configs;
using Source.Engine;

namespace Source.Game.Factories
{
    public class UnitFactory
    {
        private const float MinPlayerRadius = 20f;
        private const float FoodRadius = 5f;

        private static readonly Color BotFillColor = new(150, 150, 150);
        private static readonly Color PlayerFillColor = Color.Yellow;

        private GameLoop _gameLoop;
        private BaseRenderer _renderer;

        public UnitFactory(GameLoop gameLoop, BaseRenderer renderer)
        {
            _gameLoop = gameLoop;
            _renderer = renderer;
        }

        public Player CreatePlayer(bool isAi)
        {
            IInputComponent inputComponent = isAi ? new RandomDirectionInput() : new KeyBoardInput();
            var color = isAi ? new(150, 150, 150) : Color.Yellow;

            var player = new Player(inputComponent, color, MinPlayerRadius, GetRandomPosition());

            RegisterObject(player);

            return player;
        }

        public Food CreateFood()
        {
            var food = new Food(FoodRadius, GetRandomPosition());

            RegisterObject(food);

            return food;
        }

        public TextObject CreateText(string text)
        {
            var textObject = new TextObject(text, new(WindowConfig.Bounds.Width - 200, WindowConfig.Bounds.Top + 20));

            _renderer.AddGameElement(textObject);

            textObject.OnDisposed += _renderer.RemoveGameElement;

            return textObject;
        }

        private Vector2f GetRandomPosition()
        {
            var bounds = WindowConfig.Bounds;

            float x = Random.Shared.Next((int)bounds.Left, (int)(bounds.Left + bounds.Width));
            float y = Random.Shared.Next((int)bounds.Top, (int)(bounds.Top + bounds.Height));

            return new(x, y);
        }

        private void RegisterObject(GameObject gameObject)
        {
            _gameLoop.Register(gameObject);
            _renderer.AddGameElement(gameObject);

            gameObject.OnDisposed += UnregisterObject;
        }

        private void UnregisterObject(GameObject gameObject)
        {
            gameObject.OnDisposed -= UnregisterObject;

            _renderer.RemoveGameElement(gameObject);
            _gameLoop.UnRegister(gameObject);
        }
    }
}