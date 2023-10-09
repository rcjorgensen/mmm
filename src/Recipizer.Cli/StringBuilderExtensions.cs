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

    public static StringBuilder AppendRowFirstColumnRightAdjustedLastColumnMultiline(
        this StringBuilder sb,
        (int, string) firstColumnInnerWidthsAndContent,
        (int, string) secondColumnInnerWidthsAndContent,
        (int, string) thirdColumnInnerWidthsAndContent,
        (int, string[]) fourthColumnInnerWidthsAndContent
    )
    {
        var (firstWidth, firstContent) = firstColumnInnerWidthsAndContent;
        var (secondWidth, secondContent) = secondColumnInnerWidthsAndContent;
        var (thirdWidth, thirdContent) = thirdColumnInnerWidthsAndContent;
        var (fourthWidth, fourthContent) = fourthColumnInnerWidthsAndContent;

        var fourthContentTop = fourthContent.Take(fourthContent.Length / 2 + 1);
        var fourthContentMiddle = fourthContent.Skip(fourthContent.Length / 2 + 1).Take(1).Single();
        var fourthContentBottom = fourthContent.Skip(fourthContent.Length / 2 + 1).Skip(1);

        foreach (var line in fourthContentTop)
        {
            sb.Append('│')
                .Append(' ')
                .Append(' ', firstWidth)
                .Append(' ')
                .Append('│')
                .Append(' ')
                .Append(' ', secondWidth)
                .Append(' ')
                .Append('│')
                .Append(' ')
                .Append(' ', thirdWidth)
                .Append(' ')
                .Append('│')
                .Append(' ')
                .Append(line)
                .Append(' ', InnerPadding(fourthWidth, line))
                .Append(' ')
                .Append('│')
                .AppendLine();
        }

        sb.Append('│')
            .Append(' ')
            .Append(' ', InnerPadding(firstWidth, firstContent))
            .Append(firstContent)
            .Append(' ')
            .Append('│')
            .Append(' ')
            .Append(secondContent)
            .Append(' ', InnerPadding(secondWidth, secondContent))
            .Append(' ')
            .Append('│')
            .Append(' ')
            .Append(thirdContent)
            .Append(' ', InnerPadding(thirdWidth, thirdContent))
            .Append(' ')
            .Append('│')
            .Append(' ')
            .Append(fourthContentMiddle)
            .Append(' ', InnerPadding(fourthWidth, fourthContentMiddle))
            .Append(' ')
            .Append('│')
            .AppendLine();

        foreach (var line in fourthContentBottom)
        {
            sb.Append('│')
                .Append(' ')
                .Append(' ', firstWidth)
                .Append(' ')
                .Append('│')
                .Append(' ')
                .Append(' ', secondWidth)
                .Append(' ')
                .Append('│')
                .Append(' ')
                .Append(' ', thirdWidth)
                .Append(' ')
                .Append('│')
                .Append(' ')
                .Append(line)
                .Append(' ', InnerPadding(fourthWidth, line))
                .Append(' ')
                .Append('│')
                .AppendLine();
        }

        return sb;
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
