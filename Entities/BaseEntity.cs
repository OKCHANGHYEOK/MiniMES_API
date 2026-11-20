using MiniMES_API.DTOs;

namespace MiniMES_API.Entities;

public class BaseEntity
{
    public T? ToResponse<T>() where T : BaseDTO, new()
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
