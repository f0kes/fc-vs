using System.Collections.Generic;
using UnityEngine;

namespace Formations.Scripts
{
	public class PointFormation : FormationBase
	{

		public override IEnumerable<Vector2> EvaluatePoints(int unitCount)
		{
			return new Vector2[unitCount];
		}
	}
}