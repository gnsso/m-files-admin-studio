using MFilesAdminStudio.CoreModule.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAdminStudio.CoreModule.Helpers
{
    public class SingleListItemSelector
    {
        public static void SelectSingle<T>(IList<T> selectables, T toBeSelected) where T : ISelectableViewModel
        {
            var currentlySelected = selectables.SingleOrDefault(s => s.IsSelected);

            if (currentlySelected == null)
            {
                toBeSelected.IsSelected = true;
                return;
            }

            if (ReferenceEquals(currentlySelected, toBeSelected))
            {
                return;
            }

            currentlySelected.IsSelected = false;
            toBeSelected.IsSelected = true;
        }

        public static void SelectSingle<T>(IList<T> selectables, Func<T, bool> toBeSelectedPredicate) where T : ISelectableViewModel
        {
            SelectSingle(selectables, selectables.SingleOrDefault(toBeSelectedPredicate));
        }
    }
}
