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
}

