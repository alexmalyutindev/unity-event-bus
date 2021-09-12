namespace EventBusSystem
{
	public interface IEventBusSubscriber
	{
		void HandleEvent<T>(T e);
	}
}
