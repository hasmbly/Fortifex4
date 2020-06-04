namespace Fortifex4.Shared.Common
{
    public class ApiResponse<T>
    {
        public ApiResponseStatus Status { get; set; }

        public T Result { get; set; }
    }
}