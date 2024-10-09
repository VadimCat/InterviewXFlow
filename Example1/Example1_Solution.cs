[Serializable]
public class Health
{
    // Событие для уведомления о изменении здоровья
    public event Action<int> OnHealthChanged;

    // Значение здоровья
    private int _value;
    public int Value
    {
        get => _value;
        private set
        {
            _value = value;
            OnHealthChanged?.Invoke(_value);
        }
    }

    // Конструктор для задания начального здоровья
    public Health(int initialHealth)
    {
        Value = initialHealth;
    }

    // Метод для уменьшения здоровья
    public void TakeDamage(int damage)
    {
        if (damage < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(damage), "Damage cannot be negative");
        }
        Value = Math.Max(Value - damage, 0); // Здоровье не опускается ниже нуля
    }

    // Метод для восстановления здоровья
    public void Heal(int amount)
    {
        if (amount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Healing amount cannot be negative");
        }
        Value += amount; // Здесь можно добавить логику ограничения максимального здоровья
    }
}

[Serializable]
public class Damage
{
    public int Value { get; private set; }
    public Damage(int value)
    {
        value = value;
    }
    
    public void ApplyTo(Health health)
    {
        health.TakeDamage(Amount);
    }
}

public interface IGameFactory
{
    Player CreateHealth();
    Settings CreateDamage();
}

public class JsonGameFactory : IGameFactory
{
    private const string NewPlayerHealthPath = "NewPlayerHealth.json";
    private const string DamagePath = "Damage.json";

    public Health CreateHealth()
    {
        return Serializer.LoadFromFile<Health>(NewPlayerHealthPath);
    }

    public Damage CreateDamage()
    {
        return Serializer.LoadFromFile<Damage>(DamagePath);
    }
}

class Program
{
    private static IGameFactory _factory = new JsonGameFactory();
    private static Health playerHealth;

    public static void Main(string[] args)
    {
        // Создаем нового игрока.
        player = _factory.CreateHealth();
        // Ударяем игрока.
        var damage = _factory.CreateDamage();
        damage.ApplyTo(playerHealth);
    }
}