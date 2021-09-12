namespace Core.Systems.Events
{
	public interface IEventBusSubscriber
	{
		void HandleEvent<T>(T e);
	}
}
