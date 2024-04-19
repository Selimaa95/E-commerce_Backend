using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/*********************************************************************************/
//Add connection string.
var ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlite(ConnectionString));
//Add DI Services
//builder.Services.AddScoped<IProduct, ProductRepo>();  
builder.Services.AddScoped( typeof(IGenericRepository<>), typeof(GenericRepository<>) );
//Auto Mapper Congfigurations.
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


/*********************************************************************************/

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//Added Static Files.
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

#region Auto Migration and Data Seeding

//Update Migration when Run my Project.
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var context = services.GetRequiredService<ApplicationDbContext>();
var logger = services.GetRequiredService<ILogger<Program>>();
try
{
    //Auto Migration
    await context.Database.MigrateAsync();
    //Seeding Data
    await StoreContextSeed.SeedAsync(context);

}
catch (Exception ex)
{

    logger.LogError(ex, "Error occured while migrating process");
}
#endregion

app.Run();
