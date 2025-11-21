namespace BudgetPlanner.API.Models
{
    public class SavingsGoal
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // Например: "На машину"
        public decimal TargetAmount { get; set; } // Сколько хотим накопить
        public decimal CurrentAmount { get; set; } // Сколько уже есть
        public DateTime? TargetDate { get; set; } // К какому числу (опционально)

        // Внешний ключ: Чья это цель
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
