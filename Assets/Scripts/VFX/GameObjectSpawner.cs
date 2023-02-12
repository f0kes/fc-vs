using System.Collections;
using System.Collections.Generic;
using AI.GPUFlock;
using UnityEngine;
using UnityEngine.VFX;

public class GameObjectSpawner : MonoBehaviour
{
	[SerializeField] private GameObject _prefab;
	[SerializeField] private int _amount;
	[SerializeField] private float _radius;
	[SerializeField] private Vector2 _bounds;
	[SerializeField] private VisualEffect _visualEffect;
	private GraphicsBuffer _buffer;
	private float _time;
	void Start()
	{
	}
	void OnDestroy()
	{
		_buffer?.Dispose();
		_buffer = null;
	}
	// Update is called once per frame
	void Update()
	{
		_buffer?.Dispose();
		_time += Time.deltaTime;
		if(_time > 1)
		{
			Vector2[] positions = new Vector2[_amount];
			for(int i = 0; i < _amount; i++)
			{
				positions[i] = new Vector2(Random.Range(-_bounds.x, _bounds.x), Random.Range(-_bounds.y, _bounds.y));
			}
			_buffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, _amount, 2 * sizeof(float));
			_buffer.SetData(positions);
			_time = 0;
		}


		_visualEffect.SetGraphicsBuffer("_Positions", _buffer);
		_visualEffect.Play();
	}
}