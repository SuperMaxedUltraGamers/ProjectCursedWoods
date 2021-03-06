namespace CursedWoods.Utils
{
	public interface ITimer
	{
		/// <summary>
		/// Notifies about timer completion.
		/// </summary>
		event System.Action TimerCompleted;

		/// <summary>
		/// Indicates weather the timer has finished or not. Note! Stopping the timer before it
		/// is finished should not change this value to true.
		/// </summary>
		bool IsComplete { get; }

		/// <summary>
		/// Indicates weather the timer is running or not.
		/// </summary>
		bool IsRunning { get; }

		/// <summary>
		/// Starts the timer.
		/// </summary>
		void Run();

		/// <summary>
		/// Stops the timer. Can be used as a Pause, i.e. doesn't reset the timer.
		/// </summary>
		void Stop();

		/// <summary>
		/// Resets the timer. Stops the timer as well if it's running.
		/// Sets IsComplete to false.
		/// </summary>
		void Reset();

		/// <summary>
		/// Sets the time. Does not start the timer. Stops the timer if timer is running when called.
		/// Sets IsComplete to false.
		/// </summary>
		/// <param name="time">The time for timer.</param>
		void Set(float time);
	}
}
