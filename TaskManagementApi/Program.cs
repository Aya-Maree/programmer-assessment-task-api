using Microsoft.EntityFrameworkCore;
using TaskManagementApi.Data;

var builder = WebApplication.CreateBuilder(args);

// add controllers for the api
builder.Services.AddControllers();

// connect to the sqlite database
builder.Services.AddDbContext<TaskDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// make sure the database is set up and has sample data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TaskDbContext>();

    context.Database.Migrate();
    DataSeeder.Seed(context);
}

// map the api controllers
app.MapControllers();

app.Run();