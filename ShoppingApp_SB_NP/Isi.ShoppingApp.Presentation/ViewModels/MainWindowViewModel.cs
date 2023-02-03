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
using System.Windows;
using System.Windows.Controls;




namespace Isi.ShoppingApp.Presentation.ViewModels
{
    public class MainWindowViewModel : Isi.Utility.ViewModels.ViewModel
    {
        public DelegateCommand IncreaseAmountToBuy { get; }
        public DelegateCommand DecreaseAmountToBuy { get; }
        public DelegateCommand OnBuyNowHandler { get; }
        public DelegateCommand OnAddToCartHandler { get; }
        
        public DelegateCommand OnEmptyCartButton { get; }
        public DelegateCommand OnCheckoutButton { get; }
        public DelegateCommand OnAddNewProduct { get; }


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
                RawTotal = SumPricesInCart();
                NotifyPropertyChanged(nameof(ShoppingCart));
            }
        }

        public decimal RawTotal 
        {
            get => rawTotal;
            set
            {
                rawTotal = value;
                Tax = RawTotal * 0.15m;
                TotalPrice = rawTotal + Tax;
            }
        }
        private decimal rawTotal;

        public decimal TotalPrice
        {
            get => Math.Round(totalPrice, 2);
            set
            {
                totalPrice = value;
                NotifyPropertyChanged(nameof(TotalPrice));
            }
        }
        private decimal totalPrice;

        public decimal Tax
        {
            get => Math.Round(tax, 2);
            set
            {
                tax = value;
                NotifyPropertyChanged(nameof(Tax));
            }
        }
        private decimal tax;


        //ADMIN PANEL ==============================================================================================================
        public string CreateName { get; set; }
        public string CreateCategory { get; set; }
        public string CreateDescription { get; set; }
        public decimal CreatePrice { get; set; }
        public int CreateQuantity { get; set; }
        public decimal? CreateDiscount { get; set; }

        public ProductService ProductService{ get => productService = new ProductService(); }

        //PRODUCT ITEMS
        
        private int amountToBuy;

        private Product productSelected;
        private ProductService productService;


        //INSTANCE OF USER
        public string User { get; set; }


        //USER CART
        private ObservableCollection<Product> shoppingCart;

        
        public MainWindowViewModel()
        {
            ProductSelected = Products[0];
            IncreaseAmountToBuy = new DelegateCommand(Increment, CanIncrement);
            DecreaseAmountToBuy = new DelegateCommand(Decrement);
            OnBuyNowHandler = new DelegateCommand(BuyProduct, CanBuyProduct);
            OnAddToCartHandler = new DelegateCommand(OnAddToCart);

            OnEmptyCartButton = new DelegateCommand(EmptyCart);
            OnCheckoutButton = new DelegateCommand(Checkout);
            OnAddNewProduct = new DelegateCommand(AddProductDB);


            ShoppingCart = new ObservableCollection<Product>();
            User = "Nicolas Perdomo";

            Tax = 0m;
            TotalPrice = 0m;
            AmountToBuy = 1;
        }

        private void AddProductDB(object obj)
        {
            ProductService.AddProduct(new Product(CreateName, CreateCategory, CreateDescription, CreatePrice, CreateQuantity, CreateDiscount));
            NotifyPropertyChanged(nameof(Products));
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
            if (IsProductSelectedNotNull())
                return true; //TODO CHECK IF THE USER CAN AFFORD THE TRANSACTION (USER.BALANCE)
            return false;
        }

        private bool IsProductSelectedNotNull()
        {
            return ProductSelected != null;
        }

        private void OnAddToCart(object _)
        {
            if(IsProductSelectedNotNull())
            {
                if (ShoppingCart.Count == 0)
                    AddNewProduct();
                else
                {
                    int indexInCart = FindProductIndexInCart(ProductSelected);
                    if (indexInCart >= 0) { AddToDuplicateProduct(ProductSelected, indexInCart); }
                    else { AddNewProduct(); }
                }
            }
            //CASE 2: PRODUCT IS ALREADY IN THE SHOPPING CART: ADD QUANTITIES , VERIFY!
        }

        private int FindProductIndexInCart(Product productSelected)
        {
            for (int i=0; i<ShoppingCart.Count; i++)
            {
                if (productSelected.Id == ShoppingCart[i].Id)
                {
                    return i;

                }
            }
            return -1;
        }

        public int FindProductIndexInCart(long Id)
        {
            for (int i = 0; i < ShoppingCart.Count; i++)
            {
                if (Id == ShoppingCart[i].Id)
                {
                    return i;

                }
            }
            return -1;
        }

        private void AddNewProduct()
        {
            Product productToCart = new Product(ProductSelected.Id, ProductSelected.Name, ProductSelected.Category, ProductSelected.Description, ProductSelected.Price, ProductSelected.Quantity, ProductSelected.PercentageDiscount);
            productToCart.Quantity = AmountToBuy;

            ShoppingCart.Add(productToCart);
            RawTotal += productToCart.FinalPrice;
        }

        private void AddToDuplicateProduct(Product productSelected, int indexInCart)
        {
            if (ShoppingCart[indexInCart].Quantity + AmountToBuy > ProductSelected.Quantity)
            {
                ShoppingCart[indexInCart].Quantity = ProductSelected.Quantity;
            }

            else
            {
                ShoppingCart[indexInCart].Quantity += AmountToBuy;
            }
            ShoppingCart = UpdateShoppingCart();
        }

        public ObservableCollection<Product> UpdateShoppingCart()
        {
            ObservableCollection<Product> newCart = new ObservableCollection<Product>();
            foreach (Product product in ShoppingCart)
            {
                if(product.Quantity > 0)
                    newCart.Add(product);
            }
            return newCart;
        }

        private void EmptyCart(object _)
        {
            ShoppingCart = new ObservableCollection<Product>();
            ShoppingCart = UpdateShoppingCart();
        }

        private void Checkout(object _)
        {
            if(true) //IF USER CAN AFFORD THE PURCHASE!
            {
                for (int i=0; i<ShoppingCart.Count; i++)
                {
                    Product productToCheckout = ProductService.GetProduct(ShoppingCart[i].Id);      //get real product.
                    productToCheckout.Quantity -= ShoppingCart[i].Quantity;                         //update changes in real product.
                    ProductService.UpdateProduct(productToCheckout);                                //push changes.
                    NotifyPropertyChanged(nameof(Products));

                    RawTotal = 0m;
                }

                ShoppingCart = new ObservableCollection<Product>();
                ShoppingCart = UpdateShoppingCart();
            }

            else
            {
                MessageBox.Show("INSUFFICIENT FUNDS", "Invalid transaction");
            }
        }

        private decimal SumPricesInCart()
        {
            decimal sum = 0;
            foreach (Product product in ShoppingCart)
            {
                sum += product.FinalPrice * product.Quantity;
            }
            return sum;
        }

        private Product AddProduct(Product product)
        {
            Product newProduct = ProductService.AddProduct(product);
            NotifyPropertyChanged(nameof(Products));
            return newProduct;
        }

        private bool RemoveProduct(Product product)
        {
            bool success = ProductService.RemoveProduct(product);
            NotifyPropertyChanged(nameof(Products));
            return success;
        }

        private bool RemoveProduct(long id)
        {
            bool success = ProductService.RemoveProduct(id);
            NotifyPropertyChanged(nameof(Products));
            return success;
        }

        public bool UpdateProduct(Product product)
        {
            bool success = productService.UpdateProduct(product);
            NotifyPropertyChanged(nameof(Products));
            return success;
        }
    }
}
