using System.Collections.Generic;
namespace OneiroDump
{
  public class Question
  {
    public required string Id {get; set;}
    public required string Text {get; set;}
    public required string Type {get; set;}

    public SubQuestions? SubQuestions {get; set;}

    public string[]? Answers {get; set;}

    public double? Min {get; set;}
    public double? Max {get; set;}

    public Question[]? AskForCount {get; set;}

  }
  public class SubQuestions
  {
    public Question[]? Yes {get; set;}
    public Question[]? No {get; set;}
  }
  public class Config
  {
    public bool DreamReports {get; set;} = true;
    public bool Fitbit {get; set;} = false;
    public required string GeneralQuestions {get; set;}
    public bool StoreInDb {get; set;} = false;
  }
  public class Answer
  {
    public object Value {get; set;} // the answer can be a lot of different types https://stackoverflow.com/questions/5886875/let-method-take-any-data-type-in-c-sharp
    public Dictionary<string, Answer>? SubAnswers {get; set;}
    override public string ToString()
    {
      if (SubAnswers != null)
      {
        return $"Value: {Value}, SubAnswers: {string.Join(", ", SubAnswers)}";
      }
      else
      {
        return $"Value: {Value}";
      }
    }
  }
}

