using System;
using System.Collections.Generic;

namespace OneiroDump
{
  public class YesNoQuestionAsker : BaseQuestionAsker
  {
    private HashSet<string> validYes = new HashSet<string> {"yes","y"};
    private HashSet<string> validNo = new HashSet<string> {"no","n"};

    protected override string MakeIndicator(Question question)
    {
      return "(Y/N): ";
    }

    protected override (bool isValid, string error) IsValidAnswer(string answer, Question question)
    {
      if (!(validYes.Contains(answer.ToLower()) || validNo.Contains(answer.ToLower())))
      {
        return (false, "Please enter either \"Y\" or \"N\"");
      }
      return (true, "");
    }

    protected override Answer ConvertStringToAnswer(string userInput)
    {
      if (validYes.Contains(userInput.ToLower()))
      {
        return new Answer {Value = true};
      }
      else if (validNo.Contains(userInput.ToLower()))
      {
        return new Answer {Value = false};
      }
      else
      {
        throw new ArgumentException("Invalid input. Make sure this function is only called after validating the input with IsValidAnswer.");
      }
    }
    public override Answer AskQuestion(Question question)
    {
      Answer answer = GetAnswer(question);

      if (question.SubQuestions == null)
      {
        return answer;
      }

      if (question.SubQuestions.Yes != null && (bool)answer.Value == true)
      {
        answer.SubAnswers = new Dictionary<string, Answer>();
        foreach (Question subQuestion in question.SubQuestions.Yes)
        {
          answer.SubAnswers[subQuestion.Id] = QuestionAsker.AskQuestion(subQuestion);
        }
      }
      else if (question.SubQuestions.No != null && (bool)answer.Value == false)
      {
        answer.SubAnswers = new Dictionary<string, Answer>();
        foreach (Question subQuestion in question.SubQuestions.No)
        {
          answer.SubAnswers[subQuestion.Id] = QuestionAsker.AskQuestion(subQuestion);
        }

      }

      return answer;
    }
  }
}
