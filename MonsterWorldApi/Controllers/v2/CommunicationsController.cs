using Microsoft.AspNetCore.Mvc;
using MonsterWorldApi.Models;

namespace MonsterWorldApi.Controllers.v2
{
    /// <summary>
    /// Controller responsável pelos retornos de requisições da API.
    /// </summary>
    public class CommunicationsController: ControllerBase
    {
        //controller criado para a normalização dos padroes de resposta herdando a classe ControllerBase

        /// <summary>
        /// Retorno positivo customizado.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Results"></param>
        /// <returns></returns>
        protected IActionResult ApiOk<T>(T Results) => Ok(CustomResponse(true, "", Results));
        /// <summary>
        /// Retorno positivo customizado.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Message"></param>
        /// <param name="Results"></param>
        /// <returns></returns>
        protected IActionResult ApiOk<T>(string Message, T Results) => Ok(CustomResponse(true, Message, Results));
        /// <summary>
        /// Resposta de negativa  de página nao encontrada.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Message"></param>
        /// <returns></returns>
        protected IActionResult ApiNotFound<T>(string Message) => NotFound(CustomResponse(false, Message, (T)default));
        /// <summary>
        /// Resposta negativa de uma requisição sem sucesso.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Results"></param>
        /// <returns></returns>
        protected IActionResult ApiBadRequest<T>(T Results) => BadRequest(CustomResponse(false, "", Results));

        APIResponse<T> CustomResponse<T>(bool Succeed = true, string Message = "", T Results = default)
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
