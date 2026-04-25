using System;
namespace OneiroDump
{
  public class EnumQuestionAsker : BaseQuestionAsker
  {
    public override string QuestionType { get { return "enum"; } }

    private string[] options;

    protected override string MakeIndicator(Question question)
    {
      string indicator = "Options:";

      int i = 1;
      foreach (string option in options)
      {
        indicator += $"\n  {i}. {option}";
        i++;
      }

      indicator += $"\n(1 - {question.Answers.Length}): ";

      return indicator;
    }

    protected override Answer ConvertStringToAnswer(string userInput)
    {
      int answerIndex = int.Parse(userInput) - 1;
      return new Answer {Value = options[answerIndex]};
    }

    public override Answer AskQuestion(Question question)
    {
      options = question.Answers;
      return GetAnswer(question);
    }

    protected override (bool isValid, string error) IsValidAnswer(string answer, Question question)
    {
      if (int.TryParse(answer, out int result))
      {
        if (result >= 1 && result <= options.Length)
        {
          return (true, "");
        }
      }

      return (false, $"Your answer must be a number between 1 and {options.Length}");
    }
    public EnumQuestionAsker(QuestionAsker questionAsker) : base(questionAsker) {}
  }
}
