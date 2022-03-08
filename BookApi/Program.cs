using BookApi;
using BookApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Registrerar databasklassen som en dependency, baserat på interfacet databasklassen implementerar
builder.Services.AddSingleton<IBooksDb, BooksDb>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// För att hindra ett fel som uppstår när bok skapas då dotnet-ramverket tar bort ändelsen "async" i metodnamnen vid körning
builder.Services.AddMvc(options => options.SuppressAsyncSuffixInActionNames = false);

var corsPolicy = "corsPolicy";
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicy, builder =>
    {
        //builder.WithOrigins("https://localhost:7046/books").AllowAnyHeader().AllowAnyMethod();
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(corsPolicy);    

} else
{
    // Använd global felhantering
    app.UseExceptionHandler("/error");
}

// För statiska filer
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();
