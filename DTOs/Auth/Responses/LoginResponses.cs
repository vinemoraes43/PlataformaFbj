namespace PlataformaFbj.Dto.Auth.Responses
{
    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
    }
}