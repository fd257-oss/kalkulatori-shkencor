using System.Globalization;

namespace ScientificCalculator;

public class CalculatorForm : Form
{
    private readonly TextBox _display;
    private double? _firstOperand;
    private string? _pendingBinaryOperation;
    private bool _shouldClearOnNextDigit;

    public CalculatorForm()
    {
        Text = "Kalkulator Shkencor";
        Width = 420;
        Height = 560;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;

        _display = new TextBox
        {
            Dock = DockStyle.Top,
            Height = 60,
            ReadOnly = true,
            TextAlign = HorizontalAlignment.Right,
            Font = new Font("Segoe UI", 20, FontStyle.Bold),
            Text = "0"
        };

        Controls.Add(_display);

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 5,
            RowCount = 6,
            Padding = new Padding(8)
        };

        for (var i = 0; i < layout.ColumnCount; i++)
        {
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
        }

        for (var i = 0; i < layout.RowCount; i++)
        {
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 16.66f));
        }

        AddButton(layout, "sin", 0, 0, (_, _) => ApplyUnary(Calculator.SinDeg));
        AddButton(layout, "cos", 1, 0, (_, _) => ApplyUnary(Calculator.CosDeg));
        AddButton(layout, "tan", 2, 0, (_, _) => ApplyUnary(Calculator.TanDeg));
        AddButton(layout, "log", 3, 0, (_, _) => ApplyUnary(Calculator.Log10));
        AddButton(layout, "ln", 4, 0, (_, _) => ApplyUnary(Calculator.Ln));

        AddButton(layout, "√", 0, 1, (_, _) => ApplyUnary(Calculator.Sqrt));
        AddButton(layout, "x²", 1, 1, (_, _) => ApplyUnary(x => Calculator.Power(x, 2)));
        AddButton(layout, "xʸ", 2, 1, (_, _) => SetBinaryOperation("^"));
        AddButton(layout, "n!", 3, 1, (_, _) => ApplyFactorial());
        AddButton(layout, "C", 4, 1, (_, _) => ClearAll(), Color.LightCoral);

        AddDigitRow(layout, "7", "8", "9", "/", 2);
        AddDigitRow(layout, "4", "5", "6", "*", 3);
        AddDigitRow(layout, "1", "2", "3", "-", 4);

        AddButton(layout, "±", 0, 5, (_, _) => ToggleSign());
        AddButton(layout, "0", 1, 5, (_, _) => AppendDigit("0"));
        AddButton(layout, ".", 2, 5, (_, _) => AppendDecimalPoint());
        AddButton(layout, "+", 3, 5, (_, _) => SetBinaryOperation("+"));
        AddButton(layout, "=", 4, 5, (_, _) => Evaluate(), Color.LightGreen);

        Controls.Add(layout);
    }

    private void AddDigitRow(TableLayoutPanel layout, string d1, string d2, string d3, string op, int row)
    {
        AddButton(layout, d1, 0, row, (_, _) => AppendDigit(d1));
        AddButton(layout, d2, 1, row, (_, _) => AppendDigit(d2));
        AddButton(layout, d3, 2, row, (_, _) => AppendDigit(d3));
        AddButton(layout, op, 3, row, (_, _) => SetBinaryOperation(op));
        if (row == 2)
        {
            AddButton(layout, "←", 4, row, (_, _) => Backspace());
        }
        else if (row == 3)
        {
            AddButton(layout, "%", 4, row, (_, _) => ApplyUnary(x => x / 100d));
        }
        else
        {
            AddButton(layout, "1/x", 4, row, (_, _) => ApplyUnary(x => Calculator.Divide(1d, x)));
        }
    }

    private void AddButton(TableLayoutPanel panel, string text, int column, int row, EventHandler onClick, Color? backColor = null)
    {
        var button = new Button
        {
            Text = text,
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 13, FontStyle.Bold),
            BackColor = backColor ?? Color.WhiteSmoke
        };

        button.Click += onClick;
        panel.Controls.Add(button, column, row);
    }

    private void AppendDigit(string digit)
    {
        if (_display.Text == "0" || _shouldClearOnNextDigit)
        {
            _display.Text = digit;
            _shouldClearOnNextDigit = false;
            return;
        }

        _display.Text += digit;
    }

    private void AppendDecimalPoint()
    {
        if (_shouldClearOnNextDigit)
        {
            _display.Text = "0";
            _shouldClearOnNextDigit = false;
        }

        if (!_display.Text.Contains(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator))
        {
            _display.Text += CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
        }
    }

    private void Backspace()
    {
        if (_shouldClearOnNextDigit)
        {
            _display.Text = "0";
            _shouldClearOnNextDigit = false;
            return;
        }

        if (_display.Text.Length <= 1)
        {
            _display.Text = "0";
        }
        else
        {
            _display.Text = _display.Text[..^1];
        }
    }

    private void ToggleSign()
    {
        var value = ReadDisplayValue();
        SetDisplayValue(-value);
    }

    private void SetBinaryOperation(string operation)
    {
        _firstOperand = ReadDisplayValue();
        _pendingBinaryOperation = operation;
        _shouldClearOnNextDigit = true;
    }

    private void Evaluate()
    {
        if (_firstOperand is null || string.IsNullOrWhiteSpace(_pendingBinaryOperation))
        {
            return;
        }

        var secondOperand = ReadDisplayValue();
        var result = _pendingBinaryOperation switch
        {
            "+" => Calculator.Add(_firstOperand.Value, secondOperand),
            "-" => Calculator.Subtract(_firstOperand.Value, secondOperand),
            "*" => Calculator.Multiply(_firstOperand.Value, secondOperand),
            "/" => Calculator.Divide(_firstOperand.Value, secondOperand),
            "^" => Calculator.Power(_firstOperand.Value, secondOperand),
            _ => throw new InvalidOperationException("Operacion i panjohur.")
        };

        SetDisplayValue(result);
        _firstOperand = null;
        _pendingBinaryOperation = null;
        _shouldClearOnNextDigit = true;
    }

    private void ApplyUnary(Func<double, double> operation)
    {
        try
        {
            var result = operation(ReadDisplayValue());
            SetDisplayValue(result);
            _shouldClearOnNextDigit = true;
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }

    private void ApplyFactorial()
    {
        try
        {
            var value = ReadDisplayValue();
            if (Math.Abs(value - Math.Round(value)) > 1e-10)
            {
                throw new InvalidOperationException("Faktoriali punon vetëm me numra të plotë.");
            }

            var result = Calculator.Factorial((int)Math.Round(value));
            SetDisplayValue(result);
            _shouldClearOnNextDigit = true;
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }

    private void ClearAll()
    {
        _firstOperand = null;
        _pendingBinaryOperation = null;
        _shouldClearOnNextDigit = false;
        _display.Text = "0";
    }

    private double ReadDisplayValue()
    {
        if (double.TryParse(_display.Text, NumberStyles.Float, CultureInfo.CurrentCulture, out var current))
        {
            return current;
        }

        if (double.TryParse(_display.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out current))
        {
            return current;
        }

        throw new FormatException("Vlerë e pavlefshme në ekran.");
    }

    private void SetDisplayValue(double value)
    {
        _display.Text = value.ToString("G15", CultureInfo.CurrentCulture);
    }

    private void ShowError(string message)
    {
        MessageBox.Show(message, "Gabim", MessageBoxButtons.OK, MessageBoxIcon.Error);
        _display.Text = "0";
        _firstOperand = null;
        _pendingBinaryOperation = null;
        _shouldClearOnNextDigit = false;
    }
}
