using Application.Common.ResponseModel;
using Application.Users.Queries;

namespace Application.Services
{
    public interface IUserService
    {
        Task<Result<IEnumerable<GetUserResponse>>> GetAllUsersAsync(CancellationToken cancellationToken = default);
        Task<Result<GetUserResponse>> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<Result> CreateUserAsync(string name, string email, CancellationToken cancellationToken = default);
        Task<Result> UpdateUserAsync(Guid userId, string name, string email, CancellationToken cancellationToken = default);
        Task<Result> DeleteUserAsync(Guid userId, CancellationToken cancellationToken = default);
    }
} 