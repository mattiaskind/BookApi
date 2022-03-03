using BookApi;
using BookApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Registrerar databasklassen som en dependency, baserat på interfacet databasklassen implementerar
// Med singleton kommer en och samma instans att vara tillgänglig genom applikationens hela cykel
builder.Services.AddSingleton<IBooksDb, BooksDb>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// För att hindra ett fel som uppstår när bok skapas då dotnet-ramverket tar bort ändelsen "async" i metodnamnen vid körning
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
