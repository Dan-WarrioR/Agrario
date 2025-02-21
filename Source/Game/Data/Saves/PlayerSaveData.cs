namespace Source.Game.Data.Saves
{
	public class PlayerSaveData
	{
		private int _skinsCount = 3;

		public int SkinIndex
		{
			get
			{
				return _skinIndex;
			}
			set
			{
				_skinIndex = Math.Clamp(value, 0, _skinsCount);
			}
		}
		private int _skinIndex;
	}
}