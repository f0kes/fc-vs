using AI.GPUFlock;

namespace Army
{
	public interface IUnitGroupSerializer
	{
		GPUUnitDraw[] Serialize();
		void Deserialize(GPUUnitDraw[] buffer);
	}
}