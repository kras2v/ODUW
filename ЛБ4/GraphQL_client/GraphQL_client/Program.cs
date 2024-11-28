using System;
using System.Text.Json;
using System.Threading.Tasks;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;

var client = new GraphQLHttpClient("http://localhost:5102/graphql", new SystemTextJsonSerializer());

try
{
    var query = new GraphQLHttpRequest
    {
        Query = @"
        query {
            books {
                id
                title
                description
            }
        }"
    };

    var response = await client.SendQueryAsync<JsonElement>(query);

    if (!response.Data.TryGetProperty("books", out var books) || books.ValueKind != JsonValueKind.Array)
    {
        Console.WriteLine("No books found in the response.");
        return;
    }

    Console.WriteLine("Books list:");
    foreach (var book in books.EnumerateArray())
    {
        var id = book.GetProperty("id").GetInt32();
        var title = book.GetProperty("title").GetString();
        var description = book.GetProperty("description").GetString();

        Console.WriteLine($"- {id}: {title} - {description}");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

