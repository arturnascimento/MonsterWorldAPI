
namespace MonsterWorldApi.Models
{
    //model criado exclusivamente com o atributos da classe APIResponse utilizada na communicationsController classe generica<T>

    /// <summary>
    /// Classe modelo para padronizar as respostas da API.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class APIResponse<T>
    {
        /// <summary>
        /// Atributo tipo bool 
        /// </summary>
        public bool Succeed { get; set; }
        /// <summary>
        /// Atributo tipo string
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Atributo tipo T
        /// </summary>
        public T Results { get; set; }
    }
}
