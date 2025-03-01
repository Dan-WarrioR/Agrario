namespace Source.SeaBattle.LevelMap
{  
	public enum BombState
	{
		None,
		Shot,
		Miss,
	}

    public class Cell
    {
		public bool IsShip { get; private set; } = false;
		public bool IsDestroyed { get; private set; } = false;

		public bool IsScaned { get; set; } = false;

		public event Action<Cell> OnBombed;

		public void PlaceShip()
		{
			IsShip = true;
		}

		public BombState BombCell()
		{
			if (IsDestroyed)
			{
				return BombState.None;
			}

			IsDestroyed = true;

			OnBombed?.Invoke(this);

			return IsShip ? BombState.Shot : BombState.Miss;
		}

		public bool CanBombCell()
		{
			return !IsDestroyed;
		}
	}
}