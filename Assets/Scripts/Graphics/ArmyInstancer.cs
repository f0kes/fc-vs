using System.Collections.Generic;
using Army;
using Army.Units;
using DefaultNamespace.Enums;
using UnityEngine;

namespace Graphics
{
	public struct MeshData
	{
		public List<Matrix4x4> Matrices;
		public MaterialPropertyBlock MaterialPropertyBlock;
	}
	public class ArmyInstancer : MonoBehaviour, IArmyInstancer
	{
		public Mesh Mesh;
		public Material Material;
		[SerializeField] private Vector3 _offset = new Vector3(0, 1f, 0);
		private List<MeshData> Batches = new List<MeshData>();
		private static readonly int Color1 = Shader.PropertyToID("_Color");

		private void RenderBatches()
		{
			foreach(var batch in Batches)
			{
				UnityEngine.Graphics.DrawMeshInstanced(Mesh, 0, Material, batch.Matrices, batch.MaterialPropertyBlock);
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
			var batch = new MeshData() { Matrices = new List<Matrix4x4>(), MaterialPropertyBlock = new MaterialPropertyBlock() };
			Batches.Add(batch);
			var colors = new List<Vector4>();
			for(int i = 0; i < army.Count; i++)
			{
				float y;
				if(army[i].TargetIndex != -1)
					y = (Mathf.Sin(Time.time * 10) + 1.5f) / 10 + 0.5f;
				else
					y = 1;
				var scale = army[i].Health / army[i].Stats[ArmyStat.Health];

				Vector3 position = new Vector3(army[i].Position.x, 0, army[i].Position.y);
				if(Batches[^1].Matrices.Count > 1000)
				{
					batch.MaterialPropertyBlock.SetVectorArray(Color1, colors);
					batch = new MeshData() { Matrices = new List<Matrix4x4>(), MaterialPropertyBlock = new MaterialPropertyBlock() };
					Batches.Add(batch);
				}
				Batches[^1].Matrices.Add(Matrix4x4.TRS(position + _offset, Quaternion.identity, Vector3.one * scale));
				colors.Add(new Vector4(Mathf.Sin(i) + 1.5f, Mathf.Sin(i + 10) + 1.5f, 0, 1));
			}
			batch.MaterialPropertyBlock.SetVectorArray(Color1, colors);
		}

	}
}