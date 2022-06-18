namespace CustomerApplication.Interfaces.Settings
{
    public interface IHttpRequestSettings
    {
        public string BaseUrl { get; set; }
        public int Port { get; set; }
    }
}