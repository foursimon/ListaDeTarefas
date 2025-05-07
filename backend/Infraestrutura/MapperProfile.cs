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
			CreateMap<Usuario, UsuarioResponse>();
			CreateMap<UsuarioUpdate, Usuario>()
				.ForAllMembers(opt => 
					opt.Condition((src, dest, prop) => prop is not null));
		}
	}
}
