namespace QShirt.Public.Proxy.ServiceProxies
{
    public class DataResult<T>
    {
        public IEnumerable<T> Data { get; set; }

        public int Total { get; set; }
    }
}