using Microsoft.EntityFrameworkCore;

namespace MiniMES_API.ModelBuilders;

public interface IEntityModelBuilder
{
    abstract static void SetModelBuilder(ModelBuilder modelBuilder);
}
