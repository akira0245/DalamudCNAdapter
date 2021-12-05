using Dalamud.Configuration;

namespace DalamudCNAdapter
{
	class DalamudFontReplacerConfig : IPluginConfiguration
	{
		private DalamudFontReplacerConfig()
		{
		}

		public static DalamudFontReplacerConfig Instance { get; } =
			(DalamudFontReplacerConfig)DalamudFontReplacer.DalamudPluginInterface.GetPluginConfig() ??
			new DalamudFontReplacerConfig();

		public int Version { get; set; }
		public void Save() => DalamudFontReplacer.DalamudPluginInterface.SavePluginConfig(this);

		public int replaceFontIndex = 0;
		public float FontSize = 17.0f;
		public ushort[] FontGlyphRange = { 1, 0xFFFF };
		public string FontPath = "NotoSansCJKsc-Medium.otf";
	}
}