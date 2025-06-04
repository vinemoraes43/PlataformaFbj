using AutoMapper;
using PlataformaFbj.Dto.Auth.Requests;
using PlataformaFbj.Dto.Auth.Responses;
using PlataformaFbj.Dto.Usuarios.Requests;
using PlataformaFbj.Dto.Usuarios.Responses;
using PlataformaFbj.Enums;
using PlataformaFbj.Models;

namespace PlataformaFbj.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Mapeamentos para Registro
            CreateMap<RegisterRequestDto, Usuario>()
                .ForMember(dest => dest.SenhaHash, opt => opt.Ignore())
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => Enum.Parse<TipoUsuario>(src.Tipo)));

            CreateMap<Usuario, RegistrationResponse>()
                .ForMember(dest => dest.Token, opt => opt.Ignore());

            // Mapeamentos para Login
            CreateMap<Usuario, LoginResponse>()
                .ForMember(dest => dest.Token, opt => opt.Ignore());

            // Mapeamentos para Usuário
            // Mapeamento para criação de usuário
            CreateMap<UsuarioCreateDto, Usuario>()
                .ForMember(dest => dest.SenhaHash, opt => opt.Ignore())
                .ForMember(dest => dest.DataCriacao, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // Mapeamento para atualização de usuário
            CreateMap<UsuarioUpdateDto, Usuario>()
                .ForMember(dest => dest.SenhaHash, opt => opt.Ignore())
                .ForMember(dest => dest.DataCriacao, opt => opt.Ignore());

            // Mapeamento para resposta
            CreateMap<Usuario, UsuarioResponseDto>();
        }
    }
}