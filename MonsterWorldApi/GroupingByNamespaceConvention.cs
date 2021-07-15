using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Linq;

namespace MonsterWorldApi
{
    /// <summary>
    /// Classe que gerencia as versões
    /// </summary>
    public class GroupingByNamespaceConvention : IControllerModelConvention
    {
        /// <summary>
        /// Método que pega a versão do namespace da controller e adiciona a controller no grupo certo.
        /// </summary>
        /// <param name="controller"></param>
        public void Apply(ControllerModel controller)
        {
            var controllernamespace = controller.ControllerType.Namespace; 
            var apiVersion = controllernamespace.Split(".").Last().ToLower();
            if (!apiVersion.StartsWith("v")) apiVersion = "v1";

            controller.ApiExplorer.GroupName = apiVersion;
        }
    }



}