using System.Collections.Generic;
using AI.GPUFlock;

namespace Army.Units
{
	public class AllUnits
	{
		private static List<UnitGroup> _unitGroups = new List<UnitGroup>();

		public static Unit[] Units;
		private static int[] _bufferSizes;
		private static GPUUnitDraw[] _unitsBuffer;
		private static GPUUnitGroup[] _groupsBuffer;

		static AllUnits()
		{
			FlocksHandler.Serializer = new AllUnits();
		}
		public static void AddUnitGroup(UnitGroup unitGroup)
		{
			_unitGroups.Add(unitGroup);
		}
		public static void RemoveUnitGroup(UnitGroup unitGroup)
		{
			_unitGroups.Remove(unitGroup);
		}
		private static void SerialiseUnits()
		{
			var unitGroups = new Unit[_unitGroups.Count][];
			_bufferSizes = new int[_unitGroups.Count];
			

			for(var i = 0; i < _unitGroups.Count; i++)
			{
				unitGroups[i] = _unitGroups[i].Units.ToArray();
				//_unitsBuffer[i].GroupId = (uint)i;
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
			_unitsBuffer = new GPUUnitDraw[bufferSize];
			int pointer = 0;
			//concat all buffers
			for(var i = 0; i < unitGroups.Length; i++)
			{
				var buffer = unitGroups[i];
				foreach(var unit in buffer)
				{
					unit.UnitGroupIndex = (uint)i;
				}
				buffer.CopyTo(Units, pointer);
				pointer += buffer.Length;
			}

			for(var i = 0; i < Units.Length; i++)
			{
				_unitsBuffer[i].Position = Units[i].Position;
				_unitsBuffer[i].Direction = Units[i].Direction;
				_unitsBuffer[i].TargetPos = Units[i].TargetPos;
				_unitsBuffer[i].GroupId = Units[i].UnitGroupIndex;
			}
		}
		private static void DeserialiseUnits()
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

		private static void SerialiseGroups()
		{
			_groupsBuffer = new GPUUnitGroup[_unitGroups.Count];
			for(var i = 0; i < _unitGroups.Count; i++)
			{
				_groupsBuffer[i] = _unitGroups[i].GetGPUUnitGroup();
			}
		}

		public GPUUnitGroup[] GetGroups()
		{
			SerialiseGroups();
			return _groupsBuffer;
		}

		public GPUUnitDraw[] GetUnits()
		{
			SerialiseUnits();
			return _unitsBuffer;
		}

		public void DeserializeUnits(GPUUnitDraw[] buffer)
		{
			_unitsBuffer = buffer;
			for(var i = 0; i < buffer.Length; i++)
			{
				Units[i].Position = _unitsBuffer[i].Position;
				Units[i].Direction = _unitsBuffer[i].Direction;
				Units[i].TargetIndex = _unitsBuffer[i].TargetedUnit;
			}
			DeserialiseUnits();
		}
	}
}