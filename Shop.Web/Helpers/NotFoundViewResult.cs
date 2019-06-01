namespace Shop.Web.Helpers
{
    using System.Net;
    using Microsoft.AspNetCore.Mvc;

    public class NotFoundViewResult : ViewResult // Hereda de un ViewResult que devuelve un ActionResult que muestra una vista de respuesta
    {
        // Administrar errores Not Found 

        // Método para mostrar una vista de respuesta
        public NotFoundViewResult(string viewName)
        {
            // Obtiene o establece el nombre o la ruta de la vista que devuelve en la respuesta
            ViewName = viewName;

            // StatusCode: Obtiene o establece el código de estado HTTP
            // HttpStatusCode: Contiene los valores de los códigos de estado definidos para HTTP.
            // NotFound: Equivalente al estado HTTP 404 indica que el recurso solicitado no existe en el servidor
            StatusCode = (int)HttpStatusCode.NotFound;
        }

    }
}
