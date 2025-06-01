using AutoMapper;
using PlataformaFbj.Dto.Auth.Requests;
using PlataformaFbj.Dto.Auth.Responses;
using PlataformaFbj.Enums;
using PlataformaFbj.Models;

namespace PlataformaFbj.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Auth
            CreateMap<RegisterRequestDto, Usuario>()
                .ForMember(dest => dest.SenhaHash, opt => opt.Ignore())
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => Enum.Parse<TipoUsuario>(src.Tipo)));

            // Usuários
            //CreateMap<Usuario, UsuarioResponseDto>();
            //CreateMap<UsuarioCreateDto, Usuario>()
                //.ForMember(dest => dest.SenhaHash, opt => opt.Ignore());

            // Jogos
            //CreateMap<Jogo, JogoResponseDto>();
            //CreateMap<JogoCreateDto, Jogo>();

            // Feedbacks
            //CreateMap<Feedback, FeedbackResponseDto>();
            //CreateMap<FeedbackCreateDto, Feedback>();
        }
    }
}