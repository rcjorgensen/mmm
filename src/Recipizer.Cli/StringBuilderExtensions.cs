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

    public static StringBuilder AppendRowFirstColumnRightAdjusted(
        this StringBuilder sb,
        params (int, string)[] columnInnerWidthsAndContent
    )
    {
        var (firstWidth, firstContent) = columnInnerWidthsAndContent.First();
        var last = columnInnerWidthsAndContent.Skip(1);

        sb.Append('│').Append(' ');

        sb.Append(' ', InnerPadding(firstWidth, firstContent))
            .Append(firstContent)
            .Append(' ')
            .Append('│');

        foreach (var (innerWidth, content) in last)
        {
            sb.Append(' ')
                .Append(content)
                .Append(' ', InnerPadding(innerWidth, content))
                .Append(' ')
                .Append('│');
        }

        return sb.AppendLine();
    }

    public static StringBuilder AppendRow(
        this StringBuilder sb,
        params (int, string)[] columnInnerWidthsAndContent
    )
    {
        sb.Append('│');

        foreach (var (innerWidth, content) in columnInnerWidthsAndContent)
        {
            sb.Append(' ')
                .Append(content)
                .Append(' ', InnerPadding(innerWidth, content))
                .Append(' ')
                .Append('│');
        }

        return sb.AppendLine();
    }

    private static int InnerPadding(int innerWidth, string content) =>
        Math.Max(innerWidth - content.Length, 0);
}
