namespace ServiceFamilyBank.Models.Responses
{
    public class Response<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public object Meta { get; set; } // Nuevo campo para metadatos
    }

    public class PaginatedData<T>
    {
        public int Total { get; set; }
        public List<T> Data { get; set; }
    }
}
