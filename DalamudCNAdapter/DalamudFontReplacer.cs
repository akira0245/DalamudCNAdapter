using System;
using System.Numerics;
using System.Threading.Tasks;
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
            if (DalamudPluginInterface.Reason == PluginLoadReason.Boot)
            {
				Task.Delay(5000).ContinueWith(task => FontReplacer.ReplaceMainFont());
			}
            else
            {
                FontReplacer.ReplaceMainFont();
            }
			DalamudFontReplacerConfig.Instance.Save();
		}


        public void Dispose()
		{
			DalamudFontReplacerConfig.Instance.Save();
		}

		public string Name => nameof(DalamudFontReplacer);
	}
}
