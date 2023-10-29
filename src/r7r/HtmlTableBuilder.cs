using System.Text;

namespace r7r;

internal sealed class HtmlTableBuilder
{
    private readonly StringBuilder _sb = new();
    private readonly HorizontalAlignment[] _horizontalAdjustments;

    public HtmlTableBuilder(int numberOfColumns, HorizontalAlignment[]? horizontalAlignments = null)
    {
        if (horizontalAlignments == null)
        {
            horizontalAlignments = new HorizontalAlignment[numberOfColumns];
            horizontalAlignments[0] = HorizontalAlignment.Right;

            for (int i = 1; i < horizontalAlignments.Length; i++)
            {
                horizontalAlignments[i] = HorizontalAlignment.Left;
            }
        }

        if (numberOfColumns != horizontalAlignments.Length)
        {
            throw new ArgumentException("Number of columns must be equal to number of adjustments");
        }

        _horizontalAdjustments = horizontalAlignments;

        _sb.Append("""<table>""");
    }

    public HtmlTableBuilder AppendHeaderRow(params string[] headers)
    {
        _sb.Append("""<tr>""");

        for (int i = 0; i < headers.Length; i++)
        {
            switch (_horizontalAdjustments[i])
            {
                case HorizontalAlignment.NotSpecified:
                    _sb.Append("""<th>""");
                    break;
                case HorizontalAlignment.Left:
                    _sb.Append("""<th align="left">""");
                    break;
                case HorizontalAlignment.Right:
                    _sb.Append("""<th align="right">""");
                    break;
                case HorizontalAlignment.Center:
                    _sb.Append("""<th align="center">""");
                    break;
            }

            _sb.Append(headers[i]).Append("""</th>""");
        }

        _sb.Append("""</tr>""");

        return this;
    }

    public HtmlTableBuilder AppendRow(params string[] cells)
    {
        _sb.Append("""<tr>""");

        for (int i = 0; i < cells.Length; i++)
        {
            switch (_horizontalAdjustments[i])
            {
                case HorizontalAlignment.NotSpecified:
                    _sb.Append("""<td>""");
                    break;
                case HorizontalAlignment.Left:
                    _sb.Append("""<td align="left">""");
                    break;
                case HorizontalAlignment.Right:
                    _sb.Append("""<td align="right">""");
                    break;
                case HorizontalAlignment.Center:
                    _sb.Append("""<td align="center">""");
                    break;
            }

            _sb.Append(cells[i]).Append("""</td>""");
        }

        _sb.Append("""</tr>""");

        return this;
    }

    public string Build() => _sb.Append("""</table>""").ToString();
}
