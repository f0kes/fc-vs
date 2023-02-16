using System.Collections.Generic;
using Army.Units;
using UnityEngine;

namespace DefaultNamespace
{
	public static class Helpers
	{
		public static Vector2[] GetUnitPositions(this List<Unit> units)
		{
			var positions = new Vector2[units.Count];
			for(var i = 0; i < units.Count; i++)
			{
				positions[i] = units[i].Position;
			}
			return positions;
		}
		public static float CalculateArmyRange(int initialUnits, float density)
		{
			return Mathf.Sqrt(initialUnits / density);
		}
		public static Vector2[] GetCircleGridPositions(float radius, int total)
		{
			var density = total / (Mathf.PI * radius * radius);
			var positions = GetCircleGridPositionsBase(radius, density, total);
			return positions;
		}
		public static Vector2[] GetCircleGridPositionsDensity(float density, int total)
		{
			var radius = Mathf.Sqrt(total / density);
			var positions = GetCircleGridPositionsBase(radius, density, total);
			return positions;
		}
		private static Vector2[] GetCircleGridPositionsBase(float radius, float density, int total)
		{
			var positions = new Vector2[total];
			//use total to calculate radius
			var circlePositions = RasteriseCircleBruteForce(total, density);
			return circlePositions;
		}
		private static Vector2[] RastriseCircleBresenham(int radius)
		{
			var octantPositions = new List<Vector2>();
			var x = 0;
			var y = radius;
			var d = 3 - 2 * radius;

			while (y >= x)
			{
				x++;
				if(d > 0)
				{
					y--;
					d = d + 4 * (x - y) + 10;
				}
				else
				{
					d = d + 4 * x + 6;
				}
				octantPositions.Add(new Vector2(x, y));
			}

			var count = octantPositions.Count;
			var positions = new Vector2[count * 8];

			var index = 0;
			foreach(var position in octantPositions)
			{
				positions[index] = new Vector2(position.x, position.y);
				positions[index + 1] = new Vector2(position.y, position.x);
				positions[index + 2] = new Vector2(-position.x, position.y);
				positions[index + 3] = new Vector2(-position.y, position.x);
				positions[index + 4] = new Vector2(-position.x, -position.y);
				positions[index + 5] = new Vector2(-position.y, -position.x);
				positions[index + 6] = new Vector2(position.x, -position.y);
				positions[index + 7] = new Vector2(position.y, -position.x);
				index += 8;
			}

			return positions;
		}
		private static Vector2[] RasteriseCircleBruteForce(int totalPixels, float density = 1)
		{
			var radius = Mathf.CeilToInt(Mathf.Sqrt(totalPixels / Mathf.PI));
			var positions = new List<Vector2>();
			for(int y = -radius; y <= radius; y++)
			{
				for(int x = -radius; x <= radius; x++)
				{
					if(x * x + y * y <= radius * radius)
					{
						positions.Add(new Vector2(x, y) * density / radius);
					}
				}
			}
			return positions.ToArray();
		}
		public static  IEnumerable<Vector2> RectFormation(int width, int height, int nthOffset, float spread) {
			var middleOffset = new Vector2(width * 0.5f, height * 0.5f);

			for (var x = 0; x < width; x++) {
				for (var z = 0; z < height; z++) {
					var pos = new Vector2(x + (z % 2 == 0 ? 0 : nthOffset), z);
					pos -= middleOffset;
					pos *= spread;
					yield return pos;
				}
			}
		}
		private static IEnumerable<Vector2> GetVerticalLine(int x, int y)
		{
			while (y > 0)
			{
				yield return new Vector2(x, y);
				y--;
			}
		}
		public static Vector2 CalculatePositionInsideUnitCircle(int index, int total)
		{
			var angle = 2 * Mathf.PI * index / total;
			return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
		}
	}
}