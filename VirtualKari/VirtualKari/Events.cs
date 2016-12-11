using System;
namespace VirtualKari
{
	public class Events
	{
		public Events()
		{


		}
		public event EventHandler<MyEventArgs> MyEvent = (s, e) => { };
		public void DoMyWork()
		{
			MyEvent(this, new MyEventArgs(myWorkResult));
		}
	}

	public class MyEventArgs : EventArgs
	{
		public String MyWorkResult { get; private set; }
		public MyEventArgs(string myWorkResult)
		{
			MyWorkResult = myWorkResult;
		}
	}
}
