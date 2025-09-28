using System;
using System.Data;

namespace DichotomyMethodLab.Core
{
  public static class FunctionParser
  {
    public static double Calculate(string function, double x)
    {
      try
      {
        // Заменяем x на значение
        string expression = function.Replace("x",
            x.ToString(System.Globalization.CultureInfo.InvariantCulture));

        // Заменяем запятые на точки для корректных вычислений
        expression = expression.Replace(',', '.');

        DataTable table = new DataTable();
        table.Columns.Add("expression", typeof(string), expression);

        DataRow row = table.NewRow();
        table.Rows.Add(row);

        return Convert.ToDouble(row["expression"]);
      }
      catch (Exception ex)
      {
        throw new ArgumentException($"Ошибка вычисления функции: {ex.Message}");
      }
    }
  }
}