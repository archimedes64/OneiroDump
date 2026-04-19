using System;
namespace OneiroDump
{
  public class FloatQuestionAsker : BaseQuestionAsker
  {

    private string GetRangeInText(Question question)
    {
      var min = question.Min;
      var max = question.Max;

      if (min != null && max != null)
      {
        return $"between {min} and {max}";
      }
      else if (min != null)
      {
        return $"greater than or equal to {min}";
        
      }
      else if (max != null)
      {
        return $"less than or equal to {max}";
      }
      
      return "";

    }
    protected override string MakeIndicator(Question question)
    {
      string rangeInText = GetRangeInText(question);

      if (rangeInText != "")
      {
        return $"({rangeInText}): ";
      }
      return "(Number): ";
    }

    protected override (bool isValid, string error) IsValidAnswer(string answer, Question question)
    {
      string rangeInText = GetRangeInText(question);

      var min = question.Min;
      var max = question.Max;


      if (double.TryParse(answer, out double result)) // https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/types/how-to-convert-a-string-to-a-number
      {
        if ((min == null || result >= min) && (max == null || result <= max))
        {
          return (true, "");
        }
          return (false, $"Number must be {rangeInText}.");
      }
      else
      {
        return (false, "Answer must be a number.");
      }

    }

    protected override Answer ConvertStringToAnswer(string userInput)
    {
      return new Answer { Value = double.Parse(userInput) };
    }
  }
}
