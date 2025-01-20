using Microsoft.EntityFrameworkCore;
using FirstApi.Models;
using FirstApi.Interfaces;
using FirstApi.Repository;
using FirstApi.Services;
// dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson
// using Microsoft.AspNetCore.Mvc.NewtonsoftJson;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
    // .ConfigureApiBehaviorOptions(options =>
    // {
    //     // Set JSON as the default response format
    //     options.SuppressInferBindingSourcesForParameters = true;
    // })
    // .AddNewtonsoftJson(options =>
    // {
    //     options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
    // });// Optional: Use Newtonsoft.Json if needed;


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MyDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<IUserService,UsersServices>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
