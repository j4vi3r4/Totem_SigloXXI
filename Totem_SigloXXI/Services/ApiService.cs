
namespace Totem_SigloXXI.Services
{
    using Newtonsoft.Json;
    using Plugin.Connectivity;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Totem_SigloXXI.Modelo;
    using Xamarin.Forms;

    public class ApiService
    {
        // los metodos async depende de la data que esta al otro lado

        //Tarea para validar conexión a internet 

        public async Task<Response> CheckConnection()
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "Enciende tu Internet",
                };
            }
            var isReachable = await CrossConnectivity.Current.IsRemoteReachable("google.com");
            if (!isReachable)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = "No Hay Conexión a Internet",
                };
            }
            return new Response
            {
                IsSuccess = true,
            };
        
        }


        public async Task<Response> GetList<T>(string urlBase, string prefix, string controller) //metodo que trae lista generica, en el llamado se nombra la lista
        {
            try
            {
                //para consumir un servicio rest
                //1 - crear un cliente
                var client = new HttpClient(); // nuevo objaeto de la case htttpclient, sirve para hacer la comunicación
                //2.- cargar la dirección
                client.BaseAddress = new Uri(urlBase);
                //3.- se concatena el prefijo y el controlador para obtener la lista
                var url = $"{prefix}{controller}"; // equivalente al string.format antiguo 
                //request
                var response = await client.GetAsync(url); //en este momento se queda esperando cuando vuelve trae una respuesta esta se debe leer del json
                var answer = await response.Content.ReadAsStringAsync(); //answer es el json pero como un string -> se debe convertir
                if (!response.IsSuccessStatusCode) //si fallo se devuelve al usuario que no se pudo conectar
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }
                //el json se convierte en una lista de objetos
                var list = JsonConvert.DeserializeObject<List<T>>(answer);
                return new Response
                {
                    IsSuccess = true,
                    Result = list,
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false, // en caso que no ha podido obtener la comunicación  
                    Message = ex.Message,
                };
            }
        }

        public async Task<Response> PostUpdate<T>(string jsonObject, string urlBase, string prefix, string controller) {
            try
            {
                // Crea un objeto que maneje la conexión HTTP
                HttpClient socket = new HttpClient();

                //Parsea la url en solo un string
                string url = $"{urlBase}{prefix}{controller}";

                //Convierte el json en el body
                var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");

                //Ejecuta la conexión a la API como POST
                var response = await socket.PostAsync(url, content);

                //Parsea la respuesta como string
                string answer = await response.Content.ReadAsStringAsync();

                //Devuelve error desde la API
                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = answer,
                    };
                }

                //el json se convierte en una lista de objetos
                var list = JsonConvert.DeserializeObject<List<T>>(answer);
                return new Response
                {
                    IsSuccess = true,
                    Result = list,
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
