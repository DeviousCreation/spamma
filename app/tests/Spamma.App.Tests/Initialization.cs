using System.Runtime.CompilerServices;
using Argon;
using DiffEngine;
using Spamma.App.Infrastructure.Domain.DomainAggregate.Aggregate;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2, PublicKey=0024000004800000940000000602000000240000525341310004000001000100c547cac37abd99c8db225ef2f6c8a3602f3b3606cc9891605d02baa56104f4cfc0734aa39b93bf7852f7d9266654753cc297e7d2edfe0bac1cdcf9f717241550e0a7b191195b7667bb4f64bcb8e2121380fd1d9d46ad2d92d2d15605093924cceaf74c4861eff62abf69b9291ed0a340e113be11e6a7d3113e92484cf7045cc7")]

namespace Spamma.App.Tests;

public static class Initialization
{
    [ModuleInitializer]
    public static void Run()
    {
        DiffRunner.Disabled = true;
        VerifierSettings.AddExtraSettings(_ => _.Converters.Add(new DomainAppConverter()));
    }
}

internal class DomainAppConverter : WriteOnlyJsonConverter<Domain>
{
    public override void Write(VerifyJsonWriter writer, Domain value)
    {
        writer.WriteStartObject();

        writer.WriteMember(value, value.Id, nameof(value.Id));
        writer.WriteMember(value, value.WhenCreated, nameof(value.WhenCreated));
        writer.WriteMember(value, value.Name, nameof(value.Name));
        writer.WriteMember(value, value.CreatedUserId, nameof(value.CreatedUserId));
        writer.WriteMember(value, value.DomainAccessPolicies, nameof(value.DomainAccessPolicies));

        writer.WriteEndObject();
    }
}

