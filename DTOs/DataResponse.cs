using MiniMES_API.Entities;

namespace MiniMES_API.DTOs;

public class DataResponse<T> where T : class
{
    public int ResponseCode { get; set; }
    public string? Message { get; set; }
    public bool IsSuccess { get; set; }

    public T? Item { get; set; }
    public List<T>? Items { get; set; }
}
