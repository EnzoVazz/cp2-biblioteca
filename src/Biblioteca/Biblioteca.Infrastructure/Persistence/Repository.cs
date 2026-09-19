using Biblioteca.Application.Interfaces;
using Biblioteca.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Infrastructure.Persistence;

/// <summary>
/// Implementação genérica de <see cref="IRepository{T}"/> com EF Core.
/// Leituras usam <c>AsNoTracking</c> para evitar tracking desnecessário.
/// </summary>
public class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly BibliotecaContext _context;
    protected readonly DbSet<T> _dbSet;
    private readonly string _primaryKeyName;

    public Repository(BibliotecaContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();

        var entityType = context.Model.FindEntityType(typeof(T))
            ?? throw new InvalidOperationException($"O tipo {typeof(T).Name} não está mapeado no DbContext.");

        var primaryKey = entityType.FindPrimaryKey()
            ?? throw new InvalidOperationException($"O tipo {typeof(T).Name} não possui chave primária.");

        _primaryKeyName = primaryKey.Properties[0].Name;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.AsNoTracking()
            .FirstOrDefaultAsync(entity => EF.Property<Guid>(entity, _primaryKeyName) == id);
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsByIdAsync(Guid id)
    {
        return await _dbSet.AsNoTracking()
            .AnyAsync(entity => EF.Property<Guid>(entity, _primaryKeyName) == id);
    }
}
