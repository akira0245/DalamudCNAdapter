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
			var im = typeof(DalamudPluginInterface).Assembly.GetTypes().First(i => i.Name.EndsWith("InterfaceManager"));
			var imInstance = service.MakeGenericType(im); _interfaceManager = imInstance.GetMethod("Get").Invoke(null, null);
			_defaultFont = im.GetProperty("DefaultFont", BindingFlags.Public | BindingFlags.Static);
			_fontFilename = Path.Combine(Assembly.GetExecutingAssembly().Location, "..", "NotoSansCJKsc-Medium.otf");
		}

		public static void ReplaceMainFont()
		{
			if (!File.Exists(_fontFilename))
			{
				throw new FileNotFoundException($"{_fontFilename} not found");
			}

			originalFont = DefaultFont;
			DalamudCNAdapter.DalamudPluginInterface.UiBuilder.BuildFonts += UiBuilder_BuildFonts;
			DalamudCNAdapter.DalamudPluginInterface.UiBuilder.Draw += pushMyFont;
			DalamudCNAdapter.DalamudPluginInterface.UiBuilder.RebuildFonts();

			static void pushMyFont()
			{
				if (myFont.HasValue)
				{
					DefaultFont = myFont.Value;
					ImGui.PushFont(myFont.Value);
					DalamudCNAdapter.DalamudPluginInterface.UiBuilder.Draw -= pushMyFont;
				}
			}
		}


		public static void RestoreMainFont()
		{
			if (!myFont.HasValue) return;
			myFont = null;

			DefaultFont = originalFont;
			ImGui.PopFont();
			DalamudCNAdapter.DalamudPluginInterface.UiBuilder.BuildFonts -= UiBuilder_BuildFonts;
			DalamudCNAdapter.DalamudPluginInterface.UiBuilder.RebuildFonts();
		}


		private static ImFontPtr DefaultFont
		{
			get => UiBuilder.DefaultFont;
			set => _defaultFont.SetValue(_interfaceManager, value);
		}

		private static ImFontPtr? myFont = null;
		private static ImFontPtr originalFont;

		private static readonly PropertyInfo _defaultFont;
		private static readonly object _interfaceManager;
		private static readonly string _fontFilename;

		private static void UiBuilder_BuildFonts()
		{
			var gcHandle = GCHandle.Alloc(DalamudCNAdapterConfig.Instance.FontGlyphRange, GCHandleType.Pinned);
			myFont = ImGui.GetIO().Fonts
				.AddFontFromFileTTF(_fontFilename, DalamudCNAdapterConfig.Instance.FontSize, null, gcHandle.AddrOfPinnedObject());
			gcHandle.Free();
		}
	}
}