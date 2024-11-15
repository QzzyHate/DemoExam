using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder();
builder.Services.AddDbContext<AppDbContext>(); //Добавляем контекст в качестве сервиса приложения. Подключение настроено в appsettings.json

var app = builder.Build();

app.MapGet("/", async (AppDbContext db) =>
{
    var orders = await db.Orders.ToListAsync(); //Асинхронно получаем список объектов
    return Results.Ok(orders);
});

app.MapPost("/", async (Order order, AppDbContext db) =>
{ 
    db.Orders.Add(order); //Добавляем заявку в бд 
    await db.SaveChangesAsync(); //Сохраняем изменения
    return Results.Created("Created: ",order); //Показываем что добавили
});

app.Run();

public class Order
{
    public int Id { get; set; } // Первичный ключ

    public int number { get; set; } //Номер заявки
    public DateOnly dateAdded { get; set; } //Дата добавления
    public string device { get; set; } //Неисправное оборудование
    public string problemType { get; set; } //Тип проблемы
    public string description { get; set; } //Описание
    public string client { get; set; } //Клиент
    public string status { get; set; } //Статус
}

public class AppDbContext : DbContext
{
    public DbSet<Order> Orders { get; set; } //Создаём таблицу с названием Orders

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) //Настраивает подключение??
    {
        optionsBuilder.UseSqlite("Data Source=app.db");
    }
}