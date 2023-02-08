using System.Collections.Generic;

namespace Graphics
{
	public interface IArmyInstancer
	{
		void CreateInstances(List<Unit> army);

		void UpdatePositions(List<Unit> army);
	}
}