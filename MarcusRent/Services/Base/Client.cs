using System.Net.Http;

namespace MarcusRent.Services.Base
{

    public partial class Client : IClient
    {
        HttpClient IClient.HttpClient => _httpClient;
    }


}
