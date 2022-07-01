using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAdminStudio.CoreModule.Helpers
{
    public class ViewRequestToken
    {
        public string CollectionName { get; set; }
        public int CountPerRequest { get; set; }
        public bool IsPartialView { get; set; }
        public int LoadedCount { get; set; }
        public int TotalCount { get; set; }
        public Func<object, bool> Predicate { get; set; }

        public bool HasMoreItems => LoadedCount < TotalCount;
        public int MoreItemsCount => TotalCount - LoadedCount;
    }
}
