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
            throw new Exception($"Invalid question format: {question.Id}");
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
      // TODO: remove all the hard coding in this method

      bool has_answers = question.Answers != null;
      bool has_min = question.Min != null;
      bool has_max = question.Max != null;
      bool has_ask_for_count = question.AskForCount != null;
      bool has_sub_questions = question.SubQuestions != null;

      HashSet<string> valid_types = new HashSet<string> {"time", "yes_no", "int", "float", "string", "enum"};

      // general validation
      if (
          !(valid_types.Contains(question.Type)) // question type must be known
          || (has_answers && (question.Type != "enum" || question.Answers.Length < 2)) // questions with answers must be of type enum. and answers must have at least 2 options.
          || (question.Type == "enum" && !has_answers) // enum questions must have answers.
          || ((has_min || has_max) && !(question.Type == "int" || question.Type == "float")) // questions with min or max must be of type int or float.
          || (has_ask_for_count && (question.Type != "int" || question.AskForCount.Length == 0)) // questions with ask_for_count must be of type int and have at least one subquestion
          || (has_sub_questions && (question.Type != "yes_no")) // questions with subquestions must be of type yes_no
      )
      {
        return false; // question is invalid
      }

      // sub questions
      if (has_sub_questions)
      {
        if (question.SubQuestions.Yes != null)
        {
          foreach (Question subQuestion in question.SubQuestions.Yes)
          {
            if (!ValidateQuestion(subQuestion))
            {
              return false; // subQuestion is invalid
            }
          }
        }

        if (question.SubQuestions.No != null)
        {
          foreach (Question subQuestion in question.SubQuestions.No)
          {
            if (!ValidateQuestion(subQuestion))
            {
              return false; // subQuestion is invalid
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
            return false; // subQuestion is invalid
          }
        }
      }

      // special ids
      if (
          (question.Id == "dreams" && (question.Type != "int" || question.AskForCount == null)) // 'dreams' must be an int and must have AskForCount
          || (question.Id == "dream_title" && question.Type != "string") // 'dream_title' must be a string
      )
      {
        return false; // question is invalid
      }

      // question is valid
      return true;
    }
  }
}
