using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using PersonDetails.Api.Data;
using PersonDetails.Api.Data.Repos;
using PersonDetails.Api.Mappings;
using PersonDetails.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Entity Framework for SQL Server
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure MongoDB
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
{
    var settings = MongoClientSettings.FromConnectionString(builder.Configuration.GetConnectionString("MongoDb"));
    return new MongoClient(settings);
});

// Add Repositories
builder.Services.AddSingleton<IPersonRepository, CsvPersonRepository>(sp =>
    new CsvPersonRepository("persons.csv"));
builder.Services.AddTransient<IPersonRepository, SqlPersonRepository>();

//builder.Services.AddTransient<IPersonRepository, MongoPersonRepository>(sp =>
//{
//    var client = sp.GetRequiredService<IMongoClient>();
//    return new MongoPersonRepository(client, "PersonDb", "Persons");
//});

builder.Services.AddTransient<PersonService>();

builder.Services.AddAutoMapper(typeof(PersonMappingProfile));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseDefaultFiles();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Use(async (context, next) =>
{
   if (!context.Request.Path.HasValue || context.Request.Path == "/")
   {
       context.Response.Redirect("/index.html");
       return;
   }

   await next();
});

app.Run();