using System.Collections.Generic;
using System.Net.Http;
using AutoMapper;
using CommandsService.Models;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using PlatformService;

namespace CommandsService.SyncDataServices.Grpc {
    public class PlatformDataClient : IPlatformDataClient
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public PlatformDataClient(IConfiguration configuration, IMapper mapper, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _mapper = mapper;
            _env = env;
        }

        public IEnumerable<Platform> ReturnAllPlatforms()
        {
            System.Console.WriteLine($"--> Calling GRPC Service {_configuration["GrpcPlatform"]}");
            GrpcChannel channel = null;
            if(_env.IsDevelopment()){
                var httpHandler = new HttpClientHandler();
                httpHandler.ServerCertificateCustomValidationCallback = 
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                channel = GrpcChannel.ForAddress(_configuration["GrpcPlatform"], new GrpcChannelOptions { HttpHandler = httpHandler });
            }
            else {
                channel = GrpcChannel.ForAddress(_configuration["GrpcPlatform"]);
            }
            var client = new GrpcPlatform.GrpcPlatformClient(channel);
            var request = new GetAllRequest();

            try
            {
                var reply = client.GetAllPlatforms(request);
                return _mapper.Map<IEnumerable<Platform>>(reply.Platform);
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine($"--> Could not call GRPC Server: {ex.Message}");
                return null;
            }
        }
    }
}