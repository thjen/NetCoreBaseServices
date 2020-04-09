using BaseArchitect.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BaseArchitect.DAL
{
    public class DAO
    {
        //private static DbProvider _Instance;
        private static BaseContext _DbContext;
        private static DataProvider _ADO;
        //private static readonly object obj1 = new object();
        //private static readonly object obj2 = new object();

        //private DbProvider() { }

        //public static DbProvider Instance {
        //    get {
        //        if (_Instance == null)
        //        {
        //            _Instance = new DbProvider();                    
        //            return _Instance;
        //        }
        //        else
        //        {
        //            return _Instance;
        //        }
        //    }
        //    private set { _Instance = value; }
        //}

        public static BaseContext EF {
            get {
                if (_DbContext == null)
                {
                    //lock (obj1)
                    //{
                    _DbContext = new BaseContext();
                    return _DbContext;
                    //}
                }
                else
                {
                    return _DbContext;
                }
            }
            private set { _DbContext = value; }
        }

        public static DataProvider ADO {
            get {
                if (_ADO == null)
                {
                    //lock (obj2)
                    //{
                    _ADO = new DataProvider();
                    return _ADO;
                    //}
                }
                else
                {
                    return _ADO;
                }
            }
            private set { _ADO = value; }
        }
    }
}
