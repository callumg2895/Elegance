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

        public void LoadStaticData(string name)
        {
            var staticDataPath = Path.Combine(_resourcesDirectory.FullName, "StaticData");
            var filePath = Path.Combine(staticDataPath, $"{name}.sql");
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

        public void LoadStoredProcedure(string name)
        {
            var storedProceduresPath = Path.Combine(_resourcesDirectory.FullName, "StoredProcedures");
            var filePath = Path.Combine(storedProceduresPath, $"{name}.sql");
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
