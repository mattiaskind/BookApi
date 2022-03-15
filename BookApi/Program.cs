using BookApi;
using BookApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Registrerar databasklassen som en dependency, baserat på interfacet databasklassen implementerar
// Singleton så att listan i BooksDb är tillgänglig under tiden servern körs
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
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();     

} else
{
    // Använd global felhantering
    app.UseExceptionHandler("/error");
}

/* Cors-policyn tillåter för tillfället anrop från alla ursprung för
 * att göra det enklare att testa api:t fullt ut. Skulle behöva ses
 * över vid publicering
*/
app.UseCors(corsPolicy);

/* Vid körning av projektet BookApi (client) används
 * de statiska filerna i wwwroot som ger tillgång till ett
 * gränsnitt som interagerar med api:t
 */
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

// Ingen auth i det här projektet
//app.UseAuthorization();

app.MapControllers();

app.Run();
