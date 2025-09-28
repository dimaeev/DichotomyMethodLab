using System;
using System.Windows.Forms.DataVisualization.Charting;
using DichotomyMethodLab.Core;

namespace DichotomyMethodLab.Services
{
  public static class ChartService
  {
    public static void PlotFunction(Chart chart, string function, double a, double b, double step = 0.01)
    {
      // Очищаем график
      chart.Series.Clear();
      chart.ChartAreas.Clear();
      chart.Legends.Clear();

      // Создаем область для графика
      ChartArea chartArea = new ChartArea();
      chartArea.AxisX.Title = "x";
      chartArea.AxisY.Title = "f(x)";
      chart.ChartAreas.Add(chartArea);

      // Создаем легенду
      Legend legend = new Legend();
      chart.Legends.Add(legend);

      // График функции
      Series functionSeries = new Series("f(x)");
      functionSeries.ChartType = SeriesChartType.Line;
      functionSeries.BorderWidth = 2;
      functionSeries.Color = System.Drawing.Color.Blue;

      // Линия y = 0
      Series zeroLine = new Series("y = 0");
      zeroLine.ChartType = SeriesChartType.Line;
      zeroLine.BorderWidth = 1;
      zeroLine.Color = System.Drawing.Color.Gray;
      zeroLine.BorderDashStyle = ChartDashStyle.Dash;

      // Заполняем график точками
      for (double x = a; x <= b; x += step)
      {
        try
        {
          double y = FunctionParser.Calculate(function, x);
          if (!double.IsNaN(y) && !double.IsInfinity(y))
          {
            functionSeries.Points.AddXY(x, y);
          }
        }
        catch
        {
          // Пропускаем точки, где функция не определена
        }

        zeroLine.Points.AddXY(x, 0);
      }

      chart.Series.Add(functionSeries);
      chart.Series.Add(zeroLine);
    }

    public static void HighlightRoot(Chart chart, double x, double y)
    {
      Series rootSeries = new Series("Корень");
      rootSeries.ChartType = SeriesChartType.Point;
      rootSeries.MarkerSize = 10;
      rootSeries.MarkerStyle = MarkerStyle.Circle;
      rootSeries.Color = System.Drawing.Color.Red;
      rootSeries.Points.AddXY(x, y);

      chart.Series.Add(rootSeries);
    }
  }
}