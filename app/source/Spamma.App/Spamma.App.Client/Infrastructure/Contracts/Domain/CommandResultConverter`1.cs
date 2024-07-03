﻿using System.Text.Json;
using System.Text.Json.Serialization;
using Spamma.App.Client.Infrastructure.Constants;

namespace Spamma.App.Client.Infrastructure.Contracts.Domain;

public class CommandResultConverter<T> : JsonConverter<CommandResult<T>>
{
    public override CommandResult<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }

        CommandResultStatus status = CommandResultStatus.Succeeded;
        ErrorData? errorData = null;
        CommandValidationResult? validationResult = null;
        T? data = default;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                break;
            }

            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException();
            }

            var propertyName = reader.GetString();
            reader.Read();

            switch (propertyName)
            {
                case "Status":
                    status = (CommandResultStatus)reader.GetInt32();
                    break;
                case "ErrorData":
                    errorData = JsonSerializer.Deserialize<ErrorData>(ref reader, options);
                    break;
                case "ValidationResult":
                    validationResult = JsonSerializer.Deserialize<CommandValidationResult>(ref reader, options);
                    break;
                case "Data":
                    data = JsonSerializer.Deserialize<T>(ref reader, options);
                    break;
                default:
                    throw new JsonException();
            }
        }

        switch (status)
        {
            case CommandResultStatus.Succeeded:
                if (data == null)
                {
                    throw new JsonException();
                }

                return CommandResult<T>.Succeeded(data);
            case CommandResultStatus.Failed:
                if (errorData == null)
                {
                    throw new JsonException();
                }

                return CommandResult<T>.Failed(errorData);
            case CommandResultStatus.Invalid:
                if (validationResult == null)
                {
                    throw new JsonException();
                }

                return CommandResult<T>.Invalid(validationResult);
            default:
                throw new JsonException();
        }
    }

    public override void Write(Utf8JsonWriter writer, CommandResult<T> value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("Status", (int)value.Status);

        if (value.Status == CommandResultStatus.Failed)
        {
            writer.WritePropertyName("ErrorData");
            JsonSerializer.Serialize(writer, value.ErrorData, options);
        }
        else if (value.Status == CommandResultStatus.Invalid)
        {
            writer.WritePropertyName("ValidationResult");
            JsonSerializer.Serialize(writer, value.ValidationResult, options);
        }
        else if (value.Status == CommandResultStatus.Succeeded)
        {
            writer.WritePropertyName("Data");
            JsonSerializer.Serialize(writer, value.Data, options);
        }

        writer.WriteEndObject();
    }
}