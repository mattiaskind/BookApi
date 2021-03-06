using BookApi;
using BookApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Registrerar databasklassen som en dependency, baserat p? interfacet databasklassen implementerar
// Singleton - s? att en och samma instans av BooksDb ?r tillg?nglig under tiden servern k?rs
builder.Services.AddSingleton<IBooksDb, BooksDb>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// F?r att hindra ett fel som uppst?r n?r bok skapas d? dotnet-ramverket tar bort ?ndelsen "async" i metodnamnen vid k?rning
// Jag har f?rs?kt utg? fr?n async/await ?ven om mina objekt i det h?r fallet bara sparas i en List.
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
    // Anv?nd global felhantering
    app.UseExceptionHandler("/error");
}

/* Cors-policyn till?ter f?r tillf?llet anrop fr?n alla ursprung f?r
 * att g?ra det enklare att testa api:t fullt ut. 
*/
app.UseCors(corsPolicy);

/* Vid k?rning av projektet BookApi (starta med frontend) anv?nds
 * de statiska filerna i wwwroot, vilket bland annat best?r av javascript som jag skrivit
 * f?r att f? ett gr?nsnitt som kan interagera med api:t
 */
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

// Ingen auth i det h?r projektet
//app.UseAuthorization();

app.MapControllers();

app.Run();
