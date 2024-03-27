﻿using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MusicService.Domain.Constants;
using MusicService.Domain.Entities;
using MusicService.Domain.Interfaces;
using MusicService.Grpc;

namespace MusicService.API.GrpcServices
{
    public class MusicGrpcService : MusicUserService.MusicUserServiceBase
    {
        private readonly ILogger<MusicGrpcService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MusicGrpcService(ILogger<MusicGrpcService> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public override async Task<Empty> AddUser(AddUserRequest request, ServerCallContext context)
        {
            if (await _unitOfWork.Users.GetByIdAsync(request.Id) != null)
            {
                throw new RpcException(new Status(StatusCode.AlreadyExists, ExceptionMessages.UserAlreadyExists));
            }

            var user = _mapper.Map<User>(request);

            await _unitOfWork.Users.CreateAsync(user, context.CancellationToken);

            await _unitOfWork.CommitAsync(context.CancellationToken);

            _logger.LogInformation("User {userId} is received from Identity and persisted.", request.Id);

            return new Empty();
        }
    }
}
