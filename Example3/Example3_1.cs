public class Player
{
	private List<Vector2> activeWalkPath = null;
	private bool isMoving;
	private Vector2 currentPosition;
	private Player currentEnemy;

	public void Update()
	{
		//так же стоит проверять изменилась ли позиция врага, если да, то обновить путь(возможно это в части <много строк кода по созданию пути>)
		if (currentEnemy != null)
		{
			UpdatePathToEnemy();
		}
	}
	
	private void UpdatePathToEnemy()
	{
		//стоит вынести в отдельный метод, так же неизвестно что нам вернут, и нет обработки случай с пустым путем
		activeWalkPath = <много строк кода по созданию пути>;
		if (activeWalkPath == null)
		{
			isMoving = false;
			currentEnemy = null;
		}
		else
		{
			isMoving = true;
		}
	}
}
