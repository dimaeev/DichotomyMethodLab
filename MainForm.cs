using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using DichotomyMethodLab.Core;
using DichotomyMethodLab.Models;
using DichotomyMethodLab.Services;

namespace DichotomyMethodLab
{
  public partial class MainForm : Form
  {
    public MainForm()
    {
      InitializeComponent();
      SetupInitialValues();
    }

    private void SetupInitialValues()
    {
      // Установка начальных значений для тестирования
      txtA.Text = "0";
      txtB.Text = "2";
      txtE.Text = "0.001";
      txtFunction.Text = "x*x - 2"; // Уравнение: x² - 2 = 0, корень ~1.414

      statusLabel.Text = "Готов к работе";
    }

    // ОБРАБОТЧИКИ СОБЫТИЙ МЕНЮ
    private void рассчитатьToolStripMenuItem_Click(object sender, EventArgs e)
    {
      CalculateRoot();
    }

    private void очиститьToolStripMenuItem_Click(object sender, EventArgs e)
    {
      ClearForm();
    }

    private void выходToolStripMenuItem_Click(object sender, EventArgs e)
    {
      Application.Exit();
    }

    private void CalculateRoot()
    {
      statusLabel.Text = "Вычисление...";

      // Проверяем входные данные
      if (!ValidateInputs(out double a, out double b, out double epsilon, out string function))
        return;

      try
      {
        // Строим график функции
        ChartService.PlotFunction(chartFunction, function, a, b);

        // Ищем корень уравнения
        CalculationResult result = DichotomyMethod.FindRoot(function, a, b, epsilon);

        // Обрабатываем результат
        if (!string.IsNullOrEmpty(result.ErrorMessage))
        {
          MessageBox.Show(result.ErrorMessage, "Ошибка",
              MessageBoxButtons.OK, MessageBoxIcon.Error);
          statusLabel.Text = "Ошибка вычисления";
          return;
        }

        if (result.IsConverged)
        {
          // Формируем текст результата
          string resultText = $"Корень уравнения найден!\r\n";
          resultText += $"x = {result.X:F6}\r\n";
          resultText += $"f(x) = {result.Fx:F6}\r\n";
          resultText += $"Количество итераций: {result.Iterations}";

          // Показываем результат
          MessageBox.Show(resultText, "Результат",
              MessageBoxButtons.OK, MessageBoxIcon.Information);

          // Выделяем корень на графике
          ChartService.HighlightRoot(chartFunction, result.X, result.Fx);

          statusLabel.Text = "Расчет завершен успешно";
        }
        else
        {
          MessageBox.Show("Алгоритм не сошелся за максимальное число итераций",
              "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
          statusLabel.Text = "Алгоритм не сошелся";
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Неожиданная ошибка: {ex.Message}", "Ошибка",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
        statusLabel.Text = "Ошибка выполнения";
      }
    }

    private bool ValidateInputs(out double a, out double b, out double epsilon, out string function)
    {
      a = b = epsilon = 0;
      function = txtFunction.Text.Trim();

      // Проверяем что функция не пустая
      if (string.IsNullOrEmpty(function))
      {
        MessageBox.Show("Введите функцию f(x)", "Ошибка ввода",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
        statusLabel.Text = "Ошибка: не введена функция";
        return false;
      }

      // Парсим числовые значения
      bool success = double.TryParse(txtA.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out a);
      success &= double.TryParse(txtB.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out b);
      success &= double.TryParse(txtE.Text, NumberStyles.Float, CultureInfo.InvariantCulture, out epsilon);

      if (!success)
      {
        MessageBox.Show("Проверьте правильность введенных числовых значений (a, b, e).\nИспользуйте точку как разделитель дробной части.",
            "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
        statusLabel.Text = "Ошибка: некорректные числовые значения";
        return false;
      }

      return true;
    }

    private void ClearForm()
    {
      // Очищаем все поля
      txtA.Text = "";
      txtB.Text = "";
      txtE.Text = "";
      txtFunction.Text = "";

      // Очищаем график
      chartFunction.Series.Clear();
      chartFunction.ChartAreas.Clear();

      statusLabel.Text = "Поля очищены";
    }
  }
}