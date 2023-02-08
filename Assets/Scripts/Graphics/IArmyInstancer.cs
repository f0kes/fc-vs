using System.Collections.Generic;
using Army;
using Army.Units;

namespace Graphics
{
	public interface IArmyInstancer
	{
		void CreateInstances(List<Unit> army);

		void UpdatePositions(List<Unit> army);
	}
}