using System;
using Dalamud;
using Dalamud.Game.Command;
using Dalamud.Interface;
using Dalamud.IoC;
using Dalamud.Plugin;
using ImGuiNET;

namespace DalamudCNAdapter
{
	public class DalamudCNAdapter : IDalamudPlugin
	{
		[PluginService] public static DalamudPluginInterface DalamudPluginInterface { get; set; }
		[PluginService] public static CommandManager CommandManager { get; set; }

		public DalamudCNAdapter()
		{
			DalamudPluginInterface.UiBuilder.Draw += UiBuilder_Draw;
			DalamudPluginInterface.UiBuilder.OpenConfigUi += UiBuilder_OpenConfigUi;
			if (DalamudPluginInterface.Reason == PluginLoadReason.Installer)
			{
				uivisible = true;
			}

			if (DalamudCNAdapterConfig.Instance.RunOnStart)
			{
				if (DalamudCNAdapterConfig.Instance.ReplaceFont)
				{
					FontReplacer.ReplaceMainFont();
				}
				else
				{
					FontReplacer.RestoreMainFont();
				}

				if (DalamudCNAdapterConfig.Instance.ReplaceCommand)
				{
					CommandManagerReplacer.ReplaceCommanRegex();
				}
				else
				{
					CommandManagerReplacer.RestoreCommanRegex();
				}
			}
		}

		private void UiBuilder_OpenConfigUi()
		{
			uivisible ^= true;
		}

		private bool uivisible = false;

		private void UiBuilder_Draw()
		{
			if (!uivisible) return;

			ImGui.SetNextWindowSize(ImGuiHelpers.ScaledVector2(240,160));
			if (ImGui.Begin("DalamudCNAdapter Config", ref uivisible, ImGuiWindowFlags.AlwaysAutoResize))
			{
				if (ImGui.Checkbox("Auto run", ref DalamudCNAdapterConfig.Instance.RunOnStart))
				{
					DalamudCNAdapterConfig.Instance.Save();
				}

				if (ImGui.Checkbox("ReplaceFont", ref DalamudCNAdapterConfig.Instance.ReplaceFont))
				{
					if (DalamudCNAdapterConfig.Instance.ReplaceFont)
					{
						FontReplacer.ReplaceMainFont();
					}
					else
					{
						FontReplacer.RestoreMainFont();
					}

					DalamudCNAdapterConfig.Instance.Save();
				}

				if (ImGui.Checkbox("ReplaceCommand", ref DalamudCNAdapterConfig.Instance.ReplaceCommand))
				{
					if (DalamudCNAdapterConfig.Instance.ReplaceCommand)
					{
						CommandManagerReplacer.ReplaceCommanRegex();
					}
					else
					{
						CommandManagerReplacer.RestoreCommanRegex();
					}

					DalamudCNAdapterConfig.Instance.Save();
				}
			}

			ImGui.End();
		}

		public void Dispose()
		{
			FontReplacer.RestoreMainFont();
			CommandManagerReplacer.RestoreCommanRegex();
			DalamudPluginInterface.UiBuilder.Draw -= UiBuilder_Draw;
			DalamudPluginInterface.UiBuilder.OpenConfigUi -= UiBuilder_OpenConfigUi;
		}

		public string Name => nameof(DalamudCNAdapter);
	}
}
