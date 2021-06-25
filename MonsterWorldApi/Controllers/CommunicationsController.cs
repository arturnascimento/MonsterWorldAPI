using Microsoft.AspNetCore.Mvc;
using MonsterWorldApi.Models;

namespace MonsterWorldApi.Controllers
{
    public class CommunicationsController: ControllerBase
    {
        //controller criado para a normalização dos padroes de resposta herdando a classe ControllerBase
        protected IActionResult ApiOk<T>(T Results) => Ok(CustomResponse(true, "", Results));
        protected IActionResult ApiOk<T>(string Message, T Results) => Ok(CustomResponse(true, Message, Results));
        protected IActionResult ApiNotFound(string Message) => NotFound(CustomResponse(false, Message, ""));

        APIResponse<T> CustomResponse<T>(bool Succeed, string Message, T Results)
        {
            return new APIResponse<T>()
            {
                Succeed = Succeed,
                Message = Message,
                Results = Results
            };
        }
        
    }
}
