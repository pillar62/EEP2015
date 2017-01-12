using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace GLModule
{
    public interface IDbHelper
    {
        int ExecuteNonQuery(IDbCommand command);
    }

    /// <summary>
    /// for 新安
    /// </summary>
    public class DbHelperFactory: IDbHelper
    {
        public static IDbHelper CreateDbHelper()
        {
            //return new DbHelperFactory();
            return null;
        }

        #region IDbHelper Members

        public int ExecuteNonQuery(IDbCommand command)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
