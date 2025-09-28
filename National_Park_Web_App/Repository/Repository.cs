using National_Park_Web_App.Repository.IRepository;
using Newtonsoft.Json;
using System.Text;

namespace National_Park_Web_App.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IHttpClientFactory _httpclientfactory;
        public Repository(IHttpClientFactory htp)
        {
            _httpclientfactory = htp;
        }
        public async Task<bool> CreateAsync(string url, T ObjToCreate)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if(ObjToCreate != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(ObjToCreate),Encoding.UTF8,("application/json"));
            }
            var client = _httpclientfactory.CreateClient();
            HttpResponseMessage httpResponse = await client.SendAsync(request); 
            if(httpResponse.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteAsync(string url, int id)
        {
           var request = new HttpRequestMessage(HttpMethod.Delete, url +"/" + id.ToString());
            var Client = _httpclientfactory.CreateClient();
            HttpResponseMessage httpResponse = await Client.SendAsync(request);

            if(httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<T>> GetAllAsync(string url)
        {
           var request = new HttpRequestMessage(HttpMethod.Get, url);
            var Client = _httpclientfactory.CreateClient();
            HttpResponseMessage httpResponse = await Client.SendAsync(request);
            if(httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var JsonString  = await httpResponse.Content.ReadAsStringAsync();   
                return JsonConvert.DeserializeObject<IEnumerable<T>>(JsonString);
            }
            return null;
        }

        public async Task<T> GetAsync(string url, int id)
        {
           var request = new HttpRequestMessage (HttpMethod.Get, url + "/"+ id.ToString());
            var Client = _httpclientfactory.CreateClient();
            HttpResponseMessage httpReponse = await Client.SendAsync(request);
            if (httpReponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var Jsonstring = await httpReponse.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(Jsonstring);
            }
            return null;
        }

        public async Task<bool> UpdateAsync(string url, T ObjToUpdate)
        {
          var request = new HttpRequestMessage(HttpMethod.Put, url);
            if(ObjToUpdate != null)
            {
                request.Content = new StringContent (
                    JsonConvert.SerializeObject(ObjToUpdate),Encoding.UTF8,("application/json"));
            }
             var client = _httpclientfactory.CreateClient();
            HttpResponseMessage httpResponse =  await client.SendAsync(request);
            if (httpResponse.StatusCode == System.Net.HttpStatusCode.NoContent)
                return true;
            return false;
        }

    }
}
