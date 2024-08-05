using Dapper.Dommel.Abstractions;

namespace InventoryControl.Infra.Data.Conventions
{
    public class TableNameResolver : ITableNameResolver
    {
        public string ResolveTableName(Type type)
        {
            return $"{type.Name}";
        }
    }
}
