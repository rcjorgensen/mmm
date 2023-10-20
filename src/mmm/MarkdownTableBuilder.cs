using System.Text;

namespace mmm;

internal sealed class MarkdownTableBuilder
{
    private readonly StringBuilder _sb = new();
    private readonly int[] _columnWidths;
    private readonly HorizontalAlignment[] _horizontalAlignments;
    private const int MinimumColumnWidth = 4;

    public MarkdownTableBuilder(
        int[] columnWidths,
        HorizontalAlignment[]? horizontalAlignments = null
    )
    {
        if (horizontalAlignments == null)
        {
            horizontalAlignments = new HorizontalAlignment[columnWidths.Length];
            horizontalAlignments[0] = HorizontalAlignment.Right;

            for (int i = 1; i < horizontalAlignments.Length; i++)
            {
                horizontalAlignments[i] = HorizontalAlignment.Left;
            }
        }

        if (columnWidths.Length != horizontalAlignments.Length)
        {
            throw new ArgumentException("Number of columns must be equal to number of adjustments");
        }

        _columnWidths = columnWidths;

        for (int i = 0; i < columnWidths.Length; i++)
        {
            _columnWidths[i] = columnWidths[i] + Math.Max(0, MinimumColumnWidth - columnWidths[i]);
        }

        _horizontalAlignments = horizontalAlignments;
    }

    public MarkdownTableBuilder AppendSeparator()
    {
        for (int i = 0; i < _columnWidths.Length; i++)
        {
            if (_horizontalAlignments[i] == HorizontalAlignment.NotSpecified)
            {
                _sb.Append('|').Append(' ').Append('-', _columnWidths[i]).Append(' ');
            }
            else if (_horizontalAlignments[i] == HorizontalAlignment.Left)
            {
                _sb.Append('|')
                    .Append(' ')
                    .Append(':')
                    .Append('-', _columnWidths[i] - 1)
                    .Append(' ');
            }
            else if (_horizontalAlignments[i] == HorizontalAlignment.Right)
            {
                _sb.Append('|')
                    .Append(' ')
                    .Append('-', _columnWidths[i] - 1)
                    .Append(':')
                    .Append(' ');
            }
            else if (_horizontalAlignments[i] == HorizontalAlignment.Center)
            {
                _sb.Append('|')
                    .Append(' ')
                    .Append(':')
                    .Append('-', _columnWidths[i] - 2)
                    .Append(':')
                    .Append(' ');
            }
        }

        _sb.Append('|').AppendLine();

        return this;
    }

    public MarkdownTableBuilder AppendRow(params string[] cells)
    {
        if (cells.Length != _columnWidths.Length)
        {
            throw new ArgumentException(
                "Number of cells must equal number of columns",
                nameof(cells)
            );
        }

        // To make multiline cells in markdown lines must be joined with `<br>` tags
        // We assume this is handled outside, because otherwise won't know the column widths
        // So we effectly only have to implement single line support here
        // See: https://stackoverflow.com/questions/47324785/how-can-i-add-a-table-with-multi-row-cells-to-a-readme-in-vsts/47335483#47335483

        for (int i = 0; i < cells.Length; i++)
        {
            var content = cells[i];

            _sb.Append('|')
                .Append(' ')
                .Append(content)
                .Append(' ', _columnWidths[i] - content.Length)
                .Append(' ');
        }

        _sb.Append('|').AppendLine();

        return this;
    }

    public string Build() => _sb.ToString();
}
