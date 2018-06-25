using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodStore
{
    internal class ParametersNavigating
    {
        private object[] _parameters;

        internal object[] Parameters
        {
            get => _parameters;
            set
            {
                _parameters = value;
                if (value != null)
                    CountParameters = value.Count();
            }
        }

        internal Type PreviousPage { get; set; }
        internal int CountParameters { get; private set; }

        internal object this[int indexParameter]
        {
            get
            {
                if (indexParameter < CountParameters)
                    return _parameters[indexParameter];
                else
                    return null;
            }
        }
    }
}
