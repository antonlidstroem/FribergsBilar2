namespace MarcusRent.Services.Base
{
    public partial interface IClient
    {
        HttpClient HttpClient { get; }
    }
}
