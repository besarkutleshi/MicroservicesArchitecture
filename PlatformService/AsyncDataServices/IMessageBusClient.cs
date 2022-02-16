using PlatformService.Dtos.RabbitMQ;

namespace PlatformService.AsyncDataServices {
    public interface IMessageBusClient {
        void PublishNewPlatform(PlatformPublishedDto platformPublishedDto);
    }
}