using System;
using System.Collections.Generic;
using Datastructures;
using DefaultNamespace.AI.Unit;
using UnityEngine;

namespace AI.Unit.Context
{
	public class FollowPointProvider : ContextProvider
	{

		private List<Vector2> _context = new List<Vector2>(1);
		private Tracked<Vector2> _pointTracked;
		public FollowPointProvider(Tracked<Vector2> pointTracked)
		{
			_pointTracked = pointTracked;
			_context.Add(Vector2.zero);
		}

		public override List<Vector2> GetContext(Vector2 agentPos, FlockParameters parameters)
		{
			_context[0] = _pointTracked.Value;
			return _context;
		}
	}
}