using System;
using System.Collections;
using System.Collections.Generic;
using Formations.Scripts;
using UnityEngine;

public class BoxFormation : FormationBase
{
	private int _rows = 0;
	private float _widthScale = 5;
	private float _heightScale = 5;
	private bool _hollow = false;
	private float _nthOffset = 0;
	private float _density = 1;

	public BoxFormation( float widthScale = 1, float heightScale = 1, bool hollow = false, float nthOffset = 0, float density = 1)
	{
		_widthScale = widthScale;
		_heightScale = heightScale;
		_hollow = hollow;
		_nthOffset = nthOffset;
		
		_density = density;
		
	}
	
	public override IEnumerable<Vector2> EvaluatePoints(int armysize)
	{
		_rows = Mathf.CeilToInt(Mathf.Sqrt(armysize));
		var middleOffset = new Vector2(_widthScale * 0.5f, _heightScale * 0.5f);

		for(var x = 0; x < _rows; x++)
		{
			for(var y = 0; y < _rows; y++)
			{
				if(_hollow && x != 0 && x != _rows - 1 && y != 0 && y != _rows - 1) continue;
				var pos = new Vector2(x + (y % 2 == 0 ? 0 : _nthOffset) * _widthScale, y * _heightScale);

				pos -= middleOffset;

				pos *= 1/_density;

				yield return pos;
			}
		}
	}
}