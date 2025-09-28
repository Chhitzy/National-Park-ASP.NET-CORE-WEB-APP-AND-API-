using National_Park_Web_App.Models;
using National_Park_Web_App.Repository.IRepository;

namespace National_Park_Web_App.Repository
{
    public class NationalParkRepository: Repository<NationalPark>,INationalParkRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public NationalParkRepository(IHttpClientFactory httpClientFactory):base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
    }
}
