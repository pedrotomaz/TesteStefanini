using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Entities;
using Questao5.Domain.Interfaces;
using Questao5.Infrastructure.Database.CommandStore.Requests;
using Questao5.Infrastructure.Database.QueryStore.Requests;
using Questao5.Infrastructure.Repositories;
using Questao5.Infrastructure.Sqlite;
using Swashbuckle.AspNetCore.Annotations;
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
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
});

builder.Services.AddScoped<ICreateMovimentoCommandStore, CreateMovimentoCommandStore>();
builder.Services.AddScoped<IConsultaMovimentoQueryStore, ConsultaMovimentoQueryStore>();
builder.Services.AddScoped<IConsultaContaCorrenteQueryStore, ConsultaContaCorrenteQueryStore>();
builder.Services.AddScoped<IIdempotencyRepository, IdempotencyRepository>();

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


app.MapPost("/moviment", async (IMediator mediator, CreateMovimentoCommand command) =>
{
    try
    {
        var response = await mediator.Send(command);
        return Results.Ok(response);
    }
    catch (BusinessValidationException ex)
    {
        return Results.BadRequest(new ErrorResponse(ex.ErrorCode, ex.Message));
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new ErrorResponse("UNKNOWN_ERROR", ex.Message));
    }
}).WithName("CreateMoviment")
    .WithMetadata(new SwaggerOperationAttribute
    {
        Summary = "Endpoint para criar a movimentação",
        Description = "Este endpoint cria uma nova movimentação para a conta corrente.",
        OperationId = "CreateMoviment",
    })
    .Produces<CreateMovimentoResponse>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status400BadRequest);

app.MapGet("/balance/{contaCorrenteId}", async (string contaCorrenteId, IMediator mediator) =>
{
    try
    {
        var response = await mediator.Send(new MovimentoQueryRequest(contaCorrenteId));
        return Results.Ok(response);
    }
    catch (BusinessValidationException ex)
    {
        return Results.BadRequest(new ErrorResponse(ex.ErrorCode, ex.Message));
    }
    catch (Exception ex)
    {
        return Results.BadRequest(new ErrorResponse("UNKNOWN_ERROR", ex.Message));
    }
}).WithName("CheckBalance")
    .WithMetadata(new SwaggerOperationAttribute
    {
        Summary = "Endpoint para consultar o saldo da conta",
        Description = "Este endpoint retorna as informaçòes da conta corrente, o saldo e a data da consulta.",
        OperationId = "CheckBalance",
    })
    .Produces<MovimentoQueryResponse>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status400BadRequest);


app.Run();

// Informações úteis:
// Tipos do Sqlite - https://www.sqlite.org/datatype3.html


