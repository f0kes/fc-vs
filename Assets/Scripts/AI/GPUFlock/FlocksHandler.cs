using System;
using System.Collections.Generic;
using Army;
using GameState;
using UnityEngine;

namespace AI.GPUFlock
{
	public static class FlocksHandler
	{

		private static List<IUnitBufferHandler> _handlers = new List<IUnitBufferHandler>();
		private static int[] _bufferSizes;
		private static GPUUnitDraw[] _buffer;

		static FlocksHandler()
		{
			Ticker.OnTick += OnTick;
		}

		private static void OnTick(Ticker.OnTickEventArgs obj)
		{
			UpdateBuffer();
			UpdateHandlers();
		}
		private static void UpdateBuffer()
		{
			//cache all buffers
			GPUUnitDraw[][] buffers = new GPUUnitDraw[_handlers.Count][];
			_bufferSizes = new int[_handlers.Count];

			for(int i = 0; i < _handlers.Count; i++)
			{
				buffers[i] = _handlers[i].GetBuffer();
			}

			//get all buffer sizes sum
			int bufferSize = 0;
			for(int i = 0; i < buffers.Length; i++)
			{
				var length = buffers[i].Length;
				_bufferSizes[i] = length;
				bufferSize += length;
			}

			//create new buffer
			_buffer = new GPUUnitDraw[bufferSize];

			//copy all buffers to new buffer
			int index = 0;
			foreach(var buffer in buffers)
			{
				foreach(var unit in buffer)
				{
					_buffer[index] = unit;
					index++;
				}
			}
		}
		private static void UpdateHandlers()
		{
			var pointer = 0;
			for(int i = 0; i < _handlers.Count; i++)
			{
				var length = _bufferSizes[i];
				var handler = _handlers[i];
				var buffer = new GPUUnitDraw[length];
				Array.Copy(_buffer,pointer, buffer, 0,length);
				handler.SetBuffer(buffer);
				pointer += length;
			}
		}
		public static void AddUnitBufferHandler(IUnitBufferHandler handler)
		{
			_handlers.Add(handler);
		}
		public static void RemoveUnitBufferHandler(IUnitBufferHandler handler)
		{
			_handlers.Remove(handler);
		}
	}
}