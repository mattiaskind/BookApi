using BookApi;
using BookApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Registrerar databasklassen som en dependency, baserat p� interfacet databasklassen implementerar
// Med singleton kommer en och samma instans att vara tillg�nglig genom applikationens hela cykel
builder.Services.AddSingleton<IBooksDb, BooksDb>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// F�r att hindra ett fel som uppst�r n�r bok skapas d� dotnet-ramverket tar bort �ndelsen "async" i metodnamnen vid k�rning
builder.Services.AddMvc(options => options.SuppressAsyncSuffixInActionNames = false);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
} else
{
    app.UseExceptionHandler("/error");
}

app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();
