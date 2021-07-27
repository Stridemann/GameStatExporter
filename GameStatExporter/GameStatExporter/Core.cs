namespace GameStatExporter
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using ExileCore;

    public class Core : BaseSettingsPlugin<Settings>
    {
        public Core()
        {
            Name = "GameStatExporter";
        }

        public override bool Initialise()
        {
            base.Initialise();

            Settings.ExportButtonNode.OnPressed += Export;
            return true;
        }

        public override void OnPluginDestroyForHotReload()
        {
            Settings.ExportButtonNode.OnPressed -= Export;
        }

        private void Export()
        {
            if (string.IsNullOrEmpty(Settings.SavePath.Value))
            {
                LogError("Save path is not set", 5);

                return;
            }

            if (!Directory.Exists(Path.GetDirectoryName(Settings.SavePath.Value)))
            {
                LogError("Incorrect export path (No such directory)", 5);

                return;
            }

            var sb = new StringBuilder();

            if (!Settings.UseComments.Value)
            {
                sb.AppendLine("using System.ComponentModel;");
                sb.Append(Environment.NewLine);
            }

            sb.AppendLine("namespace ExileCore.Shared.Enums");
            sb.AppendLine("{");
            sb.AppendLine("\tpublic enum GameStat");
            sb.AppendLine("\t{");

            var duplicatesDict = new Dictionary<string, int>();

            foreach (var statsRecord in GameController.Files.Stats.records)
            {
                var descr = string.IsNullOrEmpty(statsRecord.Value.UserFriendlyName)
                        ? statsRecord.Value.Key
                        : statsRecord.Value.UserFriendlyName;

                if (Settings.UseComments.Value)
                {
                    sb.AppendLine("\t\t/// <summary>");
                    sb.AppendLine($"\t\t/// {descr}");
                    sb.AppendLine("\t\t/// </summary>");
                }
                else
                {
                    sb.AppendLine($"\t\t[Description(\"{descr}\")]");
                }

                var formatedName = FormatName(statsRecord.Key);

                if (duplicatesDict.TryGetValue(formatedName, out var duplicatesCount))
                {
                    duplicatesCount++;
                    formatedName += duplicatesCount;
                    duplicatesDict[formatedName] = duplicatesCount;
                }
                else
                {
                    duplicatesDict.Add(formatedName, 1);
                }

                sb.Append("\t\t");
                sb.Append(formatedName);
                sb.Append(" = ");
                sb.Append(statsRecord.Value.ID);
                sb.Append(",");
                sb.AppendLine(Environment.NewLine);
            }

            sb.AppendLine("\t}");
            sb.AppendLine("}");
            File.WriteAllText(Settings.SavePath.Value, sb.ToString());
        }

        private string FormatName(string name)
        {
            var renamed = name.Replace("%", "pct").Replace("+", "").Replace("-", "");
            var info = CultureInfo.CurrentCulture.TextInfo;
            renamed = info.ToTitleCase(renamed).Replace("_", string.Empty);

            return renamed;
        }
    }
}
