using Dapper;
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
            //SQL Client Instance 'ı alındı
            var con = new SqlConnection(); //MSSQL Connection
            con.ConnectionString = @"Server=(localdb)\MSSQLLocalDB;Database=DapperDb;Trusted_Connection=true;";


            ////Dapper Execute Kullanarak parametresiz data ekledik
            //con.Execute("insert into Products values('Iphone',4000,30)"); 

            ////Tekli Data Ekleme - Parametreli data ekledik
            //con.Execute("insert into Products values(@name,@price,@stock)", new
            //{
            //    name = "Playstation",
            //    price = 4500,
            //    stock = 50
            //});

            ////Çoklu data ekleme
            //con.Execute("insert into Products values(@name,@price,@stock)", new[]
            //{
            //    new {name = "Playstation 1",price = 4500,stock = 50},
            //    new {name = "Playstation 5",price = 9500,stock = 5}
            //});

            ////Data Güncelleme
            //con.Execute("update Products set name='Telefon',stock=15,price=200 where id=3");

            ////Tekli Data Güncelleme - Parametreli
            //con.Execute("update Products set name=@name,price=@price,stock=@stock where id=3", new
            //{
            //    name = "DVD Player",
            //    price = 4500,
            //    stock = 50
            //});

            ////Çoklu data Güncelleme
            //con.Execute("update Products set name=@name,price=@price,stock=@stock where id=@id)", new[]
            //{
            //    new {name = "Playstation 1",price = 4500,stock = 50,id=3},
            //    new {name = "Playstation 5",price = 9500,stock = 5,id=4}
            //});

            ////Parametresiz Silme
            //con.Execute("delete from Products where id=1");

            ////Parametreli Silme
            //con.Execute("delete from Products where id=@id",new {id=3});


            ////İlk Kaydın ilk sütununu bize geri döndürür
            //var id = con.ExecuteScalar("select * from Products");

            ////İlk Kaydın istenen sütununu bize geri döndürür,generic olarakta tanımlayabiliriz
            //var name = con.ExecuteScalar<string>("select name from Products");

            ////Filtre sonucu gelen ilk datanın gerekli sütununu geri döndürdük
            //var getName = con.ExecuteScalar<string>("select name from Products where id=@id", new { id = 3 });

            ////IEnumerable<dynamic> dönecektir.dynamic veri tipi JS de ki var keyword u gibi değişken tipteki dataları atamamızı sağlar
            //var products = con.Query("select * from Products");

            ////Tip güvenli olarak ürünleri aldık --> IEnumarable<Product> 
            //var typeSafeProducts = con.Query<Product>("select * from Products");

            ////İlk datayı aldık.Birden fazla data gelirse şayet yine ilkini bize verecektir
            //var typeSafeProductFirst=con.QueryFirst<Product>("select * from Products");

            ////Gerekli şartları sağlayan 1 adet datayı alırız, yoksa hata alırız
            //var typeSafeProductSingle = con.QuerySingle<Product>("select * from Products where id=1");

            ////İlk datayı aldık.Birden fazla data gelirse şayet yine ilkini bize verecektir,herhangi bir data yoksa hata fırlatmaz
            //var typeSafeProductFirstDefault = con.QueryFirstOrDefault<Product>("select * from Products");

            ////Gerekli şartları sağlayan 1 adet datayı alırız, yoksa hata alırız,herhangi bir data yoksa hata fırlatmaz
            //var typeSafeProductSingleDefault = con.QuerySingleOrDefault<Product>("select * from Products where id=1");


            //JOIN İşlemi
            var productDictionary = new Dictionary<int, Product>();

            var products = con.Query<Product, ProductCategory, Product>(@"select * from Products inner join ProductCategory on Products.Id=ProductCategory.ProductId", (product, productCategory) =>
              {
                  //Bu kısımda yaptığımız işlem ürünlere ait kategorilerin ProductCategories listesine eklensin ve bir çok kategoriye ait ürün birden fazla gelmesin.
                  Product otherProduct;
                  if (!productDictionary.TryGetValue(product.Id, out otherProduct)) //Id daha önce eklenmemiş ise
                  {
                      otherProduct = product;
                      otherProduct.ProductCategories = new List<ProductCategory>();
                      productDictionary.Add(product.Id, product);
                  }
                  otherProduct.ProductCategories.Add(productCategory);
                  return otherProduct;
              }).Distinct().ToList();


            //Transaction İşlemi
            //con.Open();
            //var transaction = con.BeginTransaction();
            //con.Execute("update BankAccount set Money=50 where id=1", transaction: transaction);
            //con.Execute("update BankAccounts set Money=15050 where id=3", transaction: transaction);
            //transaction.Commit();


            //Store Procedure
            //Şeklinde yazdığımız store procedure leri kullanacağız
            //create proc sp_addProduct
            //@name nvarchar(50),
            //@price decimal(18, 3),
            //@stock int
            //as
            //begin
            //insert into Products values(@name, @price, @stock)
            //end
            con.Execute("sp_addProduct",new 
            {
                name="Kitap",
                price=30,
                stock=22
            },commandType:System.Data.CommandType.StoredProcedure);  //Databasemizde yer alan store procedure ismini vererek kaydetme işlemini gerçekleştirdik.Parametre olarakta bunun store procedure olduğunu bunu sql komutu olarak algılamaması gerektiğini söyledik

            con.Execute("sp_updateProduct", new
            {
                name = "Kalem",
                price = 25,
                stock = 22,
                id=11
            }, commandType: System.Data.CommandType.StoredProcedure);

            con.Execute("sp_deleteProduct", new
            {
                id = 11
            }, commandType: System.Data.CommandType.StoredProcedure);


            var productsSP=con.Query<Product>("sp_getProducts",commandType: System.Data.CommandType.StoredProcedure);



            Console.WriteLine("İşlem Tamamlandı");
        }
    }

    class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }

        public List<ProductCategory> ProductCategories { get; set; }
    }

    class ProductCategory
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }


        public int ProductId { get; set; }
        public Product Product { get; set; }
    }

    class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<ProductCategory> ProductCategories { get; set; }
    }
}
