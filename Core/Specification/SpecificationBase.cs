using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specification {
    public class SpecificationBase<TEntity> : ISpecification<TEntity> {
        public SpecificationBase(Expression<Func<TEntity, bool>> filtro) {
            Filtro = filtro;
        }
        public SpecificationBase() {
        }

        public Expression<Func<TEntity, bool>> Filtro { get; }

        public List<Expression<Func<TEntity, object>>> Includes { get; } = new List<Expression<Func<TEntity, object>>>();

        protected void AgregarInclude(Expression<Func<TEntity, object>> includeExpression) {
            Includes.Add(includeExpression);
        }
    }
}
