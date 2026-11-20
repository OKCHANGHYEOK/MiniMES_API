using MiniMES_API.Entities;

namespace MiniMES_API.DTOs;

public class BaseDTO
{
    public string? keyword { get; set; }
    public string? SortField { get; set; }
    public string? SortDir { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }


    public T? ToEntity<T>() where T : BaseEntity, new()
    {
        var props = typeof(T).GetProperties();
        if (props is null)
            return null;

        var item = new T();

        foreach (var prop in props)
        {
            var sourceProperty = GetType().GetProperty(prop.Name);
            if (sourceProperty is null || !prop.CanWrite)
            {
                continue;
            }

            var value = sourceProperty.GetValue(this);
            prop.SetValue(item, value);
        }

        return item;
    }
}
