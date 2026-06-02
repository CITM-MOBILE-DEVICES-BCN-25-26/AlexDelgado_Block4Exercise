using UnityEngine;

namespace MobilePerformance
{
	public class OptimizedDistanceCubeSpawner : MonoBehaviour
	{
		[Header("Spawn Settings")]
		[SerializeField] private int cubesCount = 50;
		[SerializeField] private float spacing = 2f;
		[SerializeField] private int cubesPerRow = 10;

		[Header("Prefab (this time required)")]
		[SerializeField] private GameObject cubePrefab;

		private void Awake()
		{
			if (cubePrefab == null)
			{
				Debug.LogError("OptimizedDistanceCubeSpawner needs a prefab assigned.", this);
				return;
			}

			SpawnCubes();
		}

		private void SpawnCubes()
		{
			Transform parent = transform;

			for (int i = 0; i < cubesCount; i++)
			{
				Vector3 position = GetCubePosition(i);
				Instantiate(cubePrefab, position, Quaternion.identity, parent);
			}
		}

		private Vector3 GetCubePosition(int index)
		{
			int x = index % cubesPerRow;
			int z = index / cubesPerRow;
			return new Vector3(x * spacing, 0f, z * spacing);
		}
	}
}
