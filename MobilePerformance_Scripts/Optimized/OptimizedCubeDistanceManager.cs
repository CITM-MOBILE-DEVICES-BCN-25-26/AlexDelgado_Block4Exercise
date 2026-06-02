using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MobilePerformance
{
	public class OptimizedCubeDistanceManager : MonoBehaviour
	{
		private static readonly List<OptimizedDistanceCube> RegisteredCubes = new List<OptimizedDistanceCube>(64);
		private static readonly StringBuilder LogBuilder = new StringBuilder(4096);

		[SerializeField] private float logInterval = 0.5f;

		[SerializeField] private float nearRadiusSqr = 25f;

		private float nextCheckTime;

		public static void Register(OptimizedDistanceCube cube)
		{
			if (cube != null && !RegisteredCubes.Contains(cube))
			{
				RegisteredCubes.Add(cube);
			}
		}

		public static void Unregister(OptimizedDistanceCube cube)
		{
			RegisteredCubes.Remove(cube);
		}

		private void Update()
		{
			if (Time.time < nextCheckTime) return;
			nextCheckTime = Time.time + logInterval;

			LogBuilder.Length = 0;

			int count = RegisteredCubes.Count;
			for (int i = 0; i < count; i++)
			{
				OptimizedDistanceCube me = RegisteredCubes[i];
				if (me == null) continue;

				Vector3 myPos = me.transform.position;
				int near = 0;

				for (int j = 0; j < count; j++)
				{
					if (j == i) continue;
					OptimizedDistanceCube other = RegisteredCubes[j];
					if (other == null) continue;

					float sqr = (other.transform.position - myPos).sqrMagnitude;
					if (sqr <= nearRadiusSqr) near++;

					if (LogBuilder.Length > 0) LogBuilder.Append('\n');
					LogBuilder.Append("Distance cube ");
					LogBuilder.Append(me.name);
					LogBuilder.Append(" -> ");
					LogBuilder.Append(other.name);
					LogBuilder.Append(": ");
					LogBuilder.Append(sqr);
				}

				me.lastNearCount = near;
			}

			if (LogBuilder.Length > 0)
			{
				Debug.Log(LogBuilder);
			}
		}
	}
}
