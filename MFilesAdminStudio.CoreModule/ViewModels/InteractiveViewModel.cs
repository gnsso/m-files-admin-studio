using MFilesAdminStudio.CoreModule.Helpers;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFilesAdminStudio.CoreModule.ViewModels
{
    public abstract class InteractiveViewModel : BindableBase
    {
        private readonly Lazy<CollectionViewProvider> collections = new Lazy<CollectionViewProvider>(() => new CollectionViewProvider());

        private readonly Lazy<PropertyValueSwitcher> valueSwitcher = new Lazy<PropertyValueSwitcher>(() => new PropertyValueSwitcher());

        protected PropertyValueSwitcher ValueSwitcher => valueSwitcher.Value;
        protected CollectionViewProvider Collections => collections.Value;
    }
}
