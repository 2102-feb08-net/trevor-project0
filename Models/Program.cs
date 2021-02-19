using System;
using System.Collections.Generic;

namespace Models
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //StoreApp storeApp = new StoreApp();
            //storeApp.Run();

            SQLStoreApp storeApp = new SQLStoreApp();
            storeApp.Run();
        }
    }
}