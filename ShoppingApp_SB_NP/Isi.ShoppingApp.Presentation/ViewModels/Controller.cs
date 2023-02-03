using Isi.ShoppingApp.Core.Entities;
using Isi.ShoppingApp.Domain.Services;
using Isi.Utility.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;




namespace Isi.ShoppingApp.Presentation.ViewModels
{
    public class Controller : Isi.Utility.ViewModels.ViewModel
    {
        public DelegateCommand IncreaseAmountToBuy { get; }
        public DelegateCommand DecreaseAmountToBuy { get; }
        public DelegateCommand OnBuyNowHandler { get; }
        public DelegateCommand OnAddToCartHandler { get; }

        UserService userService;

        public Product ProductToCart
        {
            get => productToCart;
            private set
            {
                productToCart = value;
                AmountToCart = AmountToBuy;
            }
        }

        public int AmountToCart { get; private set; }
        
        public int AmountToBuy
        {
            get => amountToBuy;
            set
            {
                if (value > 0)
                {
                    amountToBuy = value;
                    NotifyPropertyChanged(nameof(AmountToBuy));
                }
            }
        }
        public ObservableCollection<Product> Products
        {
            get => getInitialCollection();
        }

        public Product ProductSelected
        {
            get => productSelected;
            set
            {
                productSelected = value;
                NotifyPropertyChanged(nameof(ProductSelected));
                AmountToBuy = 1;
            }
        }


        //SHOPPING CART ========================================================================================================================================================
        public ObservableCollection<Product>ShoppingCart
        {
            get => shoppingCart;
            set
            {
                shoppingCart = value;
                NotifyPropertyChanged(nameof(ShoppingCart));
            }
        }

        public ProductService ProductService{ get => productService = new ProductService(); }

        //PRODUCT ITEMS
        
        private int amountToBuy;

        private Product productSelected;
        private ProductService productService;


        //INSTANCE OF USER
        public User User { get; set; }

        public string FullName { get; set; }
        
        private decimal balance;
        public decimal Balance
        {
            get => balance;
            set
            {
                if (value > 0)
                    balance = value;
            }
        }

        //USER CART
        private ObservableCollection<Product> shoppingCart;
        private Product productToCart;
        private int amountToCart;
        
        public Controller(User user)
        {
            userService = new UserService();
            ProductSelected = Products[0];
            IncreaseAmountToBuy = new DelegateCommand(Increment, CanIncrement);
            DecreaseAmountToBuy = new DelegateCommand(Decrement);
            OnBuyNowHandler = new DelegateCommand(BuyProduct, CanBuyProduct);
            OnAddToCartHandler = new DelegateCommand(OnAddToCart);


            ShoppingCart = new ObservableCollection<Product>();
            User = user;
            FullName = user.FullName;
            Balance = user.Balance;
            AmountToBuy = 1;
        }

        public ObservableCollection<Product> getInitialCollection()
        {
            return ProductService.GetDBProducts();
        }


        private void Increment(object _)
        {
            if (CanIncrement(_))
            {
                AmountToBuy++;
            }
        }

        private bool CanIncrement(object _)
        {
            return AmountToBuy < ProductSelected.Quantity & ProductSelected != null;
        }


        private void Decrement(object _)
        {
            AmountToBuy--;
        }

        private void BuyProduct(object _)
        {
            ProductSelected.Quantity -= AmountToBuy;
            UpdateProduct(ProductSelected);
            NotifyPropertyChanged(nameof(ProductSelected));
        }

        private bool CanBuyProduct(object _)
        {
            if (ProductSelected != null)
                return true; //TODO CHECK IF THE USER CAN AFFORD THE TRANSACTION (USER.BALANCE)
            return false; 
        }

        private void OnAddToCart(object _)
        {
            foreach (Product product in ShoppingCart)
            {
                //CASE 1: NEW PRODUCT: THEN ADD THE PRODUCT
                if(ProductSelected.Id != product.Id)
                {
                    if((ProductSelected.Quantity + product.Quantity) < ProductSelected.Quantity)
                    {
                        //p
                    }
                    else
                    {

                    }
                }

            }
            //CASE 2: PRODUCT IS ALREADY IN THE SHOPPING CART: ADD QUANTITIES , VERIFY!

            ProductToCart = ProductSelected;
            ProductToCart.Quantity = AmountToCart;
            ShoppingCart.Add(ProductToCart);
        }

        //IF EVERYTHING WORKS CHANGE PUBLIC => PRIVATE 
        public Product AddProduct(Product product)
        {
            Product newProduct = ProductService.AddProduct(product);
            NotifyPropertyChanged(nameof(Products));
            return newProduct;
        }

        //IF EVERYTHING WORKS CHANGE PUBLIC => PRIVATE 
        public bool RemoveProduct(Product product)
        {
            bool success = ProductService.RemoveProduct(product);
            NotifyPropertyChanged(nameof(Products));
            return success;
        }

        //IF EVERYTHING WORKS CHANGE PUBLIC => PRIVATE 
        public bool RemoveProduct(long id)
        {
            bool success = ProductService.RemoveProduct(id);
            NotifyPropertyChanged(nameof(Products));
            return success;
        }

        //IF EVERYTHING WORKS CHANGE PUBLIC => PRIVATE 
        public bool UpdateProduct(Product product)
        {
            bool success = productService.UpdateProduct(product);
            NotifyPropertyChanged(nameof(Products));
            return success;
        }

    }
}
