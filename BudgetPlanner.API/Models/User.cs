using Microsoft.AspNetCore.Identity;

namespace BudgetPlanner.API.Models
{
    public class User:IdentityUser
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty; // Мы не храним пароли в открытом виде!

        // Связь: Один пользователь может иметь много транзакций и категорий
        // Эти списки нужны для EF Core, чтобы понимать связи
        public List<Transaction> Transactions { get; set; } = new();
        public List<Category> Categories { get; set; } = new();
        public List<SavingsGoal> SavingsGoals { get; set; } = new();
    }
}
