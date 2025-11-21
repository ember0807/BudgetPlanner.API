using BudgetPlanner.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace BudgetPlanner.API.Data
{
    // AppDbContext должен наследоваться от DbContext
    public class AppDbContext : IdentityDbContext<User>
    {
        // Конструктор, который позволяет передавать настройки контекста
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSets — это коллекции, которые представляют таблицы в базе данных.
        // Каждое свойство соответствует одной из наших моделей.
        public DbSet<User> Users { get; set; } = default!;
        public DbSet<Category> Categories { get; set; } = default!;
        public DbSet<Transaction> Transactions { get; set; } = default!;
        public DbSet<SavingsGoal> SavingsGoals { get; set; } = default!;


        // Этот метод используется для настройки связей между таблицами
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка связи "один ко многим" (User -> Transactions)
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.User) // Транзакция имеет одного Пользователя
                .WithMany(u => u.Transactions) // Пользователь имеет много Транзакций
                .HasForeignKey(t => t.UserId) // Внешний ключ в таблице Transaction
                .OnDelete(DeleteBehavior.Cascade); // При удалении Пользователя удаляются все его Транзакции

            // Настройка связи "один ко многим" (Category -> Transactions)
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Category)
                .WithMany(c => c.Transactions)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); // При удалении Категории, Транзакции не удаляются (нужно будет сначала удалить Транзакции)

            // Настройка связи "один ко многим" (User -> SavingsGoals)
            modelBuilder.Entity<SavingsGoal>()
                .HasOne(sg => sg.User)
                .WithMany(u => u.SavingsGoals)
                .HasForeignKey(sg => sg.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
