using System.Windows.Forms;
using SharpDX;
using PoeHUD.Plugins;
using PoeHUD.Hud.Settings;

namespace GameStatExporter
{
	//All properties and public fields of this class will be saved to file
	public class Settings : SettingsBase
	{
		public Settings()
		{
			Enable = true;
		}

	    [Menu("Use commentary instead Description attribute")]
	    public ToggleNode UseComments { get; set; } = true;

	    [Menu("Save path")]
        public TextNode SavePath { get; set; } = new TextNode();

	    [Menu("Export!")]
	    public ButtonNode ExportButtonNode { get; set; } = new ButtonNode();
	}
}
