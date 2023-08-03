using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace hospital
{
    /// <summary>
    /// Represents DB context
    /// </summary>
    public partial interface IDbContext
    {
        #region Methods

        DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity;

        int SaveChanges();

        string GenerateCreateScript();

        IQueryable<TQuery> QueryFromSql<TQuery>(string sql, params object[] parameters) where TQuery : class;

        IQueryable<TEntity> EntityFromSql<TEntity>(string sql, params object[] parameters) where TEntity : BaseEntity;

        

        void Detach<TEntity>(TEntity entity) where TEntity : BaseEntity;

        #endregion Methods
    }
}