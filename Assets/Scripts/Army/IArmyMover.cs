using System.Collections.Generic;
using DefaultNamespace.Enums;
using Formations.Scripts;
using Stats;
using UnityEngine;

namespace Army
{
	public interface IArmyMover
	{
		void Init(ArmyMoverArgs args);
		void SetTarget(Vector2 dir);

		void MoveInRadius(Vector2 point);
	}
}