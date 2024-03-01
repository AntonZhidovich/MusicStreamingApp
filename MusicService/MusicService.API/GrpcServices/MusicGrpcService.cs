using AutoMapper;
using Grpc.Core;
using MusicService.Application.GrpcServers;
using MusicService.Domain.Constants;
using MusicService.Domain.Entities;
using MusicService.Domain.Interfaces;

namespace MusicService.API.GrpcServices
{
    public class MusicGrpcService : MusicUserService.MusicUserServiceBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MusicGrpcService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public override async Task<AddUserResponse> AddUser(AddUserRequest request, ServerCallContext context)
        {
            if (await _unitOfWork.Users.GetByIdAsync(request.Id) != null)
            {
                throw new RpcException(new Status(StatusCode.AlreadyExists, ExceptionMessages.UserAlreadyExists));
            }

            var user = _mapper.Map<User>(request);

            await _unitOfWork.Users.CreateAsync(user, context.CancellationToken);

            await _unitOfWork.CommitAsync(context.CancellationToken);

            return new AddUserResponse();
        }
    }
}
