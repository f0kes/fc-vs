using System;
using System.Collections.Generic;
using AI.GPUFlock;
using Army.Units.UnitEventArgs;
using DefaultNamespace.Enums;
using Stats;
using UnityEngine;

namespace Army.Units
{
	public class UnitGroup : IUnitGroupSerializer
	{
		private GPUUnitDraw[] _unitBuffer;
		public List<Unit> Units{get; private set;} = new List<Unit>();
		private StatDict<ArmyStat> Stats{get; set;}
		public uint Team{get; set;}
		public event Action<Unit> OnUnitAdded;
		public event Action<Unit> OnUnitRemoved; 
		
		
		public static UnitGroup FromGroups(List<UnitGroup> groups)
		{
			var unitGroup = new UnitGroup(groups[0].Stats, groups[0].Team);
			foreach(var group in groups)
			{
				foreach(var unit in group.Units)
				{
					unitGroup.AddUnit(unit);
				}
			}
			return unitGroup;
		}

		public UnitGroup(StatDict<ArmyStat> stats, uint team)
		{
			Stats = stats;
			Team = team;
			AllUnits.AddUnitGroup(this);
		}
		~UnitGroup()
		{
			AllUnits.RemoveUnitGroup(this);
		}

		public GPUUnitGroup GetGPUUnitGroup()
		{
			return new GPUUnitGroup
			{
				Speed = Stats[ArmyStat.Speed],
				Team = Team,
				AttackRange = Stats[ArmyStat.AttackRange]
			};
		}

		public GPUUnitDraw[] Serialize()
		{
			_unitBuffer = new GPUUnitDraw[Units.Count];
			for(var i = 0; i < Units.Count; i++)
			{
				_unitBuffer[i].Position = Units[i].Position;
				_unitBuffer[i].Direction = Units[i].Direction;
				_unitBuffer[i].TargetPos = Units[i].TargetPos;
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
			unit.Group = this;
			unit.OnUnitKilled += RemoveUnit;
			OnUnitAdded?.Invoke(unit);

			void RemoveUnit(object sender, UnitKilledEventArgs eventArgs)
			{
				
				Units.Remove(unit);
				unit.OnUnitKilled -= RemoveUnit;
				OnUnitRemoved?.Invoke(unit);
			}
		}
	}
}