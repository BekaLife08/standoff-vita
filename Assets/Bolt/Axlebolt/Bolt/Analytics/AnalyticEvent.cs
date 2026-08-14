namespace Axlebolt.Bolt.Analytics
{
	public class AnalyticEvent
	{
		public string Category { get; set; }

		public string Event { get; set; }

		public int Count { get; set; }

		public AnalyticEvent(string category, string @event)
		{
			Category = category;
			Event = @event;
		}

		public AnalyticEvent(string category, string @event, int count)
		{
			Category = category;
			Event = @event;
			Count = count;
		}
	}
}
