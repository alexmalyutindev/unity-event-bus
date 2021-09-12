using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace EventBusSystem.Tests
{
    public class EventBusTest
    {
        [Test(TestOf = typeof(EventBus))]
        public void TaskSubscription()
        {
            int massage = 10;
            var eventBus = new EventBus();

            Task.Delay(TimeSpan.FromSeconds(3f))
                .ContinueWith(_ => eventBus.Fire(massage));

            var tcs = new TaskCompletionSource<int>();
            using (eventBus.Subscribe<int>(result => tcs.SetResult(result)))
                Task.Run(async () => await tcs.Task).GetAwaiter().GetResult();

            Assert.IsTrue(tcs.Task.Result == massage);
        }
    }
}
