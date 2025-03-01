using SFML.Graphics;
using Source.Engine.Configs;
using Source.Engine.GameObjects;
using Source.Engine.Tools;

namespace Source.Engine.Rendering
{
	public interface IUIElement
	{

	}

	public class BaseRenderer
	{
		//////////////////////////////////////////////////
		
		#region Data
		
		public GameCamera Camera { get; private set; }
		
		protected List<GameObject> GameElements = new();

		protected List<GameObject> UIElements = new();

		protected RenderWindow Window { get; private set; }
		
		private Color _backgroundColor;

		internal void Initialize(RenderWindow window)
		{
			Dependency.Register(this);

			Window = window;
			_backgroundColor = WindowConfig.BackgroundColor;
			Camera = new(window, WindowConfig.WindowSize, WindowConfig.WindowSize);
		}

		~BaseRenderer()
		{
			Dependency.Unregister(this);
		}
		
		#endregion
		
		//////////////////////////////////////////////////

		#region Interfaces
		
		public virtual void Render()
		{
			Window.Clear(_backgroundColor);
			Camera.Update();

			Camera.BeginGameView();
			DrawActiveElements(GameElements);

			Camera.BeginUIView();
			DrawActiveElements(UIElements);

			Window.Display();
		}
		
		public void AddRenderElement(GameObject gameObject)
		{
			if (gameObject is IUIElement)
			{
				if (!UIElements.Contains(gameObject))
				{
					UIElements.Add(gameObject);
				}

				return;
			}

			if (!GameElements.Contains(gameObject))
			{
				GameElements.Add(gameObject);
			}
		}

		public void RemoveGameElement(GameObject gameObject)
		{
			if (gameObject is IUIElement)
			{
				UIElements.Remove(gameObject);
				return;
			}

			GameElements.Remove(gameObject);
		}
		
		public void Zoom(float factor)
		{
			Camera.Zoom(factor);
		}
		
		protected void DrawActiveElements<T>(IEnumerable<T> objects) where T : Drawable, IActivable
		{
			foreach (var obj in objects)
			{
				if (obj.IsActive)
				{
					Window.Draw(obj);
				}
			}
		}
		
		#endregion
		
		//////////////////////////////////////////////////

		#region Private Implementation

		

		#endregion
		
		//////////////////////////////////////////////////
	}
}
