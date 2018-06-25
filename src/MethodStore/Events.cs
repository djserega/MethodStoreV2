using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodStore
{
    internal delegate void ParameterSearchEvent();

    public class ParameterSearchEvents : EventArgs 
    {
        internal event ParameterSearchEvent ChangedItemSearch;
        internal void EvokeParameterSearchEvent()
        {
            ChangedItemSearch?.Invoke();
        }
    }
}
