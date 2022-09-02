using Core.Interface;
using Core.Specification;
using Infraestructura.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Repository {
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class {
        private ApplicationDbContext db;

        public Repository(ApplicationDbContext db) {
            this.db = db;
        }

        public async Task<TEntity> actualizarAsync(TEntity objeto) {
            db.Update(objeto);
            await db.SaveChangesAsync();
            return objeto;
        }

        public async Task<TEntity> crearAsync(TEntity objeto) {
            await db.AddAsync(objeto);
            await db.SaveChangesAsync();
            return objeto;
        }

        public async Task<bool> eliminarPorIdAsync(int id) {
            try {
                TEntity obj = await db.Set<TEntity>().FindAsync(id);
                if (obj == null) {
                    return false;
                }
                db.Remove(obj);
                await db.SaveChangesAsync();
                return true;
            } catch (Exception) {
                return false;
            }
        }

        public async Task<TEntity> obtenerPorIdAsync(int id) {
            return await db.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity> obtenerPorIdEspecificoAsync(ISpecification<TEntity> espec) {
            return await aplicarEspecificacion(espec).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<TEntity>> obtenerTodosAsync() {
            return await db.Set<TEntity>().ToListAsync();
        }

        public async Task<IReadOnlyList<TEntity>> obtenerTodosEspecificacionAsync(ISpecification<TEntity> espec) {
            return await aplicarEspecificacion(espec).ToListAsync();
        }

        private IQueryable<TEntity> aplicarEspecificacion(ISpecification<TEntity> espec) {
            return EvaluatorSpecification<TEntity>.GetQuery(db.Set<TEntity>().AsQueryable(), espec);

        }
    }
}
