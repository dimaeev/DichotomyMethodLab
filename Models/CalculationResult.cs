namespace DichotomyMethodLab.Models
{
  public class CalculationResult
  {
    public double X { get; set; }           // Найденный корень
    public double Fx { get; set; }          // Значение функции в корне
    public int Iterations { get; set; }     // Количество итераций
    public bool IsConverged { get; set; }   // Сошелся ли алгоритм
    public string ErrorMessage { get; set; } // Сообщение об ошибке
  }
}