using LiteDB;

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddSingleton<TodoRepository>()
    .AddRazorPages();

builder.Logging.AddConsole().SetMinimumLevel(LogLevel.Warning);

var app = builder.Build();
Console.WriteLine("Starting");
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    Console.WriteLine("This is sparta");
}
else
{
    app.UseExceptionHandler("/Error");
}

app.UseStatusCodePagesWithReExecute("/Error", "?code={0}");
app.UseStaticFiles();

app.MapGet("/favicon.ico", () => Results.NotFound());
app.MapGet("/api/todo", () => new Todo(ObjectId.NewObjectId(), "Test", false));

app.MapRazorPages();

app.Run();

record Todo(ObjectId? Id, string Title, bool Completed = false, int? Order = null);

record Summary(int Total, int Start, Todo[] List);

class TodoRepository
{
    private readonly string dbPath;
    private readonly ILogger<TodoRepository> logger;

    public TodoRepository(IWebHostEnvironment env, ILogger<TodoRepository> logger)
    {
        this.dbPath = Path.Combine(env.ContentRootPath, "data/todos.db");
        this.logger = logger;
    }

    public Summary Paged(int start, int pageSize)
    {
        using var db = new LiteDatabase(dbPath);
        var col = db.GetCollection<Todo>(nameof(Todo));
        col.EnsureIndex(s => s.Order);

        return new(
            Total: col.Count(),
            Start: start,
            List: col.Find(Query.All(Query.Descending), skip: start, limit: pageSize).ToArray()
        );
    }

    public Todo Get(ObjectId id)
    {
        using var db = new LiteDatabase(dbPath);
        var col = db.GetCollection<Todo>(nameof(Todo));
        return col.FindById(id);
    }

    public void Save(Todo todo)
    {
        using var db = new LiteDatabase(dbPath);
        var col = db.GetCollection<Todo>(nameof(Todo));
        col.EnsureIndex(s => s.Order);
        col.Upsert(todo);
    }
}
