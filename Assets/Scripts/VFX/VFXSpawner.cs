using UnityEngine;
using UnityEngine.VFX;

namespace VFX
{
	public class VFXSpawner : MonoBehaviour
	{
		[SerializeField] private GameObject _prefab;
		[SerializeField] private int _amount;
		[SerializeField] private float _radius;
		[SerializeField] private Vector2 _bounds;
		[SerializeField] private VisualEffect _visualEffect;
		private GraphicsBuffer _buffer;
		private Vector2[] _positions;
		private float _time;
		void Start()
		{
			var positions = new Vector2[_amount];
			for(int i = 0; i < _amount; i++)
			{
				// ReSharper disable once PossibleLossOfFraction
				positions[i] = new Vector2(i % (int)_bounds.x, i / (int)_bounds.x);
			}
		}
		private Vector2[] GetPositions()
		{
			var positions = new Vector2[_amount];
			for(int i = 0; i < _amount; i++)
			{
				int columns = (int)_bounds.x;
				positions[i] = new Vector2(i % columns, i / columns);
			}
			return positions;
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


			var positions = GetPositions();
			_buffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, _amount, 2 * sizeof(float));
			_buffer.SetData(positions);
			_time = 0;


			_visualEffect.SetGraphicsBuffer("_Positions", _buffer);
			_visualEffect.Play();
		}
	}
}