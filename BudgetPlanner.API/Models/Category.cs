namespace BudgetPlanner.API.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Тип: "Income" (Доход) или "Expense" (Расход)
        public string Type { get; set; } = "Expense";

        // Связь с пользователем (может быть null, если это общая категория для всех)
        public int? UserId { get; set; }
        public User? User { get; set; }

        // Связь: Одна категория может использоваться во многих транзакциях
        public List<Transaction> Transactions { get; set; } = new();
    }
}
