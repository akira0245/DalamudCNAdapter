using Dalamud.Configuration;

namespace DalamudCNAdapter
{
	class DalamudCNAdapterConfig : IPluginConfiguration
	{
		private DalamudCNAdapterConfig()
		{
		}

		public static DalamudCNAdapterConfig Instance { get; } =
			(DalamudCNAdapterConfig)DalamudCNAdapter.DalamudPluginInterface.GetPluginConfig() ??
			new DalamudCNAdapterConfig();

		public int Version { get; set; }
		public void Save() => DalamudCNAdapter.DalamudPluginInterface.SavePluginConfig(this);

		public bool RunOnStart = false;
		public bool ReplaceFont = false;
		public bool ReplaceCommand = false;
		public float FontSize = 17.0f;
		public ushort[] FontGlyphRange = { 1, 0xFFFF };
	}
}