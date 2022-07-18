using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenplayClassifier.MVVM
{
    public interface IReflectProperty
    {
        // Methods
        object GetPropertyByName(string propertyName);
    }
}
