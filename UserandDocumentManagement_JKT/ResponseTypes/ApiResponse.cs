namespace UserandDocumentManagement_JKT.ResponseTypes
{
    public class ApiResponse<T>
    {
        public string DateTime { get; set; }
        public T Data { get; set; }
        public object? Error { get; set; }
        public int Status { get; set; }

        public ApiResponse(T data, int status)
        {
            DateTime = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            Data = data;
            Error = null;
            Status = status;
        }

        public ApiResponse(object error, int status)
        {
            DateTime = System.DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            Data = default!;
            Error = error;
            Status = status;
        }
    }
}
