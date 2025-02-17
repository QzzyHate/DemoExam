using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder();
builder.Services.AddDbContext<AppDbContext>(); //��������� �������� � �������� ������� ����������. ����������� ��������� � appsettings.json

var app = builder.Build();

app.MapGet("/", async (AppDbContext db) =>
{
    var orders = await db.Orders.ToListAsync(); //���������� �������� ������ ��������
    return Results.Ok(orders);
});

app.MapPost("/", async (Order order, AppDbContext db) =>
{ 
    db.Orders.Add(order); //��������� ������ � �� 
    await db.SaveChangesAsync(); //��������� ���������
    return Results.Created("Created: ",order); //���������� ��� ��������
});

app.Run();

public class Order
{
    public int Id { get; set; } // ��������� ����

    public int number { get; set; } //����� ������
    public DateOnly dateAdded { get; set; } //���� ����������
    public string device { get; set; } //����������� ������������
    public string problemType { get; set; } //��� ��������
    public string description { get; set; } //��������
    public string client { get; set; } //������
    public string status { get; set; } //������
}

public class AppDbContext : DbContext
{
    public DbSet<Order> Orders { get; set; } //������ ������� � ��������� Orders

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) //����������� �����������??
    {
        optionsBuilder.UseSqlite("Data Source=app.db");
    }
}