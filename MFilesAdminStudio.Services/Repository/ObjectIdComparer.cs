using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAdminStudio.Repository
{
    public class ObjectIdComparer : IEqualityComparer<ObjectId>
    {
        public bool Equals(ObjectId x, ObjectId y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(ObjectId obj)
        {
            return 0;
        }
    }
}
