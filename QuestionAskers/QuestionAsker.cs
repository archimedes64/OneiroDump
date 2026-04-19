using System;
using System.Collections.Generic;

namespace OneiroDump
{
  public class QuestionAsker
  {
    private static Dictionary<string, BaseQuestionAsker> questionAskers = new Dictionary<string, BaseQuestionAsker>
    {
      {"yes_no", new YesNoQuestionAsker()},
      {"float", new FloatQuestionAsker()},
      {"int", new IntQuestionAsker()},
      {"string", new StringQuestionAsker()},
      {"time", new TimeQuestionAsker()},
      {"enum", new EnumQuestionAsker()}
    };
    public static Answer AskQuestion(Question question)
    {
      if (!questionAskers.ContainsKey(question.Type))
      {
        throw new ArgumentException($"Invalid question type: {question.Type}");
      }

      return questionAskers[question.Type].AskQuestion(question);
    }
  }
}
