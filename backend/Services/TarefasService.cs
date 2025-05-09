using backend.Infraestrutura.Mappers;
using backend.Models.Dtos;
using backend.Models.Entities;
using backend.Repositorios.Interface;
using backend.Services.Interface;
using MySqlX.XDevAPI;
using System.Security.Claims;

namespace backend.Services
{
	public class TarefasService(ITarefasRepositorio tarefasRepositorio,
		IHttpContextAccessor httpContext) : ITarefasService
	{
		private Guid PegarIdUsuario()
		{
			return Guid.Parse(httpContext.HttpContext?.User.FindFirstValue(
				ClaimTypes.NameIdentifier)!);
		}
		public async Task<List<TarefasResponse>> BuscarTarefasPorUsuario()
		{
			Guid idUsuario = PegarIdUsuario();
			List<Tarefas> tarefas = await tarefasRepositorio
				.BuscarTarefasPorUsuario(idUsuario);
			List<TarefasResponse> resposta = new List<TarefasResponse>();
			foreach(Tarefas tarefa in tarefas)
			{
				resposta.Add(tarefa.ToTarefasResponse());
			}
			return resposta;
		}
		public async Task<TarefasResponse> CriarNovaTarefa(TarefasCreate dados)
		{
			Guid idUsuario = PegarIdUsuario();
			Tarefas novaTarefa = dados.ToTarefas(idUsuario);
			Tarefas resposta = await tarefasRepositorio.CriarTarefa(novaTarefa);
			return resposta.ToTarefasResponse();
		}
		public async Task<TarefasResponse> EditarTarefa(Guid idTarefa, TarefasUpdate dados)
		{
			Tarefas tarefaAtualizada = await tarefasRepositorio.AtualizarTarefa(idTarefa, dados);
			return tarefaAtualizada.ToTarefasResponse();
		}

		public async Task<TarefasResponse> AtualizarStatusTarefa(Guid idTarefa, bool status)
		{
			Tarefas tarefaAtualizada = await tarefasRepositorio.AtualizarStatusTarefa(idTarefa, status);
			return tarefaAtualizada.ToTarefasResponse();
		}

		public async Task ExcluirTarefa(Guid idTarefa)
		{
			await tarefasRepositorio.ExcluirTarefa(idTarefa);
			return;
		}
	}
}
