namespace Application.Users.Commands.DeleteUser;

public class DeleteUserHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public DeleteUserHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await _userRepository.DeleteUserAsync(request.Id);
        await _unitOfWork.SaveChangesAsync();
    }
}