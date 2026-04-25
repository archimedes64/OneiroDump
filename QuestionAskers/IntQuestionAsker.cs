using System;
namespace OneiroDump
{
  public class IntQuestionAsker : BaseQuestionAsker
  {
    public override string QuestionType { get { return "int"; } }

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
      return "(Whole Number): ";
    }

    protected override (bool isValid, string error) IsValidAnswer(string answer, Question question)
    {
      string rangeInText = GetRangeInText(question);

      var min = question.Min;
      var max = question.Max;


      if (int.TryParse(answer, out int result)) // https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/types/how-to-convert-a-string-to-a-number
      {
        if ((min == null || result >= min) && (max == null || result <= max))
        {
          return (true, "");
        }
          return (false, $"Number must be {rangeInText}.");
      }
      else
      {
        return (false, "Answer must be a whole number.");
      }

    }

    protected override Answer ConvertStringToAnswer(string userInput)
    {
      return new Answer { Value = int.Parse(userInput) };
    }

    public override Answer AskQuestion(Question question)
    {
      Answer answer = GetAnswer(question);

      if (question.AskForCount == null)
      {
        return answer;
      }

      answer.SubAnswers = new Dictionary<string, Answer>();
      for (int i = 0; i < (int)answer.Value; i++)
      {
        var subAnswer = new Answer();
        subAnswer.SubAnswers = new Dictionary<string, Answer>();
        answer.SubAnswers[$"{i + 1}"] = subAnswer;

        foreach (Question subQuestion in question.AskForCount)
        {
          answer.SubAnswers[$"{i + 1}"].SubAnswers[subQuestion.Id] = questionAsker.AskQuestion(subQuestion);
        }

      }
      return answer;
    }
    public IntQuestionAsker(QuestionAsker questionAsker) : base(questionAsker) {}
  }
}

