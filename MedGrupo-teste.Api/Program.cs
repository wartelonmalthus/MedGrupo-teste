using MedGrupo_teste.Application.Services;
using MedGrupo_teste.Infraestructure;
using MedGrupo_teste.Infraestructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AdicionarInfraestrutura(builder.Configuration);
builder.Services.AddScoped<ServicoContato>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var contextoDb = scope.ServiceProvider.GetRequiredService<ContextoAplicacaoDb>();
    contextoDb.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
