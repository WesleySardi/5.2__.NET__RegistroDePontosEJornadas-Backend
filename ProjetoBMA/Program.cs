using Microsoft.EntityFrameworkCore;
using ProjetoBMA.Data;
using ProjetoBMA.Mappings;
using ProjetoBMA.Middleware;
using ProjetoBMA.Repositories;
using ProjetoBMA.Repositories.Interfaces;
using ProjetoBMA.Services;
using ProjetoBMA.Services.Interfaces;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

ConfigureControllersAndSwagger(builder.Services);
ConfigureDbContext(builder.Services, builder.Configuration);
ConfigureRepositoriesAndServices(builder.Services);
ConfigureAutoMapper(builder.Services);
ConfigureCors(builder.Services);

var app = builder.Build();

ConfigurePipeline(app);

app.Run();

void ConfigureControllersAndSwagger(IServiceCollection services)
{
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(c =>
    {
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

        c.IncludeXmlComments(xmlPath);
    });
}

void ConfigureDbContext(IServiceCollection services, IConfiguration configuration)
{
    var conn = configuration.GetConnectionString("DefaultConnection");
    services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(conn));
}

void ConfigureRepositoriesAndServices(IServiceCollection services)
{
    services.AddScoped<ITimeEntryRepository, TimeEntryRepository>();
    services.AddScoped<ITimeEntryService, TimeEntryService>();
}

void ConfigureAutoMapper(IServiceCollection services)
{
    services.AddAutoMapper(typeof(MappingProfile).Assembly);
}

void ConfigureCors(IServiceCollection services)
{
    services.AddCors(options =>
    {
        options.AddPolicy("AllowFrontend", policy =>
        {
            policy.WithOrigins(
                    "http://localhost:5173",
                    "https://meu-teste.com.br"
                )
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
    });
}

void ConfigurePipeline(WebApplication app)
{
    app.UseCors("AllowFrontend");

    app.UseCustomExceptionMiddleware();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();

    app.MapControllers();

    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.Migrate();
        SeedData.EnsureSeedDataAsync(db).Wait();
    }
}