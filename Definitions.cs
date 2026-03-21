using System.Collections.Generic;
namespace OneiroDump
{
  public class Question
  {
    public required string Id {get; set;}
    public required string Text {get; set;}
    public required string Type {get; set;}

    public SubQuestions? SubQuestions {get; set;}

    public List<string>? Answers {get; set;}

    public double? Min {get; set;}
    public double? Max {get; set;}

    public List<Question>? AskForCount {get; set;}

  }
  public class SubQuestions
  {
    public List<Question>? Yes {get; set;}
    public List<Question>? No {get; set;}
  }
  public class Config
  {
    public bool DreamReports {get; set;}
    public bool Fitbit {get; set;}
    public string GeneralQuestions {get; set;}
    public bool StoreInDb {get; set;}
  }
}

