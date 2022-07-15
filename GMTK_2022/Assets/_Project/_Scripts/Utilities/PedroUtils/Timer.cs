// maded by Pedro M Marangon
using System;
using UnityEngine;

namespace PedroUtils
{
	public class Timer
	{
		private string _name;
		private bool _shouldDebug;
		private float _startingSeconds;

		public Timer(string name, float seconds, bool shouldDebug = true)
		{
			_name = name;
			_shouldDebug = shouldDebug;
			_startingSeconds = seconds;
			RemainingSeconds = seconds;
		}

		public bool IsTimerRunning { get; private set; }
		public float RemainingSeconds { get; private set; }
		public float Percentage => RemainingSeconds / _startingSeconds;
		public event Action OnTimerEnd;
		public event Action<float> OnTimerTick;
		public event Action OnTimerChange;

		public void StartTicking() => StartTicking(_startingSeconds);
		public void StartTicking(float seconds)
		{
			if(RemainingSeconds <= 0) AddSeconds(seconds);
			if (_shouldDebug) Debug.Log($"{_name}: Start ticking...");
			IsTimerRunning = true;
		}
		public void StopTicking()
		{
			RemainingSeconds = 0;
			if (_shouldDebug) Debug.Log($"{_name}: Stop ticking...");
			IsTimerRunning = false;
		}

		public void AddSeconds(float secondsToAdd)
		{
			RemainingSeconds += secondsToAdd;
			OnTimerChange?.Invoke();
		}

		public void RemoveSeconds(float secondsToRemove)
		{
			RemainingSeconds -= secondsToRemove;
			OnTimerChange?.Invoke();
		}

		public void Tick(float deltaTime)
		{
			if (RemainingSeconds <= 0f || !IsTimerRunning) return;
			RemainingSeconds -= deltaTime;

			OnTimerTick?.Invoke(RemainingSeconds);

			CheckForTimerEnd();
		}

		private void CheckForTimerEnd()
		{
			if (RemainingSeconds > 0f) return;

			RemainingSeconds = 0;

			if (_shouldDebug) Debug.Log($"{_name}: TimerFinished");
			
			IsTimerRunning = false;

			OnTimerTick?.Invoke(RemainingSeconds);
			OnTimerEnd?.Invoke();
		}
	}
}
