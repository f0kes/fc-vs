using AI.GPUFlock;

namespace Army
{
	public interface IUnitGroupSerializer
	{
		GPUUnitGroup GetGPUUnitGroup();
		GPUUnitDraw[] Serialize();
		void Deserialize(GPUUnitDraw[] buffer);
	}
}