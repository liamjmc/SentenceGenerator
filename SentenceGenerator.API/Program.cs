using SentenceGenerator.API;
using SentenceGenerator.DataAccess.Repository;
using SentenceGenerator.Domain;
using SentenceGenerator.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IGptRequester, GptRequester>();
builder.Services.AddTransient<IGptRequestBuilder, GptRequestBuilder>();
builder.Services.AddTransient<ISentenceGenerator, SentenceGenerator.Domain.Services.SentenceGenerator>();
builder.Services.AddTransient<ISentenceInserter, SentenceInserter>();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddScoped<ISentenceRepository, SentenceRepository>();

builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.DocumentTitle = "Sentence Generator API";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.MapControllers();

app.Run();
