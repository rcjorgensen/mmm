using System.Text;

namespace Recipizer.Cli;

internal static class StringBuilderExtensions
{
    public static StringBuilder AppendTop(this StringBuilder sb, params int[] columnWidths)
    {
        sb.Append('┌');

        var first = columnWidths.SkipLast(1);
        var last = columnWidths.TakeLast(1).Single();

        foreach (var width in first)
        {
            sb.Append('─', width).Append('┬');
        }

        sb.Append('─', last).Append('┐').AppendLine();

        return sb;
    }

    public static StringBuilder AppendSeparator(this StringBuilder sb, params int[] columnWidths)
    {
        sb.Append('├');

        var first = columnWidths.SkipLast(1);
        var last = columnWidths.TakeLast(1).Single();

        foreach (var width in first)
        {
            sb.Append('─', width).Append('┼');
        }

        sb.Append('─', last).Append('┤').AppendLine();

        return sb;
    }

    public static StringBuilder AppendBottom(this StringBuilder sb, params int[] columnOuterWidths)
    {
        sb.Append('└');

        var first = columnOuterWidths.SkipLast(1);
        var last = columnOuterWidths.TakeLast(1).Single();

        foreach (var width in first)
        {
            sb.Append('─', width).Append('┴');
        }

        sb.Append('─', last).Append('┘').AppendLine();

        return sb;
    }

    public static StringBuilder AppendRow(
        this StringBuilder sb,
        params (int, string[], bool)[] columnInnerWidthsAndContent
    )
    {
        // Find row inner height - just max length of string[]s
        var rowInnerHeight = columnInnerWidthsAndContent.Select(x => x.Item2.Length).Max();

        // Center adjust all cells by padding with empty lines above and below
        var paddedContents = new List<string[]>();
        foreach (var (columnInnerWidth, content, rightAdjust) in columnInnerWidthsAndContent)
        {
            var topPadding = (rowInnerHeight - content.Length) / 2;
            var bottomPadding = rowInnerHeight - topPadding - content.Length;

            var paddedContent = new string[rowInnerHeight];

            var emptyLine = new StringBuilder().Append(' ', columnInnerWidth).ToString();

            for (int i = 0; i < topPadding; i++)
            {
                paddedContent[i] = emptyLine;
            }

            for (int i = topPadding; i < topPadding + content.Length; i++)
            {
                var line = content[i - topPadding].Trim(); // Let's trim to make sure there are no newlines, it's fine to trim all whitespace in doing so

                // Left adjust everything for now - later we'll use a flag or something to right or left adjust
                if (rightAdjust)
                {
                    paddedContent[i] = new StringBuilder()
                        .Append(' ', columnInnerWidth - line.Length)
                        .Append(line)
                        .ToString();
                }
                else
                {
                    paddedContent[i] = new StringBuilder()
                        .Append(line)
                        .Append(' ', columnInnerWidth - line.Length)
                        .ToString();
                }
            }

            for (
                int i = topPadding + content.Length;
                i < topPadding + content.Length + bottomPadding;
                i++
            )
            {
                paddedContent[i] = emptyLine;
            }

            paddedContents.Add(paddedContent);
        }

        for (int i = 0; i < rowInnerHeight; i++)
        {
            var cells = from x in paddedContents select x[i];
            foreach (var cell in cells)
            {
                sb.Append('│').Append(' ').Append(cell).Append(' ');
            }

            sb.Append('│').AppendLine();
        }

        return sb;
    }

    public static StringBuilder AppendRow(
        this StringBuilder sb,
        params (int, string, bool)[] columnInnerWidthsAndContent
    )
    {
        return sb.AppendRow(
            (
                from x in columnInnerWidthsAndContent
                select (x.Item1, new[] { x.Item2 }, x.Item3)
            ).ToArray()
        );
    }

    private static int InnerPadding(int innerWidth, string content) =>
        Math.Max(innerWidth - content.Length, 0);
}
