using System.Collections.Generic;
using AI.GPUFlock;
using GameState;

namespace Army
{
	public class UnitGroup : IUnitBufferHandler
	{
		private static UnitGroup _rootGroup;
		
		private Unit[] _units;
		private UnitGroup[] _subGroups;
		
		public UnitGroup()
		{
			Ticker.OnTick += OnTick;
		}

		private static void OnTick(Ticker.OnTickEventArgs obj)
		{
		}

		public GPUUnitDraw[] GetBuffer()
		{
			throw new System.NotImplementedException();
		}

		public void SetBuffer(GPUUnitDraw[] buffer)
		{
			throw new System.NotImplementedException();
		}
	}
}