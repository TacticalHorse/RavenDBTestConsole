using Raven.Client.Documents.Session;
using RavenDBTestConsole.Model;
using RavenDBTestConsole.Raven;

//CreateProduct("Apples", 9.10);
//CreateProduct("Tea", 10.22);
//CreateProduct("Banana", 8.99);
//CreateProduct("Coffee", 27.97);
//CreateProduct("Coffee", 1.20);
//GetProduct("products/1-A");
//GetAllProducts();
//GetProducts(1, 3);
//CreateCart("john@deo.com");
AddProductToCart("john@deo.com", "products/1-A",5);




static void CreateProduct(string name, double price)
{
    Product p = new Product();
    p.Name = name;
    p.Price = price;

    using (var session = DocumentStoreHolder.Store.OpenSession())
    {
        session.Store(p); 
        session.SaveChanges();
    }
    Console.WriteLine(p.Id);
}

static void GetProduct(string id)
{
    using (var session = DocumentStoreHolder.Store.OpenSession())
    {
        Product p = session.Load<Product>(id);
        Console.WriteLine($"Product: {p.Name}  \t\t price: {p.Price}");
    }
}

static void GetAllProducts()
{
    using (var session = DocumentStoreHolder.Store.OpenSession())
    {
        //foreach (var item in session.Query<Product>())
        foreach (var item in session.Query<Product>(collectionName: "products"))
        {
            Console.WriteLine($"Product: {item.Name}  \t\t price: {item.Price}");
        } 
    }
}

static void GetProducts(int pageNdx, int pageSize)
{
    int skip = (pageNdx-1) * pageSize, 
        take = pageSize;
    using (var session = DocumentStoreHolder.Store.OpenSession())
    {
        var items = session.Query<Product>(collectionName: "products")
            .Statistics(out QueryStatistics stats)
            .Skip(skip)
            .Take(take)
            .ToList();

        Console.WriteLine($"Showing results {skip+1} to {skip + pageSize} of {stats.TotalResults}");
        foreach (var item in items)
        {
            Console.WriteLine($"Product: {item.Name}  \t\t price: {item.Price}");
        }

        Console.WriteLine($"This was prodicd in {stats.DurationInMs}  ms");
    }
}

static  void CreateCart(string  customer)
{
    Cart cart = new Cart();
    cart.Customer = customer;
    using (var session = DocumentStoreHolder.Store.OpenSession())
    {
        session.Store(cart);
        session.SaveChanges();
    }
}

static  void AddProductToCart(string customer, string productId,  int quantity)
{
    using (var session = DocumentStoreHolder.Store.OpenSession())
    {
        Cart cart = session.Query<Cart>().Single(x => x.Customer == customer);
        Product p = session.Load<Product>(productId);

        cart.Lines.Add(new CartLine
        {
            ProductName = p.Name,
            ProductPrice = p.Price,
            Quantity = quantity

        });

        session.SaveChanges();
    }
}