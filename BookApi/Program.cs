using BookApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Registrerar databasklassen som en dependency, baserat på interfacet databasklassen implementerar
// Med singleton kommer en och samma instans att vara tillgänglig genom applikationens hela cykel
builder.Services.AddSingleton<IBooksDb, BooksDb>();
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

app.MapControllers();

app.Run();
