using BookApi;
using BookApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Registrerar databasklassen som en dependency, baserat p� interfacet databasklassen implementerar
builder.Services.AddSingleton<IBooksDb, BooksDb>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// F�r att hindra ett fel som uppst�r n�r bok skapas d� dotnet-ramverket tar bort �ndelsen "async" i metodnamnen vid k�rning
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
    // Anv�nd global felhantering
    app.UseExceptionHandler("/error");
}

/* Cors-policyn till�ter f�r tillf�llet anrop fr�n alla ursprung vilket
 * inte �r optimalt i produktionsmilj�. Jag anv�nder det bara nu f�r att
 * kunna testa projektet men det skulle beh�va konfigureras om projektet
 * anv�nds i andra sammanhang
*/
app.UseCors(corsPolicy);


/* Vid k�rning av projektet BookApi (client) anv�nds
 * de statiska filerna i wwwroot som ger tillg�ng till ett
 * gr�nsnitt som interagerar med api:t
 */
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();
