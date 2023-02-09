using System.Collections.Generic;
using Army;
using Army.Units;
using DefaultNamespace.Enums;
using UnityEngine;

namespace Graphics
{
	public class ArmyInstancer : MonoBehaviour, IArmyInstancer
	{
		public Mesh Mesh;
		public Material Material;
		[SerializeField] private Vector3 _offset = new Vector3(0, 1f, 0);
		private List<List<Matrix4x4>> Batches = new List<List<Matrix4x4>>();

		private void RenderBatches()
		{
			foreach(var batch in Batches)
			{
				UnityEngine.Graphics.DrawMeshInstanced(Mesh, 0, Material, batch);
			}
		}

		private void Update()
		{
			RenderBatches();
		}


		public void CreateInstances(List<Unit> army)
		{
			UpdatePositions(army);
		}

		public void UpdatePositions(List<Unit> army)
		{
			Batches.Clear();
			Batches.Add(new List<Matrix4x4>());
			for(int i = 0; i < army.Count; i++)
			{
				float y;
				if(army[i].TargetIndex != -1)
					y = (Mathf.Sin(Time.time * 10) + 1.5f) / 10 + 0.5f;
				else
					y = 1;
				var scale = army[i].Health / army[i].Stats[ArmyStat.Health];

				Vector3 position = new Vector3(army[i].Position.x, 0, army[i].Position.y);
				if(Batches[Batches.Count - 1].Count > 1000)
				{
					Batches.Add(new List<Matrix4x4>());
				}
				Batches[Batches.Count - 1].Add(Matrix4x4.TRS(position + _offset, Quaternion.identity, Vector3.one * scale));
			}
		}

	}
}