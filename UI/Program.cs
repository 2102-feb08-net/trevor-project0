using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Models;
using DAL;

namespace UI
{
    public class Program
    {

        public static void Main(string[] args)
        {
            StoreApp storeapp = new StoreApp();
            storeapp.Run();
        }
    }
}
