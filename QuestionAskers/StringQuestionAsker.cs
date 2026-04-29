using System;

namespace OneiroDump
{
  public class StringQuestionAsker : BaseQuestionAsker
  {
    public override string QuestionType { get { return "string"; } }

    protected override string MakeIndicator(Question question)
    {
      return ": ";
    }

    protected override (bool isValid, string error) IsValidAnswer(string answer, Question question)
    {
      return (true, ""); // whether its a string is already validated by GetInput
    }

    public StringQuestionAsker(QuestionAsker questionAsker) : base(questionAsker) {}
  }
}

