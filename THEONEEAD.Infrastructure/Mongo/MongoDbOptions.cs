namespace THEONEEAD.Infrastructure.Mongo;

public class MongoDbOptions
{
    public const string SectionName = "MongoDB";
    public string ConnectionString { get; set; } = "";
    public string DatabaseName { get; set; } = "theoneead";
}
