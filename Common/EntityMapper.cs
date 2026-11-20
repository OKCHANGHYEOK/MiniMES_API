namespace MiniMES_API.Common;

public class EntityMapper
{
    public static void Map<T>(T oldItem, T newItem) where T : class
    {
        var props = typeof(T).GetProperties();
        if (props is null)
            return;

        foreach (var prop in props)
        {
            if (prop.Name == $"{typeof(T).Name}Id")
            {
                continue;
            }

            var newValue = prop.GetValue(newItem);

            prop.SetValue(oldItem, newValue);
        }
    }
}
