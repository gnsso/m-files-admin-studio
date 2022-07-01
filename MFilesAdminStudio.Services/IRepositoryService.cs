using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAdminStudio.Services
{
    public interface IRepositoryService
    {
        string FilePath { get; }

        LiteDatabase GetDatabase();
        LiteDatabase UseCollection<T>(out LiteCollection<T> collection);
        LiteDatabase UseCollection<T>(string name, out LiteCollection<T> collection);
        LiteDatabase UseCollection(string name, out LiteCollection<BsonDocument> collection);
    }
}
