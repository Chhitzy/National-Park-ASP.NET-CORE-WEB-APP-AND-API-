using National_Park_Web_App.Models;
using National_Park_Web_App.Repository.IRepository;

namespace National_Park_Web_App.Repository
{
    public class    TrailRepository:Repository<Trail>,ITrailRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public TrailRepository(IHttpClientFactory httpClientFactory):base(httpClientFactory) 
        {
            _httpClientFactory = httpClientFactory;
        }
    }
}
