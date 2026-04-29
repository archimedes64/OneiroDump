using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace OneiroDump
{
  public class Program
  {
    public static void Main(string[] args)
    {
      // setup
      ConfigLoader configLoader = new ConfigLoader();
      Config config = configLoader.LoadConfig("config/config.yaml"); // TODO: don't hard code this
      Question[] questions = configLoader.LoadQuestions(config.Questions);

      QuestionAsker questionAsker = new QuestionAsker();

      questionAsker.AddQuestionAskers(new BaseQuestionAsker[] {
        new YesNoQuestionAsker(questionAsker),
        new EnumQuestionAsker(questionAsker),
        new FloatQuestionAsker(questionAsker),
        new StringQuestionAsker(questionAsker),
        new IntQuestionAsker(questionAsker),
        new TimeQuestionAsker(questionAsker)
      });

      // ask questions
      var answers = new Dictionary<string, Answer>();


      foreach (Question question in questions) {
        answers[question.Id] = questionAsker.AskQuestion(question);
      }

      // store answers
      var generalInfo = new Dictionary<string, Answer>();

      var dreams = new Dictionary<string, Dictionary<string, Answer>>();

      foreach (var answer in answers)
      {
        Console.WriteLine($"{answer.Key}: {answer.Value}");

        if (answer.Key != "dreams")
        {
          generalInfo[answer.Key] = answer.Value;
          continue;
        }

        foreach (var subAnswer in answer.Value.SubAnswers)
        {
          string dreamNumber = subAnswer.Key;

          string title = dreamNumber;

          if (subAnswer.Value.SubAnswers.ContainsKey("dream_title"))
          {
            title = (string)subAnswer.Value.SubAnswers["dream_title"].Value; // this is a string because this special id must be a string
          }

          dreams[title] = subAnswer.Value.SubAnswers;
        }
      }

      // Comfirm Input
      Console.WriteLine("Here are your answers.");

      Console.WriteLine("\nGeneral Information: ");
      foreach (var answer in generalInfo)
      {
        Console.WriteLine($"  {answer.Key}: {answer.Value}");
      }

      Console.WriteLine("\nDreams: ");
      foreach (var dream in dreams)
      {
        Console.WriteLine($"  {dream.Key}: ");

        foreach (var dreamAnswers in dream.Value)
        {
          Console.WriteLine($"    {dreamAnswers.Key}: {dreamAnswers.Value}");
        }
      }
      bool do_save = (bool)questionAsker.AskQuestion(new Question{Id = "comfirm", Type = "yes_no", Text = "Do you want to save these answers?"}).Value;

      if (!do_save)
      {
        Main(args);
        return;
      }

      // save text

      // https://dirask.com/posts/C-NET-get-current-year-month-day-hour-minute-second-millisecond-Kj85L1
      DateTime now = DateTime.Now;

      //https://zetcode.com/csharp/system-io-path/
      string dir = Path.Combine(config.SaveLocation, $"{now.Year}/{now.Month}/{now.Day}");
      
      Directory.CreateDirectory(dir); //https://learn.microsoft.com/en-us/dotnet/api/system.io.directory.createdirectory?view=net-10.0

      //https://github.com/aaubry/YamlDotNet?tab=readme-ov-file
      var serializer = new SerializerBuilder()
        .WithNamingConvention(CamelCaseNamingConvention.Instance)
        .Build();

      string generalYaml = serializer.Serialize(generalInfo);
      
      Console.WriteLine($"Saving to {Path.Combine(dir, "info.yaml")}");
      File.WriteAllText(Path.Combine(dir, "info.yaml"), generalYaml);

      foreach (var dream in dreams)
      {
        string file_path = Path.Combine(dir, $"Dream - {dream.Key}.yaml");
        Console.WriteLine($"Saving to {file_path}");
        File.WriteAllText(file_path,serializer.Serialize(dream.Value));
      }


    }

  }
}
