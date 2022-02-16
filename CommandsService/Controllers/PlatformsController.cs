using System;
using System.Collections.Generic;
using AutoMapper;
using CommandsService.Dtos;
using CommandsService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers {

    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformsController : ControllerBase {
        private readonly ICommandRepo _repo;
        private readonly IMapper _mapper;

        public PlatformsController(ICommandRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms() {
            Console.WriteLine(" --> Getting Platforms from CommandsService");
            var platforms = _repo.GetAallPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
        }

        [HttpPost]
        public ActionResult TestInBoundConnection(){
            Console.WriteLine("--> Inboud Post # Command Service");
            return Ok("Inbound test of from Platforms Controller");
        }
    }
}