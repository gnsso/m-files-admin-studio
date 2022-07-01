using LiteDB;
using MFilesAdminStudio.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAdminStudio.Repository
{
    public class RepositoryService : IRepositoryService
    {
        private readonly string dbFilePath;

        public string FilePath => dbFilePath;

        public RepositoryService(IFileSystemService fileService, string dbFileName)
        {
            dbFilePath = fileService.GetFilePath(dbFileName);
        }

        public LiteDatabase GetDatabase()
        {
            return new LiteDatabase(dbFilePath);
        }

        public LiteDatabase UseCollection<T>(out LiteCollection<T> collection)
        {
            var database = GetDatabase();

            collection = database.GetCollection<T>();

            return database;
        }

        public LiteDatabase UseCollection<T>(string name, out LiteCollection<T> collection)
        {
            var database = GetDatabase();

            collection = database.GetCollection<T>(name);

            return database;
        }

        public LiteDatabase UseCollection(string name, out LiteCollection<BsonDocument> collection)
        {
            var database = GetDatabase();

            collection = database.GetCollection(name);

            return database;
        }
    }
}
