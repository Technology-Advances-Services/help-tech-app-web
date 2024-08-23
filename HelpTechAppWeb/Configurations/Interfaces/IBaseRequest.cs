namespace HelpTechAppWeb.Configurations.Interfaces
{
    public interface IBaseRequest
    {
        Task<dynamic?> GetAsync
            (string resource);

        Task<dynamic?> GetAsync
            (string resource, string token);

        Task<dynamic?> PostAsync
            (string resource, object content);

        Task<dynamic?> PostAsync
            (string resource, string token,
            object content);
    }
}