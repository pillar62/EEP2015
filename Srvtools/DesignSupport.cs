using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.Design;

namespace Srvtools
{
    public class DesignSupport
    {
        static public Component GetDesignRoot()
        {
            Component cRet = null;

            IDesignerHost myDH = (IDesignerHost)EditionDifference.ActiveWindowObject();
            if (myDH != null)
            {
                cRet = (Component)myDH.RootComponent;
            }
            return cRet;
        }
    }
}
