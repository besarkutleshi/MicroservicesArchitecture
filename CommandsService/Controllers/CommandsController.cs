using System.Collections.Generic;
using AutoMapper;
using CommandsService.Dtos;
using CommandsService.Interfaces;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers {
    
    [Route("api/c/platforms/{platformId}/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase {
        private ICommandRepo _commandRepo;
        private IMapper _mapper;

        public CommandsController(ICommandRepo commandRepo, IMapper mapper)
        {
            _commandRepo = commandRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId){
            System.Console.WriteLine($"--> Hit GetCommandsForPlatform : {platformId}");
            
            if(!_commandRepo.PlatformExists(platformId)){
                return NotFound();
            }

            var commands = _commandRepo.GetCommandsForPlatform(platformId);
            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
        }

        [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
        public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId){
            System.Console.WriteLine($" --> Hit GetCommandForPlatform {platformId} / {commandId}");
            if(!_commandRepo.PlatformExists(platformId)){
                return NotFound();
            }
            var command = _commandRepo.GetCommand(platformId,commandId);
            if(command == null) return NotFound();
            return Ok(_mapper.Map<CommandReadDto>(command));
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommandDtoForPlatform(int platformId, CommandCreateDto commandCreateDto){
            System.Console.WriteLine($"--> Hit CreateCommandForPlatform: {platformId}");
            if(!_commandRepo.PlatformExists(platformId)){
                System.Console.WriteLine("--> Platform not exists!");
                return NotFound();
            }
            var command = _mapper.Map<Command>(commandCreateDto);
            _commandRepo.CreateCommand(platformId,command);
            _commandRepo.SaveChanges();
            var commandReadDto = _mapper.Map<CommandReadDto>(command);
            return CreatedAtRoute(nameof(GetCommandForPlatform), new {platformId = platformId, commandId = commandReadDto.Id},commandReadDto);
        } 


    }
}