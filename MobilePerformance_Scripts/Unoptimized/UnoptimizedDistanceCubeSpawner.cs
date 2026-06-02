using UnityEngine;

namespace MobilePerformance
{
	public class UnoptimizedDistanceCubeSpawner : MonoBehaviour
	{
		[Header("Spawn Settings")]
		[SerializeField] private int cubesCount = 50;
		[SerializeField] private float spacing = 2f;
		[SerializeField] private int cubesPerRow = 10;

		[Header("Optional Prefab")]
		[SerializeField] private GameObject cubePrefab;
		
		private void Awake()
		{
			SpawnCubes();
		}

		private void SpawnCubes()
		{
			for (int i = 0; i < cubesCount; i++)
			{
				Vector3 position = GetCubePosition(i);
				GameObject cube = CreateCube(position, i);

				if (!cube.TryGetComponent(out UnoptimizedDistanceCube _))
				{
					cube.AddComponent<UnoptimizedDistanceCube>();
				}
			}
		}

		private Vector3 GetCubePosition(int index)
		{
			int x = index % cubesPerRow;
			int z = index / cubesPerRow;

			return new Vector3(x * spacing, 0f, z * spacing);
		}

		private GameObject CreateCube(Vector3 position, int index)
		{
			GameObject cube;

			if (cubePrefab != null)
			{
				cube = Instantiate(cubePrefab, position, Quaternion.identity, transform);
			}
			else
			{
				cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
				cube.transform.SetParent(transform);
				cube.transform.position = position;
			}

			cube.name = $"Bad Distance Cube {index + 1}";

			return cube;
		}
	}
}