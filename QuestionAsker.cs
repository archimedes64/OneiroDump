using System;
using System.Collections.Generic;

namespace OneiroDump
{
  public class QuestionAsker
  {

    public Answer AskQuestion(Question question)
    {
      var answer = new Answer();
      switch(question.Type)
      {
        case "yes_no":
          answer.Value = AskYesNo(question.Text);
          if (question.SubQuestions == null)
          {
            break;
          }

          if (question.SubQuestions.Yes != null && (bool)answer.Value == true)
          {
            answer.SubAnswers = new Dictionary<string, Answer>();
            foreach (Question subQuestion in question.SubQuestions.Yes)
            {
              answer.SubAnswers[subQuestion.Id] = AskQuestion(subQuestion);
            }
          }
          else if (question.SubQuestions.No != null && (bool)answer.Value == false)
          {
            answer.SubAnswers = new Dictionary<string, Answer>();
            foreach (Question subQuestion in question.SubQuestions.No)
            {
              answer.SubAnswers[subQuestion.Id] = AskQuestion(subQuestion);
            }

          }
          break;
        case "int":
          answer.Value = AskInt(question.Text, question.Min, question.Max);
          if (question.AskForCount == null)
          {
            break;
          }


          answer.SubAnswers = new Dictionary<string, Answer>();
          for (int i = 0; i < (int)answer.Value; i++)
          {
            var subAnswer = new Answer();
            subAnswer.SubAnswers = new Dictionary<string, Answer>();
            answer.SubAnswers[$"{i + 1}"] = subAnswer;

            foreach (Question subQuestion in question.AskForCount)
            {
              answer.SubAnswers[$"{i + 1}"].SubAnswers[subQuestion.Id] = AskQuestion(subQuestion);
            }

          }
          break;

        case "float":
          answer.Value = AskFloat(question.Text, question.Min, question.Max);
          break;

        case "time":
          answer.Value = AskTime(question.Text);
          break;
        case "string":
          answer.Value = AskString(question.Text);
          break;
        case "enum":
          answer.Value = AskEnum(question.Text, question.Answers);
          break;
      }

      return answer;
    }

    private string invalid_data_message = "Invalid input.";

    private string GetInput(string message, string indicator)
    {
      Console.WriteLine(message);
      Console.Write(indicator);
      var input = Console.ReadLine();

      if (input == null) // I honestly dont know how to make readline return null, but I was getting a warning when compiling
      {
        input = "";
      }

      return input;
    }

    private bool AskYesNo(string questionText)
    {
      // return true for yes, false for no
      string indicator = "(Y/N): ";
      HashSet<string> validYes = new HashSet<string> {"yes","y"}; // using a set may be overkill for only 2 values
      HashSet<string> validNo = new HashSet<string> {"no","n"};

      string input = GetInput(questionText, indicator);

      while (true){
        if (validYes.Contains(input.ToLower()))
        {
          return true;
        }
        else if (validNo.Contains(input.ToLower()))
        {
          return false;
        }

        Console.WriteLine(this.invalid_data_message);
        input = GetInput(message: "Please enter either \"Y\" or \"N\"", indicator: indicator);

      }
    }
    private int AskInt(string questionText, double? min = null, double? max = null)
    {
      string range_indicator = "";
      string range_in_text = "";
      if (min != null && max != null)
      {
        range_indicator = $" {min} - {max}";
        range_in_text = $"between (or equal to) {min} and {max}";
      }
      else if (min != null)
      {
        range_indicator = $" >= {min}";
        range_in_text = $"greater than or equal to {min}";
      }
      else if (max != null)
      {
        range_indicator = $" <= {max}";
        range_in_text = $"less than or equal to {max}";
      }
      string indicator = $"(Whole Number{range_indicator}): ";
      string input = GetInput(questionText, indicator);
      while (true)
      {
        if (int.TryParse(input, out int result)) // https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/types/how-to-convert-a-string-to-a-number
        {
          if ((min == null || result >= min) && (max == null || result <= max))
          {
            return result;
          }
          input = GetInput(message: $"Number must be {range_in_text}.", indicator: indicator);
        }
        else
        {
          Console.WriteLine(this.invalid_data_message);
          input = GetInput(message: $"Please enter a whole number.", indicator: indicator);
        }
      }
    }
    private double AskFloat(string questionText, double? min = null, double? max = null)
    {
      string indicator = "";
      string range_in_text = "";
      if (min != null && max != null)
      {
        indicator = $"({min} - {max}): ";
        range_in_text = $"between (or equal to) {min} and {max}";
      }
      else if (min != null)
      {
        range_in_text = $"greater than or equal to {min}";
        indicator = $"({range_in_text}): ";
      }
      else if (max != null)
      {
        range_in_text = $"less than or equal to {max}";
        indicator = $"({range_in_text}): ";
      }
      string input = GetInput(questionText, indicator);
      while (true)
      {
        if (double.TryParse(input, out double result)) // https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/types/how-to-convert-a-string-to-a-number
        {
          if ((min == null || result >= min) && (max == null || result <= max))
          {
            return result;
          }
          input = GetInput(message: $"Number must be {range_in_text}.", indicator: indicator);
        }
        else
        {
          Console.WriteLine(this.invalid_data_message);
          input = GetInput(message: $"Please enter a number.", indicator: indicator);
        }
      }
    }
    private string AskTime(string questionText)
    {
      string indicator = "(HH:MM. Use military/24 hour time): ";
      string input = GetInput(questionText, indicator);
      while (true)
      {
        int colon_count = input.Split(":").Length - 1; // https://stackoverflow.com/questions/541954/how-to-count-occurrences-of-a-char-string-within-a-string
        if (colon_count == 0)
        {
          Console.WriteLine(this.invalid_data_message);
          input = GetInput(message: "Make sure you include a colon to seperate the hours and minutes", indicator: indicator);
          continue;
        }
        else if (colon_count > 1)
        {
          Console.WriteLine(this.invalid_data_message);
          input = GetInput(message: "Make sure you only include one colon", indicator: indicator);
          continue;
        }

        string[] time = input.Split(':');
        string hour = time[0];
        string minute = time[1];

        if (minute.Length != 2 || hour.Length != 2) 
        {
          Console.WriteLine(this.invalid_data_message);
          input = GetInput(message: $"Make sure that both the hours and minutes are two digits long", indicator: indicator);
          continue;
        }

        if (int.TryParse(hour, out int hour_int) && int.TryParse(minute, out int minute_int)) // check if they are numbers and within the valid range
        {
          if (hour_int < 0 || hour_int > 23)
          {
            Console.WriteLine(this.invalid_data_message);
            input = GetInput(message: $"Hour must be between 00 and 23", indicator: indicator);
            continue;
          }

          if (minute_int < 0 || minute_int > 59)
          {
            Console.WriteLine(this.invalid_data_message);
            input = GetInput(message: $"Minute must be between 00 and 59", indicator: indicator);
            continue;
          }
        }
        else // not numbers. user inputed completely bad data.
        {
          Console.WriteLine(this.invalid_data_message);
          input = GetInput(message: "", indicator: indicator);
          continue;
        }
        return input;
      }
    }
    private string AskString(string questionText)
    {
      return GetInput(questionText, ": ");
    }
    private string AskEnum(string questionText, string[] options)
    {
      Console.WriteLine(questionText);
      string indicator = $"(1 - {options.Length}): ";
      int i = 1;
      Console.WriteLine("");
      Console.WriteLine("Options:");
      foreach (string option in options)
      {
        Console.WriteLine($"  {i}. {option}");
        i++;
      }
      string input = GetInput("", indicator);
      while (true)
      {
        if (int.TryParse(input, out int result))
        {
          if (result >= 1 && result <= options.Length)
          {
            return options[result - 1];
          }
        }
        Console.WriteLine(this.invalid_data_message);
        input = GetInput(message: $"Please enter a number between 1 and {options.Length}", indicator: indicator);
      }
    }
  }
}
