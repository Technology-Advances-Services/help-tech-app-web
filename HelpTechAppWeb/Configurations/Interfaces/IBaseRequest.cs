namespace HelpTechAppWeb.Configurations.Interfaces
{
    public interface IBaseRequest<T> where T : class
    {
        Task<T?> GetAsync
            (string resource);

        Task<T?> GetAsync
            (string resource, string token);

        Task<T?> PostAsync
            (string resource, object content);

        Task<T?> PostAsync
            (string resource, string token,
            object content);
    }
}