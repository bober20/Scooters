namespace Domain.Models;

public class ResponseData<T>
{
    public T? Data { get; private set; }
    public string? ErrorMessage { get; private set; }
    public bool IsSuccessful { get; private set; } = true;

    public static ResponseData<T> Success(T data)
    {
        return new ResponseData<T>
        {
            Data = data
        };
    }
    
    public static ResponseData<T> Failure(string errorMessage, T? data = default)
    {
        return new ResponseData<T>
        {
            ErrorMessage = errorMessage,
            IsSuccessful = false
        };
    }
}