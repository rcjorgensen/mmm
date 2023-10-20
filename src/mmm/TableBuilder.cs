using System.Text;

namespace mmm;

internal sealed class TableBuilder
{
    private readonly StringBuilder _sb = new();
    private readonly int[] _columnInnerWidths;
    private readonly int[] _columnOuterWidths;
    private readonly HorizontalAlignment[] _horizontalAlignments;
    private const int HorizontalPadding = 2;

    public TableBuilder(
        int[] columnInnerWidths,
        HorizontalAlignment[]? horizontalAlignments = null
    )
    {
        if (horizontalAlignments == null)
        {
            horizontalAlignments = new HorizontalAlignment[columnInnerWidths.Length];
            horizontalAlignments[0] = HorizontalAlignment.Right;

            for (int i = 1; i < horizontalAlignments.Length; i++)
            {
                horizontalAlignments[i] = HorizontalAlignment.Left;
            }
        }

        if (columnInnerWidths.Length != horizontalAlignments.Length)
        {
            throw new ArgumentException("Number of columns must be equal to number of adjustments");
        }

        _columnInnerWidths = columnInnerWidths;
        _columnOuterWidths = new int[columnInnerWidths.Length];

        for (int i = 0; i < columnInnerWidths.Length; i++)
        {
            _columnOuterWidths[i] = columnInnerWidths[i] + HorizontalPadding;
        }

        _horizontalAlignments = horizontalAlignments;
    }

    public TableBuilder AppendTop()
    {
        _sb.Append('┌');

        for (int i = 0; i < _columnOuterWidths.Length - 1; i++)
        {
            _sb.Append('─', _columnOuterWidths[i]).Append('┬');
        }

        _sb.Append('─', _columnOuterWidths[_columnOuterWidths.Length - 1]).Append('┐').AppendLine();

        return this;
    }

    public TableBuilder AppendSeparator()
    {
        _sb.Append('├');

        for (int i = 0; i < _columnOuterWidths.Length - 1; i++)
        {
            _sb.Append('─', _columnOuterWidths[i]).Append('┼');
        }

        _sb.Append('─', _columnOuterWidths[_columnOuterWidths.Length - 1]).Append('┤').AppendLine();

        return this;
    }

    public TableBuilder AppendBottom()
    {
        _sb.Append('└');

        for (int i = 0; i < _columnOuterWidths.Length - 1; i++)
        {
            _sb.Append('─', _columnOuterWidths[i]).Append('┴');
        }

        _sb.Append('─', _columnOuterWidths[_columnOuterWidths.Length - 1]).Append('┘').AppendLine();

        return this;
    }

    public TableBuilder AppendRow(params string[][] cells)
    {
        // TODO: We should sanitize the content of the cells and make sure they don't contain any newline characters that will mess up the table

        if (cells.Length != _columnInnerWidths.Length)
        {
            throw new ArgumentException(
                "Number of cells must equal number of columns",
                nameof(cells)
            );
        }

        var rowInnerHeight = cells.Select(x => x.Length).Max();

        // Center adjust all cells by padding with empty lines above and below


        var paddedContents = new List<string[]>();
        for (int i = 0; i < cells.Length; i++)
        {
            var content = cells[i];
            var columnInnerWidth = _columnInnerWidths[i];
            var adjustment = _horizontalAlignments[i];

            var topPadding = (rowInnerHeight - content.Length) / 2;
            var bottomPadding = rowInnerHeight - topPadding - content.Length;

            var paddedContent = new string[rowInnerHeight];

            var emptyLine = new StringBuilder().Append(' ', columnInnerWidth).ToString();

            for (int j = 0; j < topPadding; j++)
            {
                paddedContent[j] = emptyLine;
            }

            for (int j = topPadding; j < topPadding + content.Length; j++)
            {
                var line = content[j - topPadding].Trim();

                paddedContent[j] =
                    adjustment == HorizontalAlignment.Right
                        ? new StringBuilder()
                            .Append(' ', columnInnerWidth - line.Length)
                            .Append(line)
                            .ToString()
                        : new StringBuilder()
                            .Append(line)
                            .Append(' ', columnInnerWidth - line.Length)
                            .ToString();
            }

            for (
                int j = topPadding + content.Length;
                j < topPadding + content.Length + bottomPadding;
                j++
            )
            {
                paddedContent[j] = emptyLine;
            }

            paddedContents.Add(paddedContent);
        }

        // Append all the rows cell by cell


        for (int i = 0; i < rowInnerHeight; i++)
        {
            var paddedCells = from x in paddedContents select x[i];
            foreach (var cell in paddedCells)
            {
                _sb.Append('│').Append(' ').Append(cell).Append(' ');
            }

            _sb.Append('│').AppendLine();
        }

        return this;
    }

    public TableBuilder AppendRow(params string[] cells)
    {
        var singleLineCells = new string[cells.Length][];

        for (int i = 0; i < cells.Length; i++)
        {
            singleLineCells[i] = new[] { cells[i] };
        }

        return AppendRow(singleLineCells);
    }

    public string Build() => _sb.ToString();
}
