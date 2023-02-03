using Isi.ShoppingApp.Core.Entities;
using Isi.ShoppingApp.Data.Repositories;
using Isi.ShoppingApp.Domain.Services;
using Isi.Utility.Authentication;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Isi.ShoppingApp.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            //Isi.ShoppingApp.Domain.Services.Inventory inventory = new Domain.Services.Inventory();

            UserRepository repository = new UserRepository();

             bool exist = repository.UserExist("gluten");
             
             if (exist)
             {
                 Console.WriteLine("Created");
             }
             else
             {
                 Console.WriteLine("Not created");
             }
            
        } 
    }
}
