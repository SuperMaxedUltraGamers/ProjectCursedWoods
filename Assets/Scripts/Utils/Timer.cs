using System;
using UnityEngine;

namespace CursedWoods.Utils
{
	public class Timer : MonoBehaviour, ITimer
	{
		// This event should be public so we can register to listen to that from outside of this class.
		// Event can be triggered only from the class it is defined in.
		public event Action TimerCompleted;

		private float time = 0;

		[SerializeField, Tooltip("Should the timer run on start?")]
		private bool runOnStart = false;

		/// <summary>
		/// How long the timer has been running since start (in seconds)
		/// </summary>
		private float elapsedTime = 0;

		private bool isRunning = false;

		public bool IsComplete
		{
			get { return elapsedTime >= time; }
		}

		public bool IsRunning
		{
			get { return isRunning; }
			// Value is the actual value which was passed to the set accessor.
			// For example in case of IsRunning, if value is set like IsRunning = true, the value refers to the true
			private set { isRunning = value; }
		}

		private void Start()
		{
			// Should we run the timer on start or not?
			if (runOnStart)
			{
				Run();
			}
		}

		/// <summary>
		/// OnValidate is called after member variable values have been changed from Unity. Called only in Editor mode.
		/// </summary>
		private void OnValidate()
		{
			if (time < 0)
			{
				time = 0;
			}
		}

		//private void FixedUpdate()
		//{
		//	// In FixedUpdate use Time.fixedDeltaTime instead of deltaTime
		//	elapsedTime += Time.fixedDeltaTime;
		//}

		private void Update()
		{
			if (IsRunning)
			{
				elapsedTime += Time.deltaTime;

				if (IsComplete)
				{
					Stop();

					if (TimerCompleted != null)
					{
						TimerCompleted();
					}
				}
			}
		}

		public void Run()
		{
			IsRunning = true;
		}

		public void Stop()
		{
			IsRunning = false;
			elapsedTime = 0;
		}

		public void Pause()
        {
			IsRunning = false;
        }

        public void Reset()
        {
			// Call Stop instead of just setting IsRunning = false because we might add more functionality
			// to Stop and when we call that method from here, this method will automatically get that functionality
			// as well.
			Stop();
			elapsedTime = 0;
		}

		public void Set(float time)
		{
			Reset();
			this.time = time;
		}
	}
}