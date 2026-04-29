using System.Collections.Generic;
namespace OneiroDump
{
  public class Question
  {
    // all questions
    public required string Id {get; set;}
    public required string Text {get; set;}
    public required string Type {get; set;}

    // yes_no
    public SubQuestions? SubQuestions {get; set;}

    // enums
    public string[]? Answers {get; set;}

    // ints and floats
    public double? Min {get; set;}
    public double? Max {get; set;}

    // ints
    public Question[]? AskForCount {get; set;}
  }

  public class SubQuestions
  {
    public Question[]? Yes {get; set;}
    public Question[]? No {get; set;}
  }

  public class Config
  {
    public required string Questions {get; set;}
    public required string SaveLocation {get; set;}
  }

  public class Answer
  {
    public object Value {get; set;} // the answer can be a lot of different types https://stackoverflow.com/questions/5886875/let-method-take-any-data-type-in-c-sharp
    public Dictionary<string, Answer>? SubAnswers {get; set;}

    override public string ToString()
    {
      if (SubAnswers != null)
      {
        List<string> subAnswers = new List<string>();
        foreach (var subAnswer in SubAnswers)
        {
          subAnswers.Add(subAnswer.Key + ": " + subAnswer.Value);
        }
        return $"Value: {Value}, SubAnswers: [{string.Join(", ", subAnswers)}]";
      }
      else
      {
        return $"Value: {Value}";
      }
    }
  }
}

