using Dapper;
using Dapper.Contrib.Extensions;
using DapperExample.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace DapperExample
{
    class Program
    {
        static void Main(string[] args)
        {

            //var productRepository = new ProductRepository();
            //productRepository.Add(new Entities.Product 
            //{
            //    Name="Dapper",
            //    Price=123,
            //    Stock=3
            //});

            var categoryRepository = new CategoryRepository();
            categoryRepository.Add(new Entities.Category
            {
                Name = "Software"
            });

            Console.WriteLine("İşlem Tamamlandı");
        }
    }
}
