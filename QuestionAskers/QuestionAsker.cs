using System;
using System.Collections.Generic;

namespace OneiroDump
{
  public class QuestionAsker
  {
    private Dictionary<string, BaseQuestionAsker> questionAskers = new Dictionary<string, BaseQuestionAsker> {};

    public Answer AskQuestion(Question question)
    {
      if (!questionAskers.ContainsKey(question.Type))
      {
        throw new ArgumentException($"Question asker for type {question.Type} already exists.");
      }

      return questionAskers[question.Type].AskQuestion(question);
    }

    public void AddQuestionAsker(BaseQuestionAsker questionAsker)
    {
      string questionType = questionAsker.QuestionType;
      if (questionAskers.ContainsKey(questionType))
      {
        throw new ArgumentException($"Question asker for type {questionType} already exists.");
      }

      questionAskers[questionType] = questionAsker;
    }

    public void AddQuestionAskers(BaseQuestionAsker[] questionAskers)
    {
      foreach (BaseQuestionAsker questionAsker in questionAskers)
      {
        AddQuestionAsker(questionAsker);
      }
    }

  }
}
