using System.Text;
using TMPro;
using UnityEngine;

namespace MobilePerformance
{
	public class OptimizedTimerText : MonoBehaviour
	{
		[SerializeField] private TMP_Text timerText;

		[SerializeField] private int visibleDecimals = 1;

		private readonly StringBuilder builder = new StringBuilder(16);

		private float elapsedTime;
		private int lastShownTick = -1;
		private float tickScale;

		private void Awake()
		{
			if (timerText == null)
			{
				timerText = GetComponent<TMP_Text>();
			}

			tickScale = Mathf.Pow(10f, visibleDecimals);
		}

		private void Update()
		{
			elapsedTime += Time.deltaTime;

			int tick = (int)(elapsedTime * tickScale);
			if (tick == lastShownTick)
			{
				return;
			}
			lastShownTick = tick;

			builder.Length = 0;
			builder.Append("Time: ");
			builder.Append((tick / tickScale).ToString("F" + visibleDecimals));

			timerText.SetText(builder);
		}
	}
}
