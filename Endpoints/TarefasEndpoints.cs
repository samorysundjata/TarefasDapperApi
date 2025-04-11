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
        }
    }
}
