namespace CommandTerminal
{
	public struct CommandArg
	{
		public string String
		{
			get;
			set;
		}

		public int Int
		{
			get
			{
				if (int.TryParse(String, out int result))
				{
					return result;
				}
				TypeError("int");
				return 0;
			}
		}

		public float Float
		{
			get
			{
				if (float.TryParse(String, out float result))
				{
					return result;
				}
				TypeError("float");
				return 0f;
			}
		}

		public bool Bool
		{
			get
			{
				if (string.Compare(String, "TRUE", ignoreCase: true) == 0)
				{
					return true;
				}
				if (string.Compare(String, "FALSE", ignoreCase: true) == 0)
				{
					return false;
				}
				TypeError("bool");
				return false;
			}
		}

		public override string ToString()
		{
			return String;
		}

		private void TypeError(string expected_type)
		{
			Terminal.Shell.IssueErrorMessage("Incorrect type for {0}, expected <{1}>", String, expected_type);
		}
	}
}
