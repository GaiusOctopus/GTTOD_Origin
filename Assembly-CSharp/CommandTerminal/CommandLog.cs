using System.Collections.Generic;

namespace CommandTerminal
{
	public class CommandLog
	{
		private List<LogItem> logs = new List<LogItem>();

		private int max_items;

		public List<LogItem> Logs => logs;

		public CommandLog(int max_items)
		{
			this.max_items = max_items;
		}

		public void HandleLog(string message, TerminalLogType type)
		{
			HandleLog(message, "", type);
		}

		public void HandleLog(string message, string stack_trace, TerminalLogType type)
		{
			LogItem logItem = default(LogItem);
			logItem.message = message;
			logItem.stack_trace = stack_trace;
			logItem.type = type;
			LogItem item = logItem;
			logs.Add(item);
			if (logs.Count > max_items)
			{
				logs.RemoveAt(0);
			}
		}

		public void Clear()
		{
			logs.Clear();
		}
	}
}
