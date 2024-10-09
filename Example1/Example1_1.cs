//В зависимости добавить сериализацию
//Player - плохое название класса, т.к. это просто Health
public class Player
{
    // Если переименовать Health в Player, то следует переименовать в Value/Amount
    public int Health {
        get; private set;
    }
    
    public Player(int health) {
        Health = health;
    }

    // Не инкапсулирует работу с состоянием Health
    public void SetHealth(int value) {
        Health = value;
    }
}

class Program
{
    // Следует создать абсрактную фабрику для создания комонентов
    private const int NewPayerHealth = 100;
    private const int Damage = 10;
    
    // Это, конечно,  не продакшен код, но зачем тут protected не понятно
    protected static Player player;

    public static void Main(string[] args)
    {
        // Создаем нового игрока.
        player = new Player(NewPayerHealth);
        // Ударяем игрока.
        // Нарушение инкапсуляции: мы достаем свойство из объекта и вручную преобразуем вместо того,
        // чтобы делегировать эту работу объекту
        player.SetHealth(player.Health - Damage);
    }
}
