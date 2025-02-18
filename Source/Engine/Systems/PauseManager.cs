using Source.Engine.Tools;

namespace Source.Engine.Systems
{
	public interface IPauseHandler
	{
		public void SetPaused(bool isPaused);
	}

	public class PauseManager : IPauseHandler
	{
		private readonly List<IPauseHandler> _handlers = new();

		public PauseManager()
		{
			Dependency.Register(this);
		}

		public bool IsPaused { get; private set; } = false;

		public void Register(IPauseHandler handler)
		{
			_handlers.Add(handler);
		}

		public void Unregister(IPauseHandler handler)
		{
			_handlers.Remove(handler);
		}

		public void Switch()
		{
			SetPaused(!IsPaused);
		}

		public void SetPaused(bool isPaused)
		{
			IsPaused = isPaused;

			foreach (var handler in _handlers)
			{
				handler.SetPaused(isPaused);
			}
		}
	}
}