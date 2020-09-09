using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Elegance.Core.Tests.Data.TestRepository
{
    public class ResourceRepository : BaseRepository
    {
        private readonly DirectoryInfo _binDirectory;
        private readonly DirectoryInfo _projectDirectory;
        private readonly DirectoryInfo _resourcesDirectory;

        public ResourceRepository()
        {
            _binDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            _projectDirectory = _binDirectory.Parent.Parent.Parent;
            _resourcesDirectory = new DirectoryInfo(Path.Combine(_projectDirectory.FullName, "Resources"));
        }

        public void LoadStoredProcedure(string name)
        {
            var resourcesPath = Path.Combine(_resourcesDirectory.FullName);
            var filePath = Path.Combine(resourcesPath, $"{name}.sql");
            var sql = File.ReadAllText(filePath);
            var statements = sql.Split("GO");

            using var session = CreateSession();

            foreach (var statement in statements)
            {
                session
                    .CreateCommand(statement)
                    .ExecuteNonQuery();
            }

        }

        public void UnloadStoredProcedures(string name)
        {
            var sql = $@"
                drop procedure {name}
            ";

            using var session = CreateSession();

            session
                .CreateCommand(sql)
                .ExecuteNonQuery();
        }
    }
}
