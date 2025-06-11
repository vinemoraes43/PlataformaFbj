using AutoMapper;
using PlataformaFbj.Dto.Auth.Requests;
using PlataformaFbj.Dto.Auth.Responses;
using PlataformaFbj.Dto.Jogos.Requests;
using PlataformaFbj.Dto.Jogos.Responses;
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
            // Mapeamentos de ControllerCadastro
            // Mapeamentos para Registro
            CreateMap<RegisterRequestDto, Usuario>()
                .ForMember(dest => dest.SenhaHash, opt => opt.Ignore())
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => Enum.Parse<TipoUsuario>(src.Tipo)));

            CreateMap<Usuario, RegistrationResponse>()
                .ForMember(dest => dest.Token, opt => opt.Ignore());

            // Mapeamentos para Login
            CreateMap<Usuario, LoginResponse>()
                .ForMember(dest => dest.Token, opt => opt.Ignore());

            // Mapeamentos de ControllerUsuario
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

            // Mapeamento de ControllerJogo
            // Mapeamento para criar jogo
            CreateMap<JogoCreateDto, Jogo>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Feedbacks, opt => opt.Ignore())
                .ForMember(dest => dest.Desenvolvedor, opt => opt.Ignore());

            // Mapeamento para atualizar as informações do jogo
            CreateMap<JogoUpdateDto, Jogo>()
                .ForMember(dest => dest.Feedbacks, opt => opt.Ignore())
                .ForMember(dest => dest.Desenvolvedor, opt => opt.Ignore())

                .ForMember(dest => dest.Plataforma,
                       opt => opt.MapFrom(src => (PlataformaJogo)src.Plataforma))
                       .ForMember(dest => dest.DesenvolvedorId,
                       opt => opt.Condition(src => src.DesenvolvedorId.HasValue));

            // Mapeamento para mostrar resposta do jogo
            CreateMap<Jogo, JogoResponseDto>()
                .ForMember(dest => dest.Desenvolvedor, opt => opt.MapFrom(src => src.Desenvolvedor.Nome))
                .ForMember(dest => dest.MediaAvaliacoes, opt => opt.MapFrom(src =>
                    src.Feedbacks.Any() ? src.Feedbacks.Average(f => f.Nota) : 0));
            // Mapeamento para mostrar detalhes do jogo
            CreateMap<Jogo, JogoDetalhesDto>()
                .ForMember(dest => dest.Desenvolvedor, opt => opt.MapFrom(src => src.Desenvolvedor.Nome));
        }
    }
}