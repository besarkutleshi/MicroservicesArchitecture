using System.Collections.Generic;
using PlatformService.Models;

namespace PlatformService.Interfaces {
    public interface IPlatformRepo { 
        bool SaveChanges();
        IEnumerable<Platform> GetAllPlatforms();
        Platform GetPlatformById(int id);
        void CreatePlatform(Platform platform);
    }
}