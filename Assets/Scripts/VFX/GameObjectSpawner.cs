using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectSpawner : MonoBehaviour
{
	[SerializeField] private GameObject _prefab;
	[SerializeField] private int _amount;
	[SerializeField] private float _radius;
	void Start()
	{
		for (int i = 0; i < _amount; i++)
		{
			var position = Random.insideUnitCircle * _radius;
			var go = Instantiate(_prefab, position, Quaternion.identity);
			go.transform.SetParent(transform);
		}
	}

	// Update is called once per frame
	void Update()
	{
	}
}