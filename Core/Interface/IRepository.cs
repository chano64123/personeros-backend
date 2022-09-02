using Core.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interface {
    public interface IRepository<TEntity> where TEntity: class{
        Task<IReadOnlyList<TEntity>> obtenerTodosAsync();
        Task<IReadOnlyList<TEntity>> obtenerTodosEspecificacionAsync(ISpecification<TEntity> espec);

        Task<TEntity> obtenerPorIdAsync(int id);
        Task<TEntity> obtenerPorIdEspecificoAsync(ISpecification<TEntity> espec);

        Task<TEntity> crearAsync(TEntity objeto);
        Task<TEntity> actualizarAsync(TEntity objeto);
        Task<bool> eliminarPorIdAsync(int id);
    }
}
