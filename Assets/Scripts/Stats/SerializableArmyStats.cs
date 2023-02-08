using System;
using System.Collections.Generic;
using DefaultNamespace.Enums;
using Stats.Structures;
using UnityEditor;
using UnityEngine;

namespace Stats
{
	[Serializable]
	public class SerializableArmyStats
	{
		[Serializable]
		public struct EnumeratedStat
		{
			public ArmyStat Name;
			public Stat Value;
		}
		[SerializeField] private List<EnumeratedStat> enumeratedStats;
		public StatDict<ArmyStat> GetStats()
		{
			var stats = new StatDict<ArmyStat>();
			foreach (var stat in enumeratedStats)
			{
				stats.SetStat(stat.Name, stat.Value);
			}
			return stats;
		}
	}
}