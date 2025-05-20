using backend.Infraestrutura.Mappers;
using backend.Models.Dtos;
using backend.Models.Entities;
using backend.Repositorios.Interface;
using backend.Resultados;
using backend.Services.Interface;
using MySqlX.XDevAPI;
using System.Security.Claims;
using System.Threading.Tasks;

namespace backend.Services
{
	public class TarefasService(ITarefasRepositorio tarefasRepositorio,
		IUsuarioRepositorio usuarioRepositorio, IHttpContextAccessor httpContext) 
		: ITarefasService
	{
		private async Task<Usuario?> BuscarUsuario()
		{
			var idUsuario = httpContext.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (idUsuario is null) return null;
			return await usuarioRepositorio.BuscarUsuarioPorId(Guid.Parse(idUsuario!));
		}
		public async Task<Result<List<TarefasResponse>, Error>> BuscarTarefasPorUsuario()
		{
			Usuario? usuario = await BuscarUsuario();
			if (usuario is null) return UsuarioFailure.TokenInvalido;
			List<Tarefas> tarefas = await tarefasRepositorio
				.BuscarTarefasPorUsuario(usuario.Id);
			List<TarefasResponse> tarefasDto = tarefas.Select(tarefa => tarefa.ToTarefasResponse()).ToList();
			return tarefasDto;
		}
		public async Task<Result<TarefasResponse, Error>> CriarNovaTarefa(TarefasCreate dados)
		{
			Usuario? usuario = await BuscarUsuario();
			if (usuario is null) return UsuarioFailure.TokenInvalido;
			Tarefas novaTarefa = dados.ToTarefas(usuario.Id);
			var resposta = await tarefasRepositorio.CriarTarefa(novaTarefa, usuario);
			if (resposta is null) return TarefasFailure.QuantidadeExcedida;
			return resposta.ToTarefasResponse();
		}
		public async Task<Result<TarefasResponse, Error>> EditarTarefa(Guid idTarefa, TarefasUpdate dados)
		{
			Tarefas? tarefa = await tarefasRepositorio.BuscarTarefaPorId(idTarefa);
			if (tarefa is null) return TarefasFailure.TarefaNaoEncontrada;
			Tarefas? tarefaAtualizada = await tarefasRepositorio.AtualizarTarefa(tarefa, dados);
			if (tarefaAtualizada is null) return TarefasFailure.TarefaNaoEncontrada;
			return tarefaAtualizada.ToTarefasResponse();
		}

		public async Task<Result<TarefasResponse, Error>> AtualizarStatusTarefa(Guid idTarefa, bool status)
		{
			Tarefas? tarefa = await tarefasRepositorio.BuscarTarefaPorId(idTarefa);
			if (tarefa is null) return TarefasFailure.TarefaNaoEncontrada;
			Tarefas tarefaAtualizada = await tarefasRepositorio.AtualizarStatusTarefa(tarefa, status);
			return tarefaAtualizada.ToTarefasResponse();
		}

		public async Task<Result<Unit, Error>> ExcluirTarefa(Guid idTarefa)
		{
			Usuario? usuario = await BuscarUsuario();
			if (usuario is null) return UsuarioFailure.TokenInvalido;
			Tarefas? tarefa = await tarefasRepositorio.BuscarTarefaPorId(idTarefa);
			if (tarefa is null) return TarefasFailure.TarefaNaoEncontrada;
			await tarefasRepositorio.ExcluirTarefa(tarefa, usuario);
			return Unit.Value;
		}
	}
}
