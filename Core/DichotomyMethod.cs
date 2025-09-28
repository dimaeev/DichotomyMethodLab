using System;
using DichotomyMethodLab.Models;

namespace DichotomyMethodLab.Core
{
  public static class DichotomyMethod
  {
    public static CalculationResult FindRoot(string function, double a, double b, double epsilon, int maxIterations = 1000)
    {
      // Проверка входных данных
      if (string.IsNullOrWhiteSpace(function))
        return new CalculationResult { ErrorMessage = "Функция не может быть пустой" };

      if (epsilon <= 0)
        return new CalculationResult { ErrorMessage = "Точность (e) должна быть положительным числом" };

      if (b <= a)
        return new CalculationResult { ErrorMessage = "Правая граница (b) должна быть больше левой (a)" };

      try
      {
        // Проверяем, что на концах отрезка функция имеет разные знаки
        double fa = FunctionParser.Calculate(function, a);
        double fb = FunctionParser.Calculate(function, b);

        if (double.IsNaN(fa) || double.IsInfinity(fa) ||
            double.IsNaN(fb) || double.IsInfinity(fb))
          return new CalculationResult { ErrorMessage = "Функция не определена на концах отрезка" };

        if (fa * fb > 0)
          return new CalculationResult { ErrorMessage = "Функция должна иметь разные знаки на концах отрезка [a, b]" };

        int iteration = 0;

        while (Math.Abs(b - a) > epsilon && iteration < maxIterations)
        {
          iteration++;

          double x = (a + b) / 2;
          double fx = FunctionParser.Calculate(function, x);

          // Если нашли точный корень
          if (Math.Abs(fx) < epsilon)
          {
            return new CalculationResult
            {
              X = x,
              Fx = fx,
              Iterations = iteration,
              IsConverged = true
            };
          }

          // Выбираем подотрезок, где функция меняет знак
          if (fa * fx < 0)
            b = x;
          else
            a = x;
        }

        double finalX = (a + b) / 2;
        double finalFx = FunctionParser.Calculate(function, finalX);

        return new CalculationResult
        {
          X = finalX,
          Fx = finalFx,
          Iterations = iteration,
          IsConverged = iteration < maxIterations
        };
      }
      catch (Exception ex)
      {
        return new CalculationResult { ErrorMessage = $"Ошибка вычисления: {ex.Message}" };
      }
    }
  }
}