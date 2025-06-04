namespace PlataformaFbj.Dto
{
    public class ErrorResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
    }
}