
using DashBoradApp.DataAccess.Entitites;
using System.Data.Entity.ModelConfiguration;

namespace DashBoradApp.DataAccess.Configurations
{
    public class EntityConfiguration<T> : EntityTypeConfiguration<T> where T : class, IEntity
    {
        public EntityConfiguration()
        {
            HasKey(e => e.ID); 
        }
    }
}