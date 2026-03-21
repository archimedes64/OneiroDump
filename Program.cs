// See https://aka.ms/new-console-template for more information

namespace OneiroDump
{
  public class Program
  {
    public static void Main(string[] args)
    {

      ConfigLoader configLoader = new ConfigLoader();
      Config config = configLoader.LoadConfig("config/config.yaml");
      List<Question> questions = configLoader.LoadQuestions(config.GeneralQuestions);

    }
  }
}
