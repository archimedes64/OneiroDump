using System;

namespace OneiroDump
{
  public abstract class BaseQuestionAsker
  {
    // https://www.c-sharpcorner.com/UploadFile/3d39b4/abstract-class-in-C-Sharp/
    protected QuestionAsker questionAsker;

    protected abstract (bool isValid, string error) IsValidAnswer(string answer, Question question);
    
    protected abstract string MakeIndicator(Question question); // a hint to the user on how they should input. ex: "(Y/N)" and "(0-10)"

    public abstract string QuestionType { get; } // corresponds to one in the config. "yes_no", "int", "float", "string", "enum", "time".

    protected virtual Answer ConvertStringToAnswer(string userInput)
    {
      return new Answer {Value = userInput};
    }

    private string GetInput(Question question)
    {
      Console.WriteLine(question.Text);
      Console.Write(MakeIndicator(question));
      var input = Console.ReadLine();
      if (input == null)
      {
        Console.WriteLine("Input cannot be null.");
        return GetInput(question);
      }

      return input;
    }


    public virtual Answer AskQuestion(Question question) // allows children to handle subquestions by overriding this
    {
      return GetAnswer(question);
    }


    protected Answer GetAnswer(Question question)
    {
      
      string userInput = GetInput(question);
      (bool isValid, string error) = IsValidAnswer(userInput, question);

      if (!isValid)
      {
        Console.WriteLine($"Invalid input: {error}.");
        return GetAnswer(question);
      }

      return ConvertStringToAnswer(userInput);

    }

    public BaseQuestionAsker(QuestionAsker questionAsker)
    {
      this.questionAsker = questionAsker;
    }

  }
}
