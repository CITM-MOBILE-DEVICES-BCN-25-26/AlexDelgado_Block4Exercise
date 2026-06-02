using UnityEngine;

namespace MobilePerformance
{
	public class OptimizedDistanceCube : MonoBehaviour
	{
		public int lastNearCount;

		private void OnEnable()
		{
			OptimizedCubeDistanceManager.Register(this);
		}

		private void OnDisable()
		{
			OptimizedCubeDistanceManager.Unregister(this);
		}
	}
}
