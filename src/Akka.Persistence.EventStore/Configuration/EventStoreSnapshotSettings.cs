using Akka.Configuration;

namespace Akka.Persistence.EventStore.Configuration;

/// <summary>
/// Settings for the EventStore snapshot-store implementation, parsed from HOCON configuration.
/// </summary>
public class EventStoreSnapshotSettings : ISettingsWithAdapter
{
    public EventStoreSnapshotSettings(Config config)
    {
        if (config == null)
            throw new ArgumentNullException(nameof(config),
                "EventStore snapshot-store settings cannot be initialized, because required HOCON section couldn't been found");

        config = config.WithFallback(EventStorePersistence.DefaultSnapshotConfiguration);
        
        ConnectionString = config.GetString("connection-string");
        Adapter = config.GetString("adapter", "default");
        StreamPrefix = config.GetString("prefix", "snapshot@");
        DefaultSerializer = config.GetString("serializer");
        MaterializerDispatcher = config.GetString("materializer-dispatcher", "akka.actor.default-dispatcher");
        Tenant = config.GetString("tenant");
    }

    public string ConnectionString { get; }
    public string Adapter { get; }
    public string DefaultSerializer { get; }
    public string StreamPrefix { get; }
    public string Tenant { get; }
    public string MaterializerDispatcher { get; }
    
    public string GetStreamName(string persistenceId, EventStoreTenantSettings tenantSettings)
    {
        return tenantSettings.GetStreamName($"{StreamPrefix}{persistenceId}", Tenant);
    }
}