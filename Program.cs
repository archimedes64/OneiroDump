// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;

namespace OneiroDump
{
  public class Program
  {
    public static void Main(string[] args)
    {

      ConfigLoader configLoader = new ConfigLoader();
      Config config = configLoader.LoadConfig("config/config.yaml");
      Question[] questions = configLoader.LoadQuestions(config.GeneralQuestions);
      QuestionAsker questionAsker = new QuestionAsker();
      var answers = new Dictionary<string, Answer>();
      foreach (Question question in questions) {
        answers[question.Id] = questionAsker.AskQuestion(question);
      }
      foreach (var answer in answers)
      {
        Console.WriteLine($"{answer.Key}: {answer.Value}");
      }
      
    }
  }
}
