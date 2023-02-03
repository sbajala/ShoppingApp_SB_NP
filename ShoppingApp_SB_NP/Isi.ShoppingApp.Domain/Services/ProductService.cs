using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Isi.ShoppingApp.Core.Entities;
using Isi.ShoppingApp.Data.Repositories;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Isi.ShoppingApp.Domain.Services
{
    public class ProductService
    {
        public ProductRepository Repository{ get => repository = new ProductRepository();}
        private ProductRepository repository;


        //PUBLIC GETTER METHODS: ==================================================================================================================

        public ObservableCollection<Product> GetDBProducts()
        {
            ObservableCollection<Product> products = new ObservableCollection<Product>();
            List<Product> productList = Repository.GetProducts();

            foreach (Product product in productList)
            {
                products.Add(product);
            }
            return products;
        }

        public Product GetProduct(long id)
        { return Repository.GetProduct(id);}

        public bool ProductExists(long id)
        { return Repository.ProductExists(id);}

        public List<Product> GetAvailableProducts()
        { return Repository.GetAvailableProducts(); }

        public List<Product> GetUnavailableProducts()
        { return Repository.GetUnavailableProducts(); }



        //PUBLIC ACTION METHODS ==================================================================================================================
        public Product AddProduct(Product product)
        { return Repository.AddProduct(product); }

        public bool RemoveProduct(Product product)
        { return Repository.RemoveProduct(product);}

        public bool RemoveProduct(long id)
        { return Repository.RemoveProduct(id); }

        public bool UpdateProduct(Product product)
        { return Repository.UpdateProduct(product); }
    }
}
