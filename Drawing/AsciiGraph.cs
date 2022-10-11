using System.Text;
using HerstLib.Utilities;

namespace HerstLib.Drawing
{
    public class AsciiGraph
    {
        private readonly Dictionary<string, long> Data;
        public AsciiGraph(Dictionary<string, long> data) => Data = data;
        public AsciiGraph(List<long> data) => Data = data.ToDictionary(x => x.ToString(), x => x);

        public string[] DrawVertical(int height)
        {
            var sb = new StringBuilder();
            var values = Data.Values.ToArray();
            var min = Data.Values.Min();
            var max = Data.Values.Max();

            var lables = Data.Keys.ToArray();
            var maxLabelLength = lables.Max(x => x.Length);
            var maxBarheight = height - maxLabelLength - 1;

            for (int i = 0; i < values.Length; i++)
                values[i] = (long)MathF.Round((float)values[i] / max * maxBarheight);

            for (int i = maxBarheight; i > 0; i--)
            {
                var legend = (long)(i != maxBarheight ? (float)max / maxBarheight * i : max);
                var strLegend = HumanReadable.ToTGMK(legend) + "┫";

                if (i == 1)
                    sb.Append(strLegend.PadLeft(5));
                else if (i % 4 == 0 && i != maxBarheight)
                    sb.Append(strLegend.PadLeft(5));
                else if (i == maxBarheight)
                    sb.Append(strLegend.PadLeft(5));
                else
                    sb.Append("┫".PadLeft(5));

                for (long j = 0; j < Data.Keys.Count; j++)
                    sb.Append(values[j] >= i ? " ███ " : "     ");
                sb.AppendLine();
            }
            sb.Append("┗".PadLeft(5));
            foreach (var label in lables)
                sb.Append("━━╋━━");
            sb.AppendLine();
            for (int i = 0; i < maxLabelLength; i++)
            {
                sb.Append("     ");
                foreach (var label in lables)
                    sb.Append(label.Length > i ? $"  {label[i]}  " : "    ");
                sb.AppendLine();
            }

            return sb.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        }

        public string[] DrawHorizontal(int width, bool compact = true)
        {
            var sb = new StringBuilder();
            var rawvalues = Data.Values.ToArray();
            var values = Data.Values.ToArray();
            var max = Data.Values.Max();

            var labels = Data.Keys.ToArray();
            var maxLabelLength = labels.Max(x => x.Length);
            var maxBarWidth = width - maxLabelLength - 1;

            for (long i = 0; i < values.Length; i++)
                values[i] = (long)MathF.Round((float)values[i] / max * maxBarWidth);

            for (int i = 0; i < Data.Keys.Count; i++)
            {
                var legend = rawvalues[i];
                string strLegend = HumanReadable.ToTGMK(legend);
                if (compact)
                    sb.Append($"{$"{(labels[i] + ':').PadRight(maxLabelLength + 1)} {strLegend,4}".PadRight(1)}");
                else
                    sb.Append($"{i.ToString().PadRight(Data.Keys.Count.ToString().Length)}");

                sb.Append(' ');
                sb.Append("┣".PadRight((int)values[i], '━'));
                sb.AppendLine();
            }
            if (!compact)
            {
                sb.AppendLine();
                for (int i = 0; i < labels.Length; i++)
                {
                    var legend = rawvalues[i];
                    var strLegend = HumanReadable.ToTGMK(legend);
                    sb.AppendLine($"{i.ToString().PadRight(2)} {(labels[i] + ':').PadRight(maxLabelLength + 1)} {rawvalues[i]}");
                }
            }

            return sb.ToString().Split(Environment.NewLine);
        }
    }
}
