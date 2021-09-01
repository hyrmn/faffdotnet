var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
}

app.UseStatusCodePagesWithReExecute("/Error", "?code={0}");

app.UseStaticFiles();
app.UseAuthorization();
app.MapRazorPages();

app.Run();
