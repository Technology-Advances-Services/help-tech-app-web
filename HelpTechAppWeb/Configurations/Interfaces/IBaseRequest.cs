namespace HelpTechAppWeb.Configurations.Interfaces
{
    public interface IBaseRequest
    {
        Task<bool> PostAsync<T>
            (string resource, T content);

        Task<bool> PostAsync<T>
            (string resource, string token,
            T content);

        Task<IEnumerable<T>> GetAsync<T>
            (string resource);

        Task<IEnumerable<T>> GetAsync<T>
            (string resource, string token);

        Task<T?> GetSingleAsync<T>
            (string resource);

        Task<T?> GetSingleAsync<T>
            (string resource, string token);
    }
}