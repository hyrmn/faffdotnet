using static Scriban.Template;

using LiteDB;
using Scriban;

using Microsoft.AspNetCore.Antiforgery;

var builder = WebApplication.CreateBuilder();
builder.Services
    .AddAntiforgery()
    .AddMemoryCache();

builder.Logging.AddConsole().SetMinimumLevel(LogLevel.Warning);

var app = builder.Build();
app.UseStaticFiles();

app.MapGet("/", async () => Results.Text(await RenderHome(), "text/html"));
app.MapGet("/favicon.ico", () => Results.NotFound());

app.Run();

static async Task<string> RenderHome()
{
    var contentTemplate = Parse(@"
    <h3>I am home</h3>
    ");

    return await Layout().RenderAsync(new { Title = "Home", Content = await contentTemplate.RenderAsync()});
}

static Template Layout()
{
    return Parse(@"
    <!DOCTYPE html>
    <meta charset=""utf-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
    <link href=""data:image/x-icon;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAA00lEQVQ4jZWSQRGEMAxFX6kBJFQCEpDAfS+c91QHSKgEJCChEioBCUhgD7RLtgsFMtML89/PTwI8qxpoAQ8EwD6B2wit4s134Q5ogCUzmO5EdlHsgUapH5P+Cp6yjl4kmaPmB2jRus/gEDt5YFUqmmg6CY5iPidir8AQdf1ZdCkOoF/8b9qyX8AdzWvibAal8lPJF/K5j8qewJ76Gh6iQW7i7nRuBJBMFuBNVVn2ZRa7y46w7SadszvGtqqBGfX9UeT3ZGpKBuZEkAx8MftFjRQW+AGoU1dleoYetgAAAABJRU5ErkJggg=="" rel=""icon"" type=""image/x-icon"" />
    <title>{{ title }}</title>
    <body>
        <div>
            {{~ content ~}}
        </div>
    </body>
    ");
}