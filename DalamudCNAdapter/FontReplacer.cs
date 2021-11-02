using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Dalamud.Interface;
using Dalamud.Plugin;
using ImGuiNET;

namespace DalamudCNAdapter
{
	static class FontReplacer
	{
		static FontReplacer()
		{
			var service = typeof(DalamudPluginInterface).Assembly.GetTypes().First(i => i.Name.Contains("Service`1"));
			var interfaceManager = typeof(DalamudPluginInterface).Assembly.GetTypes().First(i => i.Name.EndsWith("InterfaceManager"));
			var interfaceManagerInstance = service.MakeGenericType(interfaceManager); _interfaceManager = interfaceManagerInstance.GetMethod("Get").Invoke(null, null);
			_defaultFont = interfaceManager.GetProperty("DefaultFont", BindingFlags.Public | BindingFlags.Static);
			_fontFilename = Path.Combine(Assembly.GetExecutingAssembly().Location, "..", DalamudFontReplacerConfig.Instance.FontPath);
		}

		private static bool replaced = false;
		public unsafe static void ReplaceMainFont()
		{
			if (replaced) return;

			if (!File.Exists(_fontFilename))
			{
				throw new FileNotFoundException($"{_fontFilename} not found");
			}

			//OriginalDefaultFont = DalamudDefaultFont;

			DalamudFontReplacer.DalamudPluginInterface.UiBuilder.BuildFonts += UiBuilder_BuildFonts;
			DalamudFontReplacer.DalamudPluginInterface.UiBuilder.RebuildFonts();

			DalamudFontReplacer.DalamudPluginInterface.UiBuilder.Draw += pushMyFont;
			static void pushMyFont()
			{
				if (myFont.HasValue)
				{
					var fonts = ImGui.GetIO().Fonts;
					fonts.Fonts[0] = myFont.Value;
					DalamudDefaultFont = myFont.Value;
					DalamudFontReplacer.DalamudPluginInterface.UiBuilder.Draw -= pushMyFont;
					replaced = true;
				}
			}
		}


		//public static void RestoreMainFont()
		//{
		//	if (!OriginalDefaultFont.HasValue) return;

		//	var fonts = ImGui.GetIO().Fonts;
		//	fonts.Fonts[0] = fonts.Fonts[fonts.Fonts.Size - 1];
		//	DalamudDefaultFont = fonts.Fonts[fonts.Fonts.Size - 1];

		//	DalamudCNAdapter.DalamudPluginInterface.UiBuilder.BuildFonts -= UiBuilder_BuildFonts;
		//	DalamudCNAdapter.DalamudPluginInterface.UiBuilder.RebuildFonts();
		//}

		private static ImFontPtr DalamudDefaultFont
		{
			get => UiBuilder.DefaultFont;
			set => _defaultFont.SetValue(_interfaceManager, value);
		}

		private static ImFontPtr? myFont = null;

		private static readonly PropertyInfo _defaultFont;
		private static readonly object _interfaceManager;
		private static readonly string _fontFilename;

		private static void UiBuilder_BuildFonts()
		{
			var gcHandle = GCHandle.Alloc(DalamudFontReplacerConfig.Instance.FontGlyphRange, GCHandleType.Pinned);
			myFont = ImGui.GetIO().Fonts
				.AddFontFromFileTTF(_fontFilename, DalamudFontReplacerConfig.Instance.FontSize, null, gcHandle.AddrOfPinnedObject());
			gcHandle.Free();
		}
	}
}