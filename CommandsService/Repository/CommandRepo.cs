using System;
using System.Collections.Generic;
using System.Linq;
using CommandsService.Data;
using CommandsService.Interfaces;
using CommandsService.Models;

namespace CommandsService.Repository {
    public class CommandRepo : ICommandRepo
    {
        private readonly AppDbContext _appDbConext;

        public CommandRepo(AppDbContext appDbContext)
        {
            _appDbConext = appDbContext;
        }

        public void CreateCommand(int platformId, Command command)
        {
            if(command == null){
                throw new ArgumentNullException(nameof(command));
            }
            command.PlatformId = platformId;
            _appDbConext.Commands.Add(command);
        }

        public void CreatePlatform(Platform plat)
        {
            if(plat == null){
                throw new ArgumentNullException(nameof(plat));
            }
            _appDbConext.Platforms.Add(plat);
        }

        public IEnumerable<Platform> GetAallPlatforms()
        {
            return _appDbConext.Platforms.ToList();
        }

        public Command GetCommand(int platformId, int commandId)
        {
            return _appDbConext.Commands.FirstOrDefault(c => c.Id == commandId && c.PlatformId == platformId);
        }

        public IEnumerable<Command> GetCommandsForPlatform(int platformId)
        {
            return _appDbConext.Commands.Where(c => c.PlatformId == platformId);
        }

        public bool PlatformExists(int platformId)
        {
            return _appDbConext.Platforms.Any(p => p.Id == platformId);
        }

        public bool SaveChanges()
        {
            return _appDbConext.SaveChanges() >= 0;
        }
    }
}