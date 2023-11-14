// See https://aka.ms/new-console-template for more information
using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.AccessControl;
using System.Text.Json;

public static class Program
{
    static void Main(string[] args)
    {
        var actionWrapper = new ActionWrapper
        {
            Action = new ComplexAction
            {
                Name = "ComplexAction",
                FirstName = "John",
                LastName = "Doe",
                CityOfBirth = "New York",
                SecurityQuestions = "What is your favorite color?"
            },
            Runs = new List<Run>
            {
                new Run
                {
                    Name = "Run1",
                    LastRunTime = DateTime.Now
                },
                new Run
                {
                    Name = "Run2",
                    LastRunTime = DateTime.Now
                }
            }
        };

        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new YourMappingProfile());
        });

        IMapper mapper = mappingConfig.CreateMapper();


        var dto = mapper.Map<ActionWrapperDTO>(actionWrapper);

        var serializer = new Newtonsoft.Json.JsonSerializer();
        serializer.Formatting = Newtonsoft.Json.Formatting.Indented;
        serializer.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All;

        MemoryStream stream = new MemoryStream();

        using (StreamWriter writer = new StreamWriter(stream))
        {
            serializer.Serialize(writer, dto);

            writer.Flush();

            ActionWrapperDTO dto2 = null;
            stream.Position = 0;
            using (StreamReader reader = new StreamReader(stream))
            using (var jReader = new JsonTextReader(reader))
            {
                dto2 = serializer.Deserialize<ActionWrapperDTO>(jReader);
            }

            Console.WriteLine((dto2.Action as ComplexActionDTO).CityOfBirth);

            var domain2 = mapper.Map<ActionWrapper>(dto2);

            Console.WriteLine((domain2.Action as ComplexAction).CityOfBirth);

            Console.ReadLine();

        }



    }
}

// Domain Classes
// ActionWrapper
// - Build
// - Action
// - Runs (multiple)
// - Revision
// - ActionCache


// DTO Classes
// ActionWrapperDTO
// - Collection<RunDTO>
// - Action


public class YourMappingProfile : Profile
{
    public YourMappingProfile()
    {
        CreateMap<ActionWrapper, ActionWrapperDTO>();
        CreateMap<Run, RunDTO>();
        CreateMap<ComplexAction, ComplexActionDTO>();
        CreateMap<SimpleAction, SimpleActionDTO>();
        CreateMap<ComplexAction, IActionDTO>().As<ComplexActionDTO>();
        CreateMap<SimpleAction, IActionDTO>().As<SimpleActionDTO>();

        CreateMap<ActionWrapperDTO, ActionWrapper>();
        CreateMap<RunDTO, Run>();
        CreateMap<ComplexActionDTO, ComplexAction>();
        CreateMap<SimpleActionDTO, SimpleAction>();
        CreateMap<ComplexActionDTO, IAction>().As<ComplexAction>();
        CreateMap<SimpleActionDTO, IAction>().As<SimpleAction>();
    }
}

public class ActionWrapper
{
    public IAction Action { get; set; }
    public List<Run> Runs { get; set; }
}

public interface IAction
{
    string Name { get; set; }
}

public class ComplexAction : IAction
{
    public string Name { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string CityOfBirth { get; set; }
    public string SecurityQuestions { get; set; }
}
public class SimpleAction : IAction
{
    public string Name { get; set; }
    public DateTime SignalTime { get; set; }
}

public class Run
{
    public string Name { get; set; }
    public DateTime LastRunTime { get; set; }
}

public class ActionWrapperDTO
{
    public List<RunDTO> Runs { get; set; }
    public IActionDTO Action { get; set; }
}

public interface IActionDTO
{
    string Name { get; set; }
}

public class ComplexActionDTO : IActionDTO
{
    public string Name { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string CityOfBirth { get; set; }
    public string SecurityQuestions { get; set; }
}

public class SimpleActionDTO : IActionDTO
{
    public string Name { get; set; }
    public DateTime SignalTime { get; set; }
}
public class RunDTO
{
    public string Name { get; set; }
    public DateTime LastRunTime { get; set; }
}
