using System;

namespace OneiroDump
{
  public class StringQuestionAsker : BaseQuestionAsker
  {
    protected override string MakeIndicator(Question question)
    {
      return ": ";
    }
    protected override (bool isValid, string error) IsValidAnswer(string answer, Question question)
    {
      return (true, ""); // whether its a string is already validated by GetInput
    }
  }
}

