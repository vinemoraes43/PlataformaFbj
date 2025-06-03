using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using PlataformaFbj.Data;
using PlataformaFbj.Dto.Auth.Requests;
using System.Linq;
using System.Threading.Tasks;

namespace PlataformaFbj.Filters
{
    public class ValidateUniqueEmailAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(
            ActionExecutingContext context, 
            ActionExecutionDelegate next)
        {
            var dbContext = context.HttpContext
                .RequestServices.GetService<AppDbContext>();

            if (context.ActionArguments.TryGetValue("dto", out var dtoObj) &&
                dtoObj is RegisterRequestDto dto)
            {
                if (await dbContext.Usuarios.AnyAsync(u => u.Email == dto.Email))
                {
                    context.Result = new BadRequestObjectResult(new
                    {
                        Success = false,
                        Message = "Email já cadastrado",
                        Errors = new[] { "Este email já está em uso" }
                    });
                    return;
                }
            }

            await next();
        }
    }
}