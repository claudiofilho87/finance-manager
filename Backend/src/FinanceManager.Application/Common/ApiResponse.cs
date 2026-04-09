namespace FinanceManager.Application.Common;

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public T? Data { get; set; }
    public string? Error { get; set; }

    public ApiResponse(bool isSuccess, string message, T? data, string? error = null)
    {
        IsSuccess = isSuccess;
        Message = message;
        Data = data;
        Error = error;
    }

    public static ApiResponse<T> SuccessResponse(T data, string message = "Request Successful")
    {
        return new ApiResponse<T>(true, message, data);
    }

    public static ApiResponse<T> ErrorResponse(string error, string message = "Request Failed")
    {
        return new ApiResponse<T>(false, message, default, error);
    }
}
