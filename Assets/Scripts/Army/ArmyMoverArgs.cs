using System.Collections.Generic;
using Army.Units;
using DefaultNamespace.Enums;
using Formations.Scripts;
using Stats;

namespace Army
{
	public struct ArmyMoverArgs
	{
		public readonly UnitGroup Units ;
		public readonly StatDict<ArmyStat> Stats;
		public readonly ArmyKDTree ArmyKDTree;
		public readonly float ArmyRadius;
		public readonly FormationBase Formation;
		
		public uint Team { get; set; }
		public ArmyMoverArgs(UnitGroup units, StatDict<ArmyStat> stats, ArmyKDTree armyKDTree, float armyRadius, FormationBase formation)
		{
			Units = units;
			Stats = stats;
			ArmyKDTree = armyKDTree;
			ArmyRadius = armyRadius;
			Formation = formation;
			Team = 0;
		}
	}
}