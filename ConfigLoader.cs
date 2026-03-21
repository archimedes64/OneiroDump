using System;
using System.IO;
using System.Collections.Generic;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace OneiroDump
{
  public class ConfigLoader
  {
      public Config LoadConfig(string filePath)
      {
        try
        {
          string file = File.ReadAllText(filePath);
          var deserializer = new DeserializerBuilder().WithNamingConvention(UnderscoredNamingConvention.Instance).Build();  
          Config config = deserializer.Deserialize<Config>(file);
          return config;
        }
        catch (Exception ex)
        {
          Console.WriteLine($"Error loading config from {filePath}: {ex.Message}");
          throw;
        }
      }
      public Question[] LoadQuestions(string filePath)
      {
        try
        {

        string file = File.ReadAllText(filePath);
        var deserializer = new DeserializerBuilder().WithNamingConvention(UnderscoredNamingConvention.Instance).Build();  // https://github.com/aaubry/YamlDotNet
        Question[] questions = deserializer.Deserialize<Question[]>(file);
        foreach (Question question in questions)
        {
          if (!ValidateQuestion(question))
          {
            throw new Exception($"Invalid question format: {question.Id}"); // theres not a way to get more helpful error messages without making this file massive and complicating the logic in ValidateQuestion
          }
        }
        return questions;

        }
        catch (Exception ex)
        {
          Console.WriteLine($"Error loading questions from {filePath}: {ex.Message}");
          throw;
        }
      }
      public bool ValidateQuestion(Question question)
      {
        bool has_answers = question.Answers != null;
        bool has_min = question.Min != null;
        bool has_max = question.Max != null;
        bool has_ask_for_count = question.AskForCount != null;
        bool has_sub_questions = question.SubQuestions != null;
        HashSet<string> valid_types = new HashSet<string> {"time", "yes_no", "int", "float", "string", "enum"};
        #pragma warning disable CS8602
        if (
            !(valid_types.Contains(question.Type))
            || (has_answers && (question.Type != "enum" || question.Answers.Length < 2))
            || ((has_min || has_max) && !(question.Type == "int" || question.Type == "float"))
            || (has_ask_for_count && (question.Type != "int" || question.AskForCount.Length == 0))
            || (has_sub_questions && (question.Type != "yes_no"))
        )
        {
          return false;
        }
        if (question.Type == "yes_no" && has_sub_questions)
        {
          if (question.SubQuestions.Yes != null) 
          {
            foreach (Question subQuestion in question.SubQuestions.Yes)
            {
              if (!ValidateQuestion(subQuestion))
              {
                return false;
              }
            }
          }

          if (question.SubQuestions.No != null)
          {
            foreach (Question subQuestion in question.SubQuestions.No)
            {
              if (!ValidateQuestion(subQuestion))
              {
                return false;
              }
            }
          }

        }
        if (has_ask_for_count)
        {
          foreach (Question subQuestion in question.AskForCount)
          {
            if (!ValidateQuestion(subQuestion))
            {
              return false;
            }
          }
        }
        return true;
      }
  }
} 
