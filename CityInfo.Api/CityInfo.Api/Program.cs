using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.StaticFiles;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders(); // clear console logger.
builder.Logging.AddConsole(); // Add logs to console window.
// Add services to the container.

builder.Services.AddControllers(options=>
{ 
    options.ReturnHttpNotAcceptable=true;
}).AddNewtonsoftJson()
  .AddXmlDataContractSerializerFormatters(); // If you want output in xml.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<FileExtensionContentTypeProvider>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// dev notes: Add below code after you get an error at app.MapContollers();
// To enable end point routing
app.UseRouting();

app.UseAuthorization();
app.UseEndpoints(endpoints =>
{ 
    endpoints.MapControllers();
});

//app.MapControllers();

app.Run();
