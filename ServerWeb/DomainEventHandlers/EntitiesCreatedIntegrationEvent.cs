#pragma warning disable 1591
namespace ServerWeb.DomainEventHandlers {
  public class EntitiesCreatedIntegrationEvent {
    public const string Topic = "cap-created-entities";

    public EntitiesCreatedIntegrationEvent(string entityTypeName, bool isArray, string data) {
      EntityTypeName = entityTypeName;
      IsArray = isArray;
      Data = data;
    }

    public string EntityTypeName { get; set; }
    public bool IsArray { get; }
    public string Data { get; }
  }
}