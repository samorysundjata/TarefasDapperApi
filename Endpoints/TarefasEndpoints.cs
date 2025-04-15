using Dapper.Contrib.Extensions;
using TarefasDapperApi.Data;
using static TarefasDapperApi.Data.TarefaContext;

namespace TarefasDapperApi.Endpoints
{
    public static class TarefasEndpoints
    {
        public static void MapTarefasEndpoints(this WebApplication app)
        {
            app.MapGet("/", () => $"Bem-Vindo a API Tarefas! {DateTime.Now}");

            app.MapGet("/tarefas", async (GetConnection connectionGetter) =>
            {
                using var con = await connectionGetter();
                var tarefas = con.GetAll<Tarefa>().ToList();
                if (tarefas == null || tarefas.Count == 0)
                {
                    return Results.NotFound("Nenhuma tarefa encontrada.");
                }
                return Results.Ok(tarefas);
            });

            app.MapGet("/tarefas/{id}", async (GetConnection connectionGetter, int id) =>
            {
                using var con = await connectionGetter();
                
                return con.Get<Tarefa>(id) is Tarefa tarefa
                    ? Results.Ok(tarefa)
                    : Results.NotFound($"Tarefa com ID {id} não encontrada.");
            });

            app.MapPost("/tarefas", async (GetConnection connectionGetter, Tarefa tarefa) =>
            {
                using var con = await connectionGetter();
                var id = (int)con.Insert(tarefa);
                return Results.Created($"/tarefas/{id}", tarefa);
            });

            app.MapPut("/tarefas/{id}", async (GetConnection connectionGetter, Tarefa tarefa) =>
            {
                using var con = await connectionGetter();
                var id = con.Update(tarefa);
                return Results.Ok();
            });
        }
    }
}
