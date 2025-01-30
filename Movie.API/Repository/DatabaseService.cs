namespace Movie.API.Repository
{
    public class DatabaseService(ApplicationDbContext context) : IDatabaseService
    {
        public void DropAllTables()
        {
            using (context)
            {
                var dropForeignKeysSql = @"
                        DECLARE @sql NVARCHAR(MAX) = '';
                        SELECT @sql += 'ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(parent_object_id)) + '.' + QUOTENAME(OBJECT_NAME(parent_object_id)) +
                                       ' DROP CONSTRAINT ' + QUOTENAME(name) + ';' 
                        FROM sys.foreign_keys;
                        EXEC sp_executesql @sql;";

                context.Database.ExecuteSqlRaw(dropForeignKeysSql);

                var dropTablesSql = @"
                        DECLARE @sql NVARCHAR(MAX) = '';
                        SELECT @sql += 'DROP TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(object_id)) + '.' + QUOTENAME(name) + ';' 
                        FROM sys.tables;
                        EXEC sp_executesql @sql;";

                context.Database.ExecuteSqlRaw(dropTablesSql);
            }
        }
    }
}
