using Microsoft.EntityFrameworkCore;

namespace MiniMES_API.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
}
