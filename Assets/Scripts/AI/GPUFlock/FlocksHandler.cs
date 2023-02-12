using System;
using System.Collections.Generic;
using Army;
using DefaultNamespace.Enums;
using GameState;
using UnityEngine;
// using Army.Unit = Unit;

namespace AI.GPUFlock
{
	public struct GPUUnitDraw
	{
		public Vector2 Position;
		public Vector2 Direction;
		public Vector2 TargetPos;
		public float Noise_Offset;
		public uint Team;
		public int TargetedUnit;
	}
	public class FlocksHandler : MonoBehaviour, ITickable
	{
		[SerializeField] private ComputeShader _computeFlock;
		const int THREADS = 256;
		public static readonly List<IUnitGroupSerializer> Serializers = new List<IUnitGroupSerializer>();
		private int[] _bufferSizes;
		private int _kernel;
		private int _stride;
		private GPUUnitDraw[] _units;
		private ComputeBuffer _unitsBuffer;
		public static FlocksHandler Instance{get; private set;}
		public List<Army.Units.Unit> Units{get; private set;} = new List<Army.Units.Unit>();

		private void Awake()
		{
			if(Instance == null)
				Instance = this;
			else
			{
				Destroy(gameObject);
				return;
			}
			Ticker.AddTickable(this);
			_kernel = _computeFlock.FindKernel("CSMain");
			_stride = (sizeof(float) * 2) * 3 + sizeof(float) + sizeof(uint) + sizeof(int); // 3 vectors2 + 1 float + 1 uint
		}

		public void OnTick(Ticker.OnTickEventArgs tick)
		{
			UpdateBuffer();
			Dispatch(tick.DeltaTime);
			UpdateHandlers();
			_unitsBuffer.Release();
		}

		private void UpdateBuffer()
		{
			//cache all buffers
			var flockBuffers = new GPUUnitDraw[Serializers.Count][];
			_bufferSizes = new int[Serializers.Count];

			for(var i = 0; i < Serializers.Count; i++)
			{
				flockBuffers[i] = Serializers[i].Serialize();
			}

			//get all buffer sizes sum
			var bufferSize = 0;
			for(var i = 0; i < flockBuffers.Length; i++)
			{
				var length = flockBuffers[i].Length;
				_bufferSizes[i] = length;
				bufferSize += length;
			}

			//create new buffer
			_units = new GPUUnitDraw[bufferSize];
			_unitsBuffer = new ComputeBuffer(bufferSize, _stride);

			int pointer = 0;
			//concat all buffers
			foreach(var buffer in flockBuffers)
			{
				buffer.CopyTo(_units, pointer);
				pointer += buffer.Length;
			}
			_unitsBuffer.SetData(_units);
		}
		private void Dispatch(float tickTime)
		{
			_computeFlock.SetFloat("DeltaTime", tickTime);
			_computeFlock.SetFloat("RotationSpeed", 1);
			_computeFlock.SetFloat("BoidSpeed", 10);
			_computeFlock.SetFloat("BoidSpeedVariation", 0.1f);
			//_computeFlock.SetVector("TargetPosition", _target.Value);
			_computeFlock.SetFloat("NeighbourDistance", 1);
			_computeFlock.SetInt("BoidsCount", _units.Length);
			_computeFlock.SetBuffer(_kernel, "boidBuffer", _unitsBuffer);

			_computeFlock.Dispatch(_kernel, _units.Length / THREADS + 1, 1, 1);
		}
		private void UpdateHandlers()
		{
			var pointer = 0;
			_unitsBuffer.GetData(_units);
			for(int i = 0; i < Serializers.Count; i++)
			{
				var length = _bufferSizes[i];
				var handler = Serializers[i];
				var buffer = new GPUUnitDraw[length];
				Array.Copy(_units, pointer, buffer, 0, length);
				handler.Deserialize(buffer);
				pointer += length;
			}
		}
		public void AddUnitBufferHandler(IUnitGroupSerializer handler)
		{
			Serializers.Add(handler);
		}
		public void RemoveUnitBufferHandler(IUnitGroupSerializer handler)
		{
			Serializers.Remove(handler);
		}
	}
}