using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectSpawner : MonoBehaviour
{
	[SerializeField] private GameObject _prefab;
	[SerializeField] private int _count = 10;
	[SerializeField] private int _rows = 10;
	[SerializeField] private float _spacing = 1;
	private void Awake()
	{
		var positions = new Vector3[_count];
		var position1 = transform.position;
		var myPosition = new Vector3(position1.x, 0, position1.z);
		for(int i = 0; i < _count; i++)
		{
			positions[i] = new Vector3(i % _rows * _spacing, 0, i / _rows * _spacing) + myPosition;
		}
		foreach(var position in positions)
		{
			Instantiate(_prefab, position, Quaternion.identity, transform);
		}
	}

}