namespace CricketApp.API.Common
{
    namespace CricketApp.API.Common
    {
        public class ApiResponse<T>
        {
            public bool Success { get; set; }
            public string? Message { get; set; }
            public T? Data { get; set; }
        }
    }

}
