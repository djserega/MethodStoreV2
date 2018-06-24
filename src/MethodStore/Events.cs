using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodStore
{
    internal delegate void FrameGoBackEvent();

    internal class FrameGoBackEvents : EventArgs
    {
        internal event FrameGoBackEvent FrameGoBackEvent;

        internal void EvokeFrameGoBack()
        {
            if (FrameGoBackEvent == null)
                return;

            FrameGoBackEvent();
        }

    }
}
