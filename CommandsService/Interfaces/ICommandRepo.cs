using System.Collections.Generic;
using CommandsService.Models;

namespace CommandsService.Interfaces {
    public interface ICommandRepo {
        bool SaveChanges();

        // Platforms
        IEnumerable<Platform> GetAallPlatforms();
        void CreatePlatform(Platform plat);
        bool PlatformExists(int platformId);

        // Commands
        IEnumerable<Command> GetCommandsForPlatform(int platformId);
        Command GetCommand(int platformId, int commandId);
        void CreateCommand(int platformId, Command command);

    }
}