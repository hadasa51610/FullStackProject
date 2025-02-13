using TodoApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddDbContext<ToDoDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("ToDoDB");
    options.UseMySql(connectionString, ServerVersion.Parse("8.0.41-mysql"));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("AllowAllOrigins");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(option =>
    {
        option.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        option.RoutePrefix = string.Empty;
    });
}

app.MapGet("/tasks", async (ToDoDbContext dto) =>
{
    var items = await dto.Items.ToListAsync();
    return Results.Ok(items);
});

app.MapPost("/tasks", async ([FromBody] Item item, ToDoDbContext Dto) =>
{
    Dto.Items.Add(item);
    await Dto.SaveChangesAsync();
    return Results.Created($"/tasks/{item.Id}", item);
});

app.MapPut("/tasks/{id}", async (int id, [FromBody] Item item, ToDoDbContext todos) =>
{
    var todo = await todos.Items.FindAsync(id);
    if (todo is null) return Results.NotFound();
    todo.Iscomplete = item.Iscomplete;
    todo.Name = item.Name != "" ? item.Name : todo.Name;
    await todos.SaveChangesAsync();
    return Results.NoContent();
});
app.MapDelete("/tasks/{id}", async (int Id, ToDoDbContext Db) =>
{
    if (await Db.Items.FindAsync(Id) is Item item)
    {
        Db.Items.Remove(item);
        await Db.SaveChangesAsync();
        return Results.Ok(item);
    }
    return Results.NotFound();
});

app.MapGet("/", () => "host server is running");

app.Run();