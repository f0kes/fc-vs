using UnityEngine;

namespace GameState
{
	public class TickerMono : MonoBehaviour
	{
		private void Update()
		{
			Ticker.Update();
		}
	}
}