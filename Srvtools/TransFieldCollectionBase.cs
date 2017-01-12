using System;
using System.Collections.Generic;
using System.Text;

namespace Srvtools
{
    public class TransFieldCollectionBase : InfoOwnerCollection
    {
        public TransFieldCollectionBase(Object aOwner, Type aItemType)
            : base(aOwner, typeof(TransFieldBase))
        {

        }
    }
}
