using System;
using System.Collections.Generic;
using System.Reflection;

namespace CommandTerminal
{
	public class CommandShell
	{
		private Dictionary<string, CommandInfo> commands = new Dictionary<string, CommandInfo>();

		private List<CommandArg> arguments = new List<CommandArg>();

		public string IssuedErrorMessage
		{
			get;
			private set;
		}

		public Dictionary<string, CommandInfo> Commands => commands;

		public void RegisterCommands()
		{
			Dictionary<string, CommandInfo> dictionary = new Dictionary<string, CommandInfo>();
			BindingFlags bindingAttr = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
			Type[] types = Assembly.GetExecutingAssembly().GetTypes();
			for (int i = 0; i < types.Length; i++)
			{
				MethodInfo[] methods = types[i].GetMethods(bindingAttr);
				foreach (MethodInfo methodInfo in methods)
				{
					RegisterCommandAttribute registerCommandAttribute = Attribute.GetCustomAttribute(methodInfo, typeof(RegisterCommandAttribute)) as RegisterCommandAttribute;
					if (registerCommandAttribute == null)
					{
						if (!methodInfo.Name.StartsWith("FRONTCOMMAND", StringComparison.CurrentCultureIgnoreCase))
						{
							continue;
						}
						registerCommandAttribute = new RegisterCommandAttribute();
					}
					ParameterInfo[] parameters = methodInfo.GetParameters();
					string text = InferFrontCommandName(methodInfo.Name);
					text = ((registerCommandAttribute.Name != null) ? registerCommandAttribute.Name : InferCommandName((text == null) ? methodInfo.Name : text));
					if (parameters.Length != 1 || parameters[0].ParameterType != typeof(CommandArg[]))
					{
						dictionary.Add(text.ToUpper(), CommandFromParamInfo(parameters, registerCommandAttribute.Help));
						continue;
					}
					Action<CommandArg[]> proc = (Action<CommandArg[]>)Delegate.CreateDelegate(typeof(Action<CommandArg[]>), methodInfo);
					AddCommand(text, proc, registerCommandAttribute.MinArgCount, registerCommandAttribute.MaxArgCount, registerCommandAttribute.Help);
				}
			}
			HandleRejectedCommands(dictionary);
		}

		public void RunCommand(string line)
		{
			string s = line;
			IssuedErrorMessage = null;
			arguments.Clear();
			while (s != "")
			{
				CommandArg item = EatArgument(ref s);
				if (item.String != "")
				{
					arguments.Add(item);
				}
			}
			if (arguments.Count != 0)
			{
				string text = arguments[0].String.ToUpper();
				arguments.RemoveAt(0);
				if (!commands.ContainsKey(text))
				{
					IssueErrorMessage("Command {0} could not be found", text);
				}
				else
				{
					RunCommand(text, arguments.ToArray());
				}
			}
		}

		public void RunCommand(string command_name, CommandArg[] arguments)
		{
			CommandInfo commandInfo = commands[command_name];
			int num = arguments.Length;
			string text = null;
			int num2 = 0;
			if (num < commandInfo.min_arg_count)
			{
				text = ((commandInfo.min_arg_count != commandInfo.max_arg_count) ? "at least" : "exactly");
				num2 = commandInfo.min_arg_count;
			}
			else if (commandInfo.max_arg_count > -1 && num > commandInfo.max_arg_count)
			{
				text = ((commandInfo.min_arg_count != commandInfo.max_arg_count) ? "at most" : "exactly");
				num2 = commandInfo.max_arg_count;
			}
			if (text != null)
			{
				string text2 = (num2 == 1) ? "" : "s";
				IssueErrorMessage("{0} requires {1} {2} argument{3}", command_name, text, num2, text2);
			}
			else
			{
				commandInfo.proc(arguments);
			}
		}

		public void AddCommand(string name, CommandInfo info)
		{
			name = name.ToUpper();
			if (commands.ContainsKey(name))
			{
				IssueErrorMessage("Command {0} is already defined.", name);
			}
			else
			{
				commands.Add(name, info);
			}
		}

		public void AddCommand(string name, Action<CommandArg[]> proc, int min_arg_count = 0, int max_arg_count = -1, string help = "")
		{
			CommandInfo commandInfo = default(CommandInfo);
			commandInfo.proc = proc;
			commandInfo.min_arg_count = min_arg_count;
			commandInfo.max_arg_count = max_arg_count;
			commandInfo.help = help;
			CommandInfo info = commandInfo;
			AddCommand(name, info);
		}

		public void IssueErrorMessage(string format, params object[] message)
		{
			IssuedErrorMessage = string.Format(format, message);
		}

		private string InferCommandName(string method_name)
		{
			int num = method_name.IndexOf("COMMAND", StringComparison.CurrentCultureIgnoreCase);
			if (num >= 0)
			{
				return method_name.Remove(num, 7);
			}
			return method_name;
		}

		private string InferFrontCommandName(string method_name)
		{
			int num = method_name.IndexOf("FRONT", StringComparison.CurrentCultureIgnoreCase);
			if (num < 0)
			{
				return null;
			}
			return method_name.Remove(num, 5);
		}

		private void HandleRejectedCommands(Dictionary<string, CommandInfo> rejected_commands)
		{
			foreach (KeyValuePair<string, CommandInfo> rejected_command in rejected_commands)
			{
				if (commands.ContainsKey(rejected_command.Key))
				{
					commands[rejected_command.Key] = new CommandInfo
					{
						proc = commands[rejected_command.Key].proc,
						min_arg_count = rejected_command.Value.min_arg_count,
						max_arg_count = rejected_command.Value.max_arg_count,
						help = rejected_command.Value.help
					};
				}
				else
				{
					IssueErrorMessage("{0} is missing a front command.", rejected_command);
				}
			}
		}

		private CommandInfo CommandFromParamInfo(ParameterInfo[] parameters, string help)
		{
			int num = 0;
			for (int i = 0; i < parameters.Length; i++)
			{
				if (parameters[i].IsOptional)
				{
					num++;
				}
			}
			CommandInfo result = default(CommandInfo);
			result.proc = null;
			result.min_arg_count = parameters.Length - num;
			result.max_arg_count = parameters.Length;
			result.help = help;
			return result;
		}

		private CommandArg EatArgument(ref string s)
		{
			CommandArg result = default(CommandArg);
			int num = s.IndexOf(' ');
			if (num >= 0)
			{
				result.String = s.Substring(0, num);
				s = s.Substring(num + 1);
			}
			else
			{
				result.String = s;
				s = "";
			}
			return result;
		}
	}
}
