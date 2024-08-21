using BLL; // Asegúrate de tener la referencia correcta a tu proyecto BLL

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<Customers>(); // Registro de Customer
builder.Services.AddTransient<Products>();  // Registro de Product
builder.Services.AddTransient<Suppliers>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Mapping controllers
app.MapControllers();

app.UseRouting(); // Debe ser llamado antes de UseEndpoints

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
