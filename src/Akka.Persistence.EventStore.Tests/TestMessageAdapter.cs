using System.Collections.Immutable;
using Akka.Persistence.EventStore.Configuration;
using Akka.Persistence.EventStore.Serialization;
using EventStore.Client;

namespace Akka.Persistence.EventStore.Tests;

public class TestMessageAdapter(Akka.Serialization.Serialization serialization, ISettingsWithAdapter settings) 
    : DefaultMessageAdapter(serialization, settings)
{
    private readonly string _tenant = settings.Tenant;
    
    protected override IStoredEventMetadata GetEventMetadata(
        IPersistentRepresentation message,
        IImmutableSet<string> tags)
    {
        return new TestEventMetadata(message, tags, _tenant);
    }

    protected override async Task<IStoredEventMetadata?> GetEventMetadataFrom(ResolvedEvent evnt)
    {
        var metadata = await DeSerialize(evnt.Event.Metadata.ToArray(), typeof(TestEventMetadata));
        
        return metadata as IStoredEventMetadata;
    }

    public class TestEventMetadata : StoredEventMetadata
    {
        public TestEventMetadata()
        {
            
        }

        public TestEventMetadata(
            IPersistentRepresentation message,
            IImmutableSet<string> tags,
            string tenant) : base(message, tags, tenant)
        {
            ExtraProp = "test";
        }

        public string? ExtraProp { get; set; }
    }
}