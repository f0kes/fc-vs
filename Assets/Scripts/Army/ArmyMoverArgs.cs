using System.Collections.Generic;
using DefaultNamespace.Enums;
using Formations.Scripts;
using Stats;

namespace Army
{
	public struct ArmyMoverArgs
	{
		public readonly List<Unit> Units ;
		public readonly StatDict<ArmyStat> Stats;
		public readonly ArmyKDTree ArmyKDTree;
		public readonly float ArmyRadius;
		public readonly FormationBase Formation;
		public ArmyMoverArgs(List<Unit> units, StatDict<ArmyStat> stats, ArmyKDTree armyKDTree, float armyRadius, FormationBase formation)
		{
			Units = units;
			Stats = stats;
			ArmyKDTree = armyKDTree;
			ArmyRadius = armyRadius;
			Formation = formation;
		}
	}
}