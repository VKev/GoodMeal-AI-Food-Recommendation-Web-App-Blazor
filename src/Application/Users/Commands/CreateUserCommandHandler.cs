using Application.Abstractions.Messaging;
using Application.Abstractions.UnitOfWork;
using Application.Common;
using Application.Common.ResponseModel;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.Users.Commands
{
    public sealed record CreateUserCommand(
        string Name,
        string Email
    ) : ICommand;
    internal sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateUserCommandHandler> _logger;
        public CreateUserCommandHandler(ILogger<CreateUserCommandHandler> logger, IUserRepository userRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _userRepository = userRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating user - Name: {Name}, Email: {Email}", command.Name, command.Email);
            await _userRepository.AddAsync(_mapper.Map<User>(command), cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}