# Scientific Calculator (C#) - Visual Studio Solution

Ky projekt tash është **Windows Forms App** (jo console), i ndërtuar për Visual Studio me file `.sln`.

## Si me hap në Visual Studio
1. Hap Visual Studio.
2. Zgjedh **Open a project or solution**.
3. Hap file-in `ScientificCalculator.sln`.
4. Shtyp **Start** (`F5`) ose `Ctrl + F5` për me ekzekutu.

## Funksionalitetet
- Operacione bazike: `+`, `-`, `*`, `/`
- Operacione shkencore:
  - `sin`, `cos`, `tan` (në gradë)
  - `log10`, `ln`
  - `sqrt`, `x²`, `xʸ`
  - `n!`, `1/x`, `%`, `±`
- Butona ndihmës: `C`, `←`, `=`

## Struktura
- `ScientificCalculator.sln` — Solution për Visual Studio.
- `ScientificCalculator/ScientificCalculator.csproj` — Projekt WinForms (`net8.0-windows`).
- `ScientificCalculator/Calculator.cs` — Logjika matematikore.
- `ScientificCalculator/CalculatorForm.cs` — UI dhe event handling i kalkulatorit.
- `ScientificCalculator/Program.cs` — Entry point i aplikacionit WinForms.
