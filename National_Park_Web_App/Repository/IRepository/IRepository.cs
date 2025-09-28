namespace National_Park_Web_App.Repository.IRepository
{
    public interface IRepository<T> where T : class 
    {
        Task<T> GetAsync(String url, int id);
        Task<IEnumerable<T>> GetAllAsync(string url);
        Task<bool> CreateAsync (String url , T ObjToCreate);
        Task<bool> UpdateAsync (String url, T ObjToUpdate);
        Task<bool> DeleteAsync (String url, int id);

    }
}
