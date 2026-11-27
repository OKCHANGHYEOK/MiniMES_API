using MiniMES_API.Entities;

namespace MiniMES_API.DTOs;

public class DataResponse<T> where T : class
{
    public int ResponseCode { get; set; }
    public string? Message { get; set; }
    public bool IsSuccess { get; set; }

    public T? Item { get; set; }
    public List<T>? Items { get; set; }

    public static DataResponse<T> Success(T? item = null, IEnumerable<T>? items = null, string? message = null, int responseCode = 200)
    {
        return new DataResponse<T>
        {
            ResponseCode = responseCode,
            Message = message,
            IsSuccess = true,
            Item = item,
            Items = items?.ToList()
        };
    }

    public static DataResponse<T> Failure(int responseCode, string message)
    {
        return new DataResponse<T>
        {
            ResponseCode = responseCode,
            Message = message,
            IsSuccess = false
        };
    }
}
