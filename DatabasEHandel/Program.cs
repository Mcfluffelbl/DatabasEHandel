using DatabasEHandel;
using DatabasEHandel.Models;
using DatabasEHandel.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using static System.Reflection.Metadata.BlobBuilder;
using static System.Runtime.InteropServices.JavaScript.JSType;

Console.WriteLine("Hello, World!");

// Intro
Console.WriteLine("DB: " + Path.Combine(AppContext.BaseDirectory, "shop.db"));
Console.WriteLine("Hello, and welcome to your´e E-Store!");

// Secure DB + migrations + seed
using (var db = new ShopContext())
{
    // Migrate Async: Creates database if it does not exist
    // Go only if there are no categories already since before
    await db.Database.MigrateAsync();

    // Easy seeding of the database
    // Only if no customers exist since before
    if (!await db.Customers.AnyAsync())
    {
        db.Customers.AddRange(
            new Customer { CustomerId = 1, Name = "Harry", Email = "Harry.Flyg@Code.com", City = "Italy" },
            new Customer { CustomerId = 2, Name = "Sten", Email = "Sten.Bergman@Telia.com", City = "Norway" }
        );
        await db.SaveChangesAsync();
        Console.WriteLine("Seeded db!");
    }

    if (!await db.Products.AnyAsync())
    {
        db.Products.AddRange(
        new Product { ProductId = 1, ProductName = "Skruv", Price = 10, StockQuantity = 1 },
        new Product { ProductId = 2, ProductName = "Spik", Price = 25, StockQuantity = 23 }
        );
        await db.SaveChangesAsync();
        Console.WriteLine("Seeded db!");
    }

    if (!await db.Orders.AnyAsync())
    {
        db.Orders.AddRange(
        new Order { OrderId = 1, OrderDate = DateTime.Today.AddDays(-3), CustomerId = 1, Status = Status.Paid, TotalAmount = 10000 },
        new Order { OrderId = 2, OrderDate = DateTime.Today.AddDays(-10), CustomerId = 2, Status = Status.Pending, TotalAmount = 200 }
        );
        await db.SaveChangesAsync();
        Console.WriteLine("Seeded db!");
    }
}

while (true)
{
    Console.WriteLine("\nMenu:");
    Console.WriteLine("1. List Customers");
    Console.WriteLine("2. List Products");
    Console.WriteLine("3. List Orders");
    Console.WriteLine("0. Exit");
    Console.Write("Choose an option: ");
    var choice = Console.ReadLine();

    // Jump out of loop if empty input
    if (string.IsNullOrEmpty(choice))
    {
        continue;
    }

    // End loop and exit program
    if (choice.Equals("0", StringComparison.OrdinalIgnoreCase))
    {
        break;
    }

    // Split input into parts
    var parts = choice.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    var cmd = parts[0].ToLowerInvariant();
    // Easy switch between commands
    switch (cmd)
    {
        case "1":
            await ListCustomers();
            break;
        case "2":
            await ListProducts();
            break;
        case "3":
            await ListOrders();
            break;
        default:
            Console.WriteLine("Invalid choice. Please try again.");
            break;
    }
}
return;

static async Task ListCustomers()
{
    using var db = new ShopContext();
    var customers = await db.Customers.ToListAsync();
    Console.WriteLine("\nCustomers:");
    foreach (var customer in customers)
    {
        Console.WriteLine($"ID: {customer.CustomerId}, Name: {customer.Name}, Email: {customer.Email}, City: {customer.City}");
    }
}

static async Task ListProducts()
{
    using var db = new ShopContext();
    var products = await db.Products.ToListAsync();
    Console.WriteLine("\nProducts:");
    foreach (var product in products)
    {
        Console.WriteLine($"ID: {product.ProductId}, Name: {product.ProductName}, Price: {product.Price}, Stock: {product.StockQuantity}");
    }
}

static async Task ListOrders()
{
    using var db = new ShopContext();
    var orders = await db.Orders
        .Include(o => o.Customer)
        .ToListAsync();
    Console.WriteLine("\nOrders:");
    foreach (var order in orders)
    {
        Console.WriteLine($"ID: {order.OrderId}, Customer: {order.Customer!.Name}, Date: {order.OrderDate.ToShortDateString()}, Status: {order.Status}, Total: {order.TotalAmount}");
    }
}