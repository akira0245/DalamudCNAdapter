using System.Reflection;
using System.Text.RegularExpressions;
using Dalamud.Game.Command;

namespace DalamudCNAdapter
{
	static class CommandManagerReplacer
	{
		static CommandManagerReplacer()
		{
			_fieldInfo = typeof(CommandManager).GetField("currentLangCommandRegex",
				BindingFlags.Instance | BindingFlags.NonPublic);
			_original = (Regex)_fieldInfo.GetValue(DalamudCNAdapter.CommandManager);
		}

		private static readonly Regex commandRegexCn = new(@"^“(?<command>.+)”出现问题：该命令不存在。$", RegexOptions.Compiled);
		private static FieldInfo? _fieldInfo;
		private static Regex _original;

		public static void ReplaceCommanRegex()
		{
			_fieldInfo.SetValue(DalamudCNAdapter.CommandManager, commandRegexCn);
		}

		public static void RestoreCommanRegex()
		{
			_fieldInfo.SetValue(DalamudCNAdapter.CommandManager, _original);
		}
	}
}