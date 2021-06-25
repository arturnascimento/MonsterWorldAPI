
namespace MonsterWorldApi.Models
{
    //model criado exclusivamente com o atributos da classe APIResponse utilizada na communicationsController classe generica<T>
    public class APIResponse<T>
    {
        public bool Succeed { get; set; }
        public string Message { get; set; }
        public T Results { get; set; }
    }
}
