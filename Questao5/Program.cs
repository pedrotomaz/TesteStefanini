using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Queries.Requests;
using Questao5.Domain.Interfaces;
using Questao5.Infrastructure.Database.CommandStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Sqlite;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

// sqlite
builder.Services.AddSingleton(new DatabaseConfig { Name = builder.Configuration.GetValue<string>("DatabaseName", "Data Source=database.sqlite") });
builder.Services.AddSingleton<IDatabaseBootstrap, DatabaseBootstrap>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICreateMovimentoCommandStore, CreateMovimentoCommandStore>();
builder.Services.AddScoped<IConsultaMovimentoQueryStore, ConsultaMovimentoQueryStore>();
builder.Services.AddScoped<IConsultaContaCorrenteQueryStore, ConsultaContaCorrenteQueryStore>();

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

// sqlite
#pragma warning disable CS8602 // Dereference of a possibly null reference.
app.Services.GetService<IDatabaseBootstrap>().Setup();
#pragma warning restore CS8602 // Dereference of a possibly null reference.


// Endpoint para criar a movimentação
app.MapPost("/movimento", async (IMediator mediator, CreateMovimentoCommand command) =>
{
    try
    {
        var response = await mediator.Send(command);

        return Results.Ok(response);
    }
    catch (Exception err)
    {
        return Results.BadRequest(err.Message);
    }
});

// Endpoint para consultar saldo
app.MapGet("/movimento/{contaCorrenteId}", async (string contaCorrenteId, IMediator mediator) =>
{
    try
    {
        var response = await mediator.Send(new MovimentoQueryRequest(contaCorrenteId));

        return Results.Ok(response);
    }
    catch (Exception err)
    {
        return Results.BadRequest(err.Message);
    }
});


app.Run();

// Informações úteis:
// Tipos do Sqlite - https://www.sqlite.org/datatype3.html


