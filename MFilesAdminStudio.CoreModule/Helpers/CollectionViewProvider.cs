using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace MFilesAdminStudio.CoreModule.Helpers
{
    public class CollectionViewProvider
    {
        private readonly IDictionary<string, IList> originalCollections = new Dictionary<string, IList>();

        public IList GetSource(string name)
        {
            return originalCollections[name];
        }

        public IList<T> GetSource<T>(string name)
        {
            return originalCollections[name].Cast<T>().ToList();
        }

        public ViewRequestToken SetSource(string name, IList collection)
        {
            originalCollections[name] = collection;

            return new ViewRequestToken
            {
                CollectionName = name,
                LoadedCount = 0,
                CountPerRequest = collection.Count,
                IsPartialView = false,
                TotalCount = collection.Count,
                Predicate = null
            };
        }

        public ICollectionView GetView(ViewRequestToken requestToken)
        {
            return Application.Current.Dispatcher.Invoke(delegate
            {
                if (requestToken.IsPartialView)
                {
                    var collection = originalCollections[requestToken.CollectionName];

                    var predicatedCollection = collection
                        .Cast<object>()
                        .Where(requestToken.Predicate ?? (obj => true))
                        .ToList();

                    requestToken.TotalCount = predicatedCollection.Count;
                    requestToken.LoadedCount += requestToken.CountPerRequest;

                    var partialCollection = predicatedCollection
                        .Take(requestToken.LoadedCount)
                        .ToList();

                    return CollectionViewSource.GetDefaultView(partialCollection);
                }
                else
                {
                    var view = CollectionViewSource.GetDefaultView(originalCollections[requestToken.CollectionName]);
                    if (requestToken.Predicate != null)
                    {
                        view.Filter = obj => requestToken.Predicate(obj);
                    }
                    return view;
                }
            }, DispatcherPriority.ContextIdle);
        }
    }
}
