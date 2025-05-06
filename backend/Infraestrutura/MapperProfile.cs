using AutoMapper;
using backend.Models.Dtos;
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
