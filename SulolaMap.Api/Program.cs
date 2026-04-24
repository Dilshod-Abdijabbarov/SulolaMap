using Application.Interfaces;
using Application.Mapping;
using Application.Services;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.InfrastructureSetting(builder.Configuration);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IPersonService,PersonService>();
builder.Services.AddAutoMapper(cfg => { cfg.AddProfile<MappingProfile>(); });

builder.Services.AddCors(options =>
{
    options.AddPolicy(
       name: "AllowOrigin",
       builder =>
       {
           builder
          .AllowAnyMethod()
          .AllowAnyHeader()
          .AllowCredentials()
          .WithOrigins(
          "http://localhost:4200",
          "http://localhost:4300"
       );
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowOrigin");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MainConfigureMigration();

app.Run();
