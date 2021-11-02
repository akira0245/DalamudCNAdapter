using System;
using System.Numerics;
using Dalamud;
using Dalamud.Game.Command;
using Dalamud.Interface;
using Dalamud.IoC;
using Dalamud.Plugin;
using ImGuiNET;

namespace DalamudCNAdapter
{
	public class DalamudFontReplacer : IDalamudPlugin
	{
		[PluginService] public static DalamudPluginInterface DalamudPluginInterface { get; set; }
		public DalamudFontReplacer()
		{
			FontReplacer.ReplaceMainFont();
		}


		public void Dispose()
		{
		}

		public string Name => nameof(DalamudFontReplacer);
	}
}
