using Application.Abstractions.UnitOfWork;
using Application.Common.ResponseModel;
using Application.Users.Queries;
using AutoMapper;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<GetUserResponse>>> GetAllUsersAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Getting all users");
                var users = await _userRepository.GetAllAsync(cancellationToken);
                var userResponses = _mapper.Map<IEnumerable<GetUserResponse>>(users);
                return Result.Success(userResponses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all users");
                return Result.Failure<IEnumerable<GetUserResponse>>(new Error("GetAllUsers.Error", ex.Message));
            }
        }

        public async Task<Result<GetUserResponse>> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Getting user by ID: {UserId}", userId);
                var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
                if (user == null)
                {
                    return Result.Failure<GetUserResponse>(new Error("GetUser.NotFound", "User not found"));
                }
                
                var userResponse = _mapper.Map<GetUserResponse>(user);
                return Result.Success(userResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by ID: {UserId}", userId);
                return Result.Failure<GetUserResponse>(new Error("GetUser.Error", ex.Message));
            }
        }

        public async Task<Result> CreateUserAsync(string name, string email, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Creating user - Name: {Name}, Email: {Email}", name, email);
                
                // Validate email uniqueness
                var existingUser = await _userRepository.GetByEmailAsync(email, cancellationToken);
                if (existingUser != null)
                {
                    return Result.Failure(new Error("CreateUser.EmailExists", "User with this email already exists"));
                }

                var user = new User
                {
                    UserId = Guid.NewGuid(),
                    Name = name,
                    Email = email,
                    CreatedAt = DateTime.UtcNow
                };

                await _userRepository.AddAsync(user, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                
                _logger.LogInformation("User created successfully with ID: {UserId}", user.UserId);
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user - Name: {Name}, Email: {Email}", name, email);
                return Result.Failure(new Error("CreateUser.Error", ex.Message));
            }
        }

        public async Task<Result> UpdateUserAsync(Guid userId, string name, string email, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Updating user - ID: {UserId}, Name: {Name}, Email: {Email}", userId, name, email);
                
                var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
                if (user == null)
                {
                    return Result.Failure(new Error("UpdateUser.NotFound", "User not found"));
                }

                // Check if email is taken by another user
                var existingUser = await _userRepository.GetByEmailAsync(email, cancellationToken);
                if (existingUser != null && existingUser.UserId != userId)
                {
                    return Result.Failure(new Error("UpdateUser.EmailExists", "Email is already taken by another user"));
                }

                user.Name = name;
                user.Email = email;

                _userRepository.Update(user);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                
                _logger.LogInformation("User updated successfully: {UserId}", userId);
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user - ID: {UserId}", userId);
                return Result.Failure(new Error("UpdateUser.Error", ex.Message));
            }
        }

        public async Task<Result> DeleteUserAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Deleting user: {UserId}", userId);
                
                var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
                if (user == null)
                {
                    return Result.Failure(new Error("DeleteUser.NotFound", "User not found"));
                }

                _userRepository.Delete(user);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                
                _logger.LogInformation("User deleted successfully: {UserId}", userId);
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user: {UserId}", userId);
                return Result.Failure(new Error("DeleteUser.Error", ex.Message));
            }
        }
    }
} 