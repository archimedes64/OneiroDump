using System;

namespace OneiroDump
{
  public class TimeQuestionAsker : BaseQuestionAsker
  {
    protected override string MakeIndicator(Question question)
    {
      return "(HH:MM. Use military/24 hour time): ";
    }

    protected override (bool isValid, string error) IsValidAnswer(string answer, Question question)
    {
      int colon_count = answer.Split(":").Length - 1; // https://stackoverflow.com/questions/541954/how-to-count-occurrences-of-a-char-string-within-a-string

      if (colon_count == 0) // no colon 
      {
        return (false, "You must include a colon that seperates the hours and minutes");
      }

      else if (colon_count > 1) // too many colons
      {
        return (false, "You must have only one colon");
      }

      string[] time = answer.Split(':');
      string hour = time[0];
      string minute = time[1];

      if (minute.Length != 2 || hour.Length != 2) // too litle or too many digits
      {
        return (false, $"Make sure that both the hours and minutes are two digits long");
      }

      if (int.TryParse(hour, out int hour_int) && int.TryParse(minute, out int minute_int)) // check if they are numbers and within the valid range
      {
        if (hour_int < 0 || hour_int > 23) // invalid amount of hours
        {
          return (false, $"Hour must be between 00 and 23");
        }

        if (minute_int < 0 || minute_int > 59) // invalid amount of minutes
        {
          return (false, $"Minute must be between 00 and 59");
        }
      }
      else // not numbers. user inputed completely bad data.
      {
        return (false, "Hours and minutes must be numbers");
      }

      return (true, "");
    }
  }
}
