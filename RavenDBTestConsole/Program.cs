using RavenDBTestConsole.Model;
using RavenDBTestConsole.Raven;

//CreateProduct("Apples", 9.10);
//CreateProduct("Tea", 10.22);
//CreateProduct("Banana", 8.99);
//CreateProduct("Coffee", 27.97);
//CreateProduct("Coffee", 1.20);
GetProduct("products/1-A");


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