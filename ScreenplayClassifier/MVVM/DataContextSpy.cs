using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ScreenplayClassifier.MVVM
{
    class DataContextSpy : Freezable
    {
        // Fields
        public static readonly DependencyProperty DataContextProperty = FrameworkElement.DataContextProperty.AddOwner(typeof(DataContextSpy),
            new PropertyMetadata(null, null, OnCoerceDataContext));

        // Properties
        public object DataContext
        {
            get { return (object)GetValue(DataContextProperty); }
            set { SetValue(DataContextProperty, value); }
        }

        public bool IsSynchronizedWithCurrentItem { get; set; }

        // Constructors
        public DataContextSpy()
        {
            IsSynchronizedWithCurrentItem = true;

            BindingOperations.SetBinding(this, DataContextProperty, new Binding());

        }

        // Methods
        public static object OnCoerceDataContext(DependencyObject depObj, object value)
        {
            DataContextSpy spy = (DataContextSpy)depObj;
            ICollectionView view;

            // Validation
            if (spy == null)
                return value;

            if (spy.IsSynchronizedWithCurrentItem)
            {
                view = CollectionViewSource.GetDefaultView(value);
                if (view != null)
                    return view.CurrentItem;
            }

            return value;
        }

        #region Implementations
        protected override Freezable CreateInstanceCore()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
