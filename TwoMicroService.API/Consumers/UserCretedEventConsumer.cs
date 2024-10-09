using Bus.Shared;
using MassTransit;

namespace TwoMicroService.API.Consumers
{
    public class UserCretedEventConsumer : IConsumer<UserCreatedEvent>
    {
        public Task Consume(ConsumeContext<UserCreatedEvent> context)
        {
            var message=context.Message;

            //var message = message with { Email = "fd" }

            Console.WriteLine($"Kullanıcı kaydedildi {message.Email}");
            return Task.CompletedTask;
        }
    }
}
