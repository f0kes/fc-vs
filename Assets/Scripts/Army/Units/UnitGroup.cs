using System.Collections.Generic;
using AI.GPUFlock;
using UnityEngine;

namespace Army.Units
{
	public class UnitGroup : IUnitGroupSerializer
	{
		private GPUUnitDraw[] _unitBuffer;
		public List<Unit> Units{get; private set;} = new List<Unit>();


		public UnitGroup()
		{
			AllUnits.AddUnitGroup(this);
		}
		~UnitGroup()
		{
			AllUnits.RemoveUnitGroup(this);
		}
		public GPUUnitDraw[] Serialize()
		{
			_unitBuffer = new GPUUnitDraw[Units.Count];
			for(var i = 0; i < Units.Count; i++)
			{
				_unitBuffer[i].Position = Units[i].Position;
				_unitBuffer[i].Direction = Units[i].Direction;
				_unitBuffer[i].TargetPos = Units[i].TargetPos;
				_unitBuffer[i].Noise_Offset = 1f;
				_unitBuffer[i].Team = Units[i].Team;
			}
			return _unitBuffer;
		}

		public void Deserialize(GPUUnitDraw[] buffer)
		{
			_unitBuffer = buffer;
			for(var i = 0; i < buffer.Length; i++)
			{
				Units[i].Position = _unitBuffer[i].Position;
				Units[i].Direction = _unitBuffer[i].Direction;
				Units[i].TargetIndex = _unitBuffer[i].TargetedUnit;
			}
		}
		public Vector2[] GetPositions()
		{
			var positions = new Vector2[Units.Count];
			for(var i = 0; i < Units.Count; i++)
			{
				positions[i] = Units[i].Position;
			}
			return positions;
		}
		public void AddUnit(Unit unit)
		{
			Units.Add(unit);
			unit.OnUnitKilled += RemoveUnit;

			void RemoveUnit()
			{
				Units.Remove(unit);
				unit.OnUnitKilled -= RemoveUnit;
			}
		}
	}
}