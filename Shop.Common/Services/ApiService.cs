namespace Shop.Common.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Models;
    using Newtonsoft.Json;
    
    // Esta clase tiene que ver todo lo que es consumo de Api

    public class ApiService
    {

        // urlBase:         https://shopceqn.azurewebsites.net
        // servicePrefix:   /Api
        // controller:      /Products

        // Método asíncrono que devuelve una lista genérica (al igual que Postman)
        public async Task<Response> GetListAsync<T>(string urlBase, string servicePrefix, string controller)
        {
            try
            {
                // API: Application Programming Interface, o Interfaz de programación de Aplicaciones al conjunto de rutinas, funciones y procedimientos(métodos) que permite utilizar recursos de un software por otro, 
                //      sirviendo como una capa de abstracción o intermediario.

                // REST (Representational State Transfer) es una arquitectura que se ejecuta sobre HTTP.
                // RESTful sucesor de métodos anteriores como SOAP y WSDL, hace referencia a un servicio web que implementa la arquitectura REST.

                // Consumir un RESTful (servicio web)

                var client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase)
                };

                var url = $"{servicePrefix}{controller}";

                // Ejecuta el método enviando una solicitud GET al Uri especificado como una operación asincrónica (al igual que Postman)
                var response = await client.GetAsync(url);
                
                // La respuesta es leida como String 
                var result = await response.Content.ReadAsStringAsync();

                // Se obtiene un valor que indica si la respuesta HTTP fue exitosa (Status 200 OK)
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                // Serializar: De JSON al tipo .NET especificado (string)
                // Deserializar: Del tipo .NET especificado (string) a JSON                 
                var list = JsonConvert.DeserializeObject<List<T>>(result); // Aqui falla si mandamos fechas con valor null

                return new Response
                {
                    IsSuccess = true,
                    Result = list
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

    }
}
