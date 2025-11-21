namespace BudgetPlanner.API.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.UtcNow;

        // Тип транзакции дублируем для удобства фильтрации (Income/Expense)
        public string Type { get; set; } = "Expense";

        // Внешний ключ: К какой категории относится
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        // Внешний ключ: Чья это транзакция
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
