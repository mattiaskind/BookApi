using BookApi;
using BookApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Registrerar databasklassen som en dependency, baserat på interfacet databasklassen implementerar
// Singleton - så att en och samma instans av BooksDb är tillgänglig under tiden servern körs
builder.Services.AddSingleton<IBooksDb, BooksDb>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// För att hindra ett fel som uppstår när bok skapas då dotnet-ramverket tar bort ändelsen "async" i metodnamnen vid körning
// Jag har försökt utgå från async/await även om mina objekt i det här fallet bara sparas i en List.
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
 * att göra det enklare att testa api:t fullt ut. 
*/
app.UseCors(corsPolicy);

/* Vid körning av projektet BookApi (starta med frontend) används
 * de statiska filerna i wwwroot, vilket bland annat består av javascript som jag skrivit
 * för att få ett gränsnitt som kan interagera med api:t
 */
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

// Ingen auth i det här projektet
//app.UseAuthorization();

app.MapControllers();

app.Run();
