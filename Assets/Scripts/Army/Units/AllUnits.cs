using System.Collections.Generic;
using AI.GPUFlock;

namespace Army.Units
{
	public class AllUnits : IUnitGroupSerializer
	{
		private static List<UnitGroup> _unitGroups = new List<UnitGroup>();

		public static Unit[] Units;
		private static int[] _bufferSizes;
		private static GPUUnitDraw[] _unitsBuffer;
		
		static AllUnits()
		{
			FlocksHandler.Serializers.Add(new AllUnits());
		}
		public static void AddUnitGroup(UnitGroup unitGroup)
		{
			_unitGroups.Add(unitGroup);
		}
		public static void RemoveUnitGroup(UnitGroup unitGroup)
		{
			_unitGroups.Remove(unitGroup);
		}
		private static void UpdateUnits()
		{
			var unitGroups = new Unit[_unitGroups.Count][];
			_bufferSizes = new int[_unitGroups.Count];

			for(var i = 0; i < _unitGroups.Count; i++)
			{
				unitGroups[i] = _unitGroups[i].Units.ToArray();
			}

			//get all buffer sizes sum
			var bufferSize = 0;
			for(var i = 0; i < unitGroups.Length; i++)
			{
				var length = unitGroups[i].Length;
				_bufferSizes[i] = length;
				bufferSize += length;
			}

			Units = new Unit[bufferSize];
			int pointer = 0;
			//concat all buffers
			foreach(var buffer in unitGroups)
			{
				buffer.CopyTo(Units, pointer);
				pointer += buffer.Length;
			}
		}
		private static void UpdateGroups()
		{
			var pointer = 0;
			for(var i = 0; i < _unitGroups.Count; i++)
			{
				var group = _unitGroups[i];
				var buffer = new Unit[_bufferSizes[i]];
				System.Array.Copy(Units, pointer, buffer, 0, buffer.Length);
				group.Units.Clear();
				group.Units.AddRange(buffer);
				pointer += buffer.Length;
			}
		}
		public GPUUnitDraw[] Serialize()
		{
			UpdateUnits();
			_unitsBuffer = new GPUUnitDraw[Units.Length];
			for(var i = 0; i < Units.Length; i++)
			{
				_unitsBuffer[i].Position = Units[i].Position;
				_unitsBuffer[i].Direction = Units[i].Direction;
				_unitsBuffer[i].TargetPos = Units[i].TargetPos;
				_unitsBuffer[i].Noise_Offset = 1f;
				_unitsBuffer[i].Team = Units[i].Team;
			}
			return _unitsBuffer;
		}

		public void Deserialize(GPUUnitDraw[] buffer)
		{
			_unitsBuffer = buffer;
			for(var i = 0; i < buffer.Length; i++)
			{
				Units[i].Position = _unitsBuffer[i].Position;
				Units[i].Direction = _unitsBuffer[i].Direction;
				Units[i].TargetIndex = _unitsBuffer[i].TargetedUnit;
			}
			UpdateGroups();
		}
	}
}