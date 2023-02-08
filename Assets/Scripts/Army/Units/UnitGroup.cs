using System.Collections.Generic;
using AI.GPUFlock;
using UnityEngine;

namespace Army.Units
{
	public class UnitGroup:IUnitGroupSerializer
	{
		private GPUUnitDraw[] _unitBuffer;
		public List<Unit> List{get;} = new List<Unit>();

		public UnitGroup()
		{
			//FlocksHandler.Instance.AddUnitBufferHandler(this);
		}
		public GPUUnitDraw[] Serialize()
		{
			_unitBuffer = new GPUUnitDraw[List.Count];
			for(var i = 0; i < List.Count; i++)
			{
				_unitBuffer[i].Position = List[i].Position;
				_unitBuffer[i].Direction = List[i].Direction;
				_unitBuffer[i].TargetPos = List[i].TargetPos;
				_unitBuffer[i].Noise_Offset = 1f;
				_unitBuffer[i].Team = List[i].Team;
			}
			return _unitBuffer;
		}

		public void Deserialize(GPUUnitDraw[] buffer)
		{
			_unitBuffer = buffer;
			for(var i = 0; i < buffer.Length; i++)
			{
				List[i].Position = _unitBuffer[i].Position;
				List[i].Direction = _unitBuffer[i].Direction;
				List[i].TargetIndex = _unitBuffer[i].TargetedUnit;
			}
		}
		public Vector2[] GetPositions()
		{
			var positions = new Vector2[List.Count];
			for(var i = 0; i < List.Count; i++)
			{
				positions[i] = List[i].Position;
			}
			return positions;
		}
		public void AddUnit(Unit unit)
		{
			List.Add(unit);
			unit.OnUnitKilled += RemoveUnit;

			void RemoveUnit()
			{
				List.Remove(unit);
				unit.OnUnitKilled -= RemoveUnit;
			}
		}
	}
}