/*  filename:       QueryColumnCollection.cs 
 *  version:        3.1
 *  lastedittime:   16:57 8/5/2006
 *  remark:         
 *  1. add new ver "DsForDD" into querycolumncollection     at 16:57 8/5/2006
 *  
 * 
 * 
 * 
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Data;

namespace Srvtools
{
    public class QueryColumnsCollection : InfoOwnerCollection
    {
        public QueryColumnsCollection(Object aOwner, Type aItemType)
            : base(aOwner, typeof(QueryColumns))
        {

        }
        public DataSet DsForDD = new DataSet();
        public new QueryColumns this[int index]
        {
			get
            {
                return (QueryColumns)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is QueryColumns)
                    {
                        //原来的Collection设置为0
                        ((QueryColumns)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((QueryColumns)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }
}
