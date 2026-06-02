using TMPro;
using UnityEngine;

namespace MobilePerformance
{
	public class UnoptimizedTimerText : MonoBehaviour
	{
		[SerializeField] private TMP_Text timerText;

		private float elapsedTime;

		private void Awake()
		{
			if (timerText == null)
			{
				timerText = GetComponent<TMP_Text>();
			}
		}

		private void Update()
		{
			elapsedTime += Time.deltaTime;

			timerText.text = $"Time: {elapsedTime:0.000}";
		}
	}
}