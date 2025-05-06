using AutoMapper;
using backend.Models.Dtos.UsuarioDto;
using backend.Models.Entities;

namespace backend.Infraestrutura
{
	public class MapperProfile : Profile
	{
		public MapperProfile() 
		{
			CreateMap<UsuarioCreate, Usuario>();
		}
	}
}
