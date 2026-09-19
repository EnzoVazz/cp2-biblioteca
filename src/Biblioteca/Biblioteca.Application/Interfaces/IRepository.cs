using Biblioteca.Domain.Common;

namespace Biblioteca.Application.Interfaces;

/// <summary>
/// Contrato genérico de persistência para agregados que herdam de <see cref="BaseEntity"/>.
/// </summary>
/// <typeparam name="T">Tipo da entidade de domínio.</typeparam>
public interface IRepository<T> where T : BaseEntity
{
    /// <summary>
    /// Obtém todos os registros do agregado.
    /// </summary>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Obtém um registro pela chave primária.
    /// </summary>
    /// <param name="id">Identificador da entidade.</param>
    /// <returns>A entidade encontrada ou <c>null</c>.</returns>
    Task<T?> GetByIdAsync(Guid id);

    /// <summary>
    /// Inclui uma nova entidade e persiste a alteração.
    /// </summary>
    Task AddAsync(T entity);

    /// <summary>
    /// Atualiza uma entidade existente e persiste a alteração.
    /// </summary>
    Task UpdateAsync(T entity);

    /// <summary>
    /// Remove uma entidade e persiste a alteração.
    /// </summary>
    Task DeleteAsync(T entity);

    /// <summary>
    /// Indica se existe um registro com o identificador informado.
    /// </summary>
    Task<bool> ExistsByIdAsync(Guid id);
}
