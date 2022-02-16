using System.Text.Json;
using AutoMapper;
using CommandsService.Dtos.RabbitMQDtos;
using CommandsService.Interfaces;
using CommandsService.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CommandsService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }
        public void ProcessEvent(string message)
        {
            var eventType = DeterminieEvent(message);
            switch(eventType){
                case EventType.PlatformPublished:
                    AddPlatform(message);
                    break;
                default:
                    break;
            }
        }

        private EventType DeterminieEvent(string notificationMessage) {
            System.Console.WriteLine($"--> Determing Event: {notificationMessage}");
            var eventType  = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);
            switch(eventType.Event){
                case "Platform_Published":
                    System.Console.WriteLine("--> Platform_Published Event Detected");
                    return EventType.PlatformPublished;
                default:
                    System.Console.WriteLine("--> Could not detect event type");
                    return EventType.Undetermined;
            }

        }

        private void AddPlatform(string platformPublishedMessage){
            using(var scope = _scopeFactory.CreateScope()){
                var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();
                var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublishedMessage);
                try
                {
                    var plat = _mapper.Map<Platform>(platformPublishedDto);
                    if(repo.ExternalPlatformExists(plat.Id)){
                        System.Console.WriteLine($"--> Platform already exists...");
                    } else{
                        repo.CreatePlatform(plat);
                        repo.SaveChanges();
                        System.Console.WriteLine($"--> Platform added!");
                    }
                }   
                catch (System.Exception ex)
                {
                    System.Console.WriteLine($"--> Could not add Platform to DB: {ex.Message}");
                }
            }
        }
    }

    enum EventType
    {
        PlatformPublished,
        Undetermined
    }
}