public class ExtPlayer : Player
{
	// Не очень люблю такие делегаты, т.к. нужно всегда смотреть внутрь, чтобы понять, что они делают.
	public delegate void HealthChangedDelegate(int oldHealth, int newHealth);

	public event HealthChangedDelegate HealthChanged;
}

//Теперь понятно почему в Example1 есть Protected :D
class ExtProgram : Program
{
	// Виджет, отображающий игроку здоровье.
	private static TextView healthView = new TextView();

	public static void ExtMain(string[] args)
	{
		// Вызов кода по созданию игрока.
		Main(args);

		// Ручное обновление виджета можно забыть написать
		// Как и во втором примере создать объект связывающий виджет и модель (HealthPresenter)
		
		healthView.Text = player.Health.ToString();
		player.HealthChanged += OnPlayerHealthChanged;

		// Ударяем игрока.
		HitPlayer();
	}

	private static void OnPlayerHealthChanged(int oldHealth, int newHealth)
	{
		healthView.Text = newHealth.ToString();
		// Логику отображения цвета следует вынести в объект HealthView
		if (newHealth - oldHealth < -10) {
			healthView.Color = Color.Red;
		} else {
			healthView.Color = Color.White;
		}
	}
}
