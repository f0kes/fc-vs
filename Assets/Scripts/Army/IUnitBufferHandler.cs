namespace Army
{
	public interface IUnitBufferHandler
	{
		GPUUnitDraw[] GetBuffer();
		void SetBuffer(GPUUnitDraw[] buffer);
	}
}