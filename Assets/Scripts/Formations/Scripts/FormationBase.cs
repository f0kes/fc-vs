using System.Collections.Generic;
using UnityEngine;

namespace Formations.Scripts
{
	public abstract class FormationBase
	{
		[SerializeField] [Range(0, 1)] protected float _noise = 0;

		public abstract IEnumerable<Vector2> EvaluatePoints(int unitCount);

		public Vector2 GetNoise(Vector2 pos)
		{
			var noise = Mathf.PerlinNoise(pos.x * _noise, pos.y * _noise);

			return new Vector2(noise, noise);
		}
	}
}