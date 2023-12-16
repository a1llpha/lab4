using System;
using System.Collections.Generic;
using System.Linq;


public interface ISearchable
{
    List<Product> SearchByPriceRange(double minPrice, double maxPrice);
    List<Product> SearchByCategory(string category);
    List<Product> SearchByRating(int minRating);
}


public class Product
{
    public string Name { get; set; }
    public double Price { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public int Rating { get; set; }
}

public class User
{
    public string Username { get; set; }
    public string Password { get; set; }
    public List<Order> PurchaseHistory { get; set; }

    public User(string username, string password)
    {
        Username = username;
        Password = password;
        PurchaseHistory = new List<Order>();
    }
}

public class Order
{
    public List<Product> Products { get; set; }
    public List<int> Quantities { get; set; }
    public double TotalCost => Products.Zip(Quantities, (p, q) => p.Price * q).Sum();
    public string Status { get; set; }

    public Order()
    {
        Products = new List<Product>();
        Quantities = new List<int>();
        Status = "Pending";
    }
}


public class Store : ISearchable
{
    public List<User> Users { get; set; }
    public List<Product> Products { get; set; }
    public List<Order> Orders { get; set; }

    public Store()
    {
        Users = new List<User>();
        Products = new List<Product>();
        Orders = new List<Order>();
    }

    public List<Product> SearchByPriceRange(double minPrice, double maxPrice)
    {
        return Products.Where(p => p.Price >= minPrice && p.Price <= maxPrice).ToList();
    }

    public List<Product> SearchByCategory(string category)
    {
        return Products.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public List<Product> SearchByRating(int minRating)
    {
        return Products.Where(p => p.Rating >= minRating).ToList();
    }

    public void AddUser(User user)
    {
        Users.Add(user);
    }

    public void AddProduct(Product product)
    {
        Products.Add(product);
    }

    public void PlaceOrder(User user, List<Product> products, List<int> quantities)
    {
        var order = new Order { Products = products, Quantities = quantities };
        user.PurchaseHistory.Add(order);
        Orders.Add(order);
    }
}

class Program
{
    static void Main()
    {
        // Створення магазину
        Store store = new Store();

        // Додавання користувача
        User user1 = new User("user1", "password1");
        store.AddUser(user1);

        // Додавання товарів
        Product product1 = new Product { Name = "Product1", Price = 50.0, Category = "Category1", Rating = 4 };
        Product product2 = new Product { Name = "Product2", Price = 30.0, Category = "Category2", Rating = 5 };
        store.AddProduct(product1);
        store.AddProduct(product2);

        // Пошук товарів за різними критеріями
        List<Product> productsInCategory = store.SearchByCategory("Category1");
        List<Product> expensiveProducts = store.SearchByPriceRange(40.0, 100.0);
        List<Product> highlyRatedProducts = store.SearchByRating(4);

        // Вивід результатів пошуку
        Console.WriteLine("Products in Category1:");
        PrintProducts(productsInCategory);

        Console.WriteLine("\nExpensive Products:");
        PrintProducts(expensiveProducts);

        Console.WriteLine("\nHighly Rated Products:");
        PrintProducts(highlyRatedProducts);

        // Робота з замовленнями
        List<Product> orderProducts = new List<Product> { product1, product2 };
        List<int> quantities = new List<int> { 2, 1 };

        store.PlaceOrder(user1, orderProducts, quantities);

        // Вивід історії покупок користувача
        Console.WriteLine($"\nPurchase History for {user1.Username}:");
        foreach (var order in user1.PurchaseHistory)
        {
            Console.WriteLine($"Order Status: {order.Status}, Total Cost: {order.TotalCost}");
            PrintProducts(order.Products, order.Quantities);
            Console.WriteLine();
        }
    }

    static void PrintProducts(List<Product> products, List<int> quantities = null)
    {
        for (int i = 0; i < products.Count; i++)
        {
            Console.WriteLine($"Product: {products[i].Name}, Quantity: {quantities?[i]}, Price: {products[i].Price}");
        }
    }

}

