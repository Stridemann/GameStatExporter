using System.Windows.Forms;
using ExileCore.Shared.Attributes;
using ExileCore.Shared.Interfaces;
using ExileCore.Shared.Nodes;

namespace GameStatExporter
{
	//All properties and public fields of this class will be saved to file
	public class Settings : ISettings
	{
		public Settings()
		{
            Enable = new ToggleNode(false);
        }

        [Menu("Enable")]
        public ToggleNode Enable { get; set; }

        [Menu("Use commentary instead Description attribute")]
	    public ToggleNode UseComments { get; set; } = new ToggleNode(true);

	    [Menu("Save path")]
        public TextNode SavePath { get; set; } = new TextNode("");

	    [Menu("Export!")]
	    public ButtonNode ExportButtonNode { get; set; } = new ButtonNode();
	}
}
