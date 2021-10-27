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

			//if (DalamudCNAdapterConfig.Instance.RunOnStart)
			{
				if (DalamudCNAdapterConfig.Instance.ReplaceFont)
				{
					FontReplacer.ReplaceMainFont();
				}
				else
				{
					//FontReplacer.RestoreMainFont();
				}

				if (DalamudCNAdapterConfig.Instance.ReplaceCommand)
				{
					CommandManagerReplacer.ReplaceCommanRegex();
				}
				//else
				//{
				//	CommandManagerReplacer.RestoreCommanRegex();
				//}
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

			if (ImGui.Begin("DalamudCNAdapter", ref uivisible, ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoCollapse))
			{
				//if (ImGui.Checkbox("Auto run", ref DalamudCNAdapterConfig.Instance.RunOnStart))
				//{
				//	DalamudCNAdapterConfig.Instance.Save();
				//}
				ImGui.SetNextWindowPos(ImGui.GetWindowViewport().GetCenter(), ImGuiCond.Always, new Vector2(0.5f, 0.5f));

				if (ImGui.BeginPopup("DalamudCNAdapter 提示", ImGuiWindowFlags.Popup | ImGuiWindowFlags.Modal | ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoDecoration))
				{
					ImGui.TextUnformatted("字体替换将在下次游戏启动时失效。\n\n");

					if (ImGui.Button("OK", new Vector2(-1, ImGui.GetFrameHeight()))) ImGui.CloseCurrentPopup();
					ImGui.EndPopup();
				}

				if (ImGui.Checkbox("ReplaceFont", ref DalamudCNAdapterConfig.Instance.ReplaceFont))
				{
					if (DalamudCNAdapterConfig.Instance.ReplaceFont)
					{
						FontReplacer.ReplaceMainFont();
					}
					else
					{
						ImGui.OpenPopup("DalamudCNAdapter 提示");
						//FontReplacer.RestoreMainFont();
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
			//FontReplacer.RestoreMainFont();
			CommandManagerReplacer.RestoreCommanRegex();
			DalamudPluginInterface.UiBuilder.Draw -= UiBuilder_Draw;
			DalamudPluginInterface.UiBuilder.OpenConfigUi -= UiBuilder_OpenConfigUi;
		}

		public string Name => nameof(DalamudCNAdapter);
	}
}
