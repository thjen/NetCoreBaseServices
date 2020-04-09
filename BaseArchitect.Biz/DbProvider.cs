
using IDO.Edu.Entities;

namespace IDO.Edu.Biz
{
    public class DbProvider
    {
        //private static DbProvider _Instance;
        private static IdoContext _DbContext;
        private static DataProvider _ADO;

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

        public static IdoContext EF {
            get {
                if (_DbContext == null)
                {
                    _DbContext = new IdoContext();
                    return _DbContext;
                }
                else
                {
                    return _DbContext;
                }
            }
            private set { _DbContext = value; }
        }

        public static DataProvider ADO 
        {
            get {
                if (_ADO == null)
                {
                    _ADO = new DataProvider();
                    return _ADO;
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
