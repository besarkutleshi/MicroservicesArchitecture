using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Dtos;
using PlatformService.Interfaces;
using PlatformService.Models;

namespace PlatformService.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class PlatformController : ControllerBase {
        private readonly IPlatformRepo _platformRepo;
        private readonly IMapper _mapper;
        public PlatformController(IPlatformRepo platformRepo, IMapper mapper)
        {
            _platformRepo = platformRepo;
            _mapper = mapper;
        } 

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms(){

            var platforms = _platformRepo.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
        }

        [HttpPost]
        public ActionResult<PlatformCreateDto> CreatePlatform(PlatformCreateDto platformCreateDto) {
            _platformRepo.CreatePlatform(_mapper.Map<Platform>(platformCreateDto));
            if(_platformRepo.SaveChanges()){
                return Ok(platformCreateDto);
            }
            return BadRequest();
        }
    }
}