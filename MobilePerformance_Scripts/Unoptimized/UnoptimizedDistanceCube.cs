using UnityEngine;

namespace MobilePerformance
{
	public class UnoptimizedDistanceCube : MonoBehaviour
	{
		private UnoptimizedDistanceCube[] allCubes;

		private void Start()
		{
			allCubes = FindObjectsByType<UnoptimizedDistanceCube>(FindObjectsSortMode.None);
		}

		private void Update()
		{
			for (int i = 0; i < allCubes.Length; i++)
			{
				UnoptimizedDistanceCube otherCube = allCubes[i];

				if (otherCube == this)
				{
					continue;
				}

				float distance = Vector3.Distance(transform.position, otherCube.transform.position);

				Debug.LogWarning($"Distance cube {this.gameObject.name}: {distance}");
			}
		}
	}
}