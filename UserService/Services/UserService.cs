using DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserService.Dto;
using UserService.Mapping;

namespace UserService.Services;

public class UserService
{
    private readonly ClassifiedsDbContext _context;
    private readonly IPasswordHasher<UserProfile> _passwordHasher;


    public UserService(ClassifiedsDbContext context, IPasswordHasher<UserProfile> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async Task<long> CreateUser(CreateUserDto user)
    {
        var newUser = user.ToDbModel();
        newUser.PasswordHash = _passwordHasher.HashPassword(newUser, user.Password);
        _context.UserProfiles.Add(newUser);
        await _context.SaveChangesAsync();
        return newUser.Id;
    }

    public async Task<long> UpdateUser(UpdateUserDto user)
    {
        var existingUser = await _context
            .UserProfiles.FirstOrDefaultAsync(x => x.Id == user.Id);
        if (existingUser == null) throw new Exception("Пользователь не найден");
        user.UpdateDbModel(existingUser);
        await _context.SaveChangesAsync();
        return user.Id;
    }

    public async Task<UserDto> GetUser(int userId)
    {
        var existingUser = await _context
            .UserProfiles
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == userId);
        if (existingUser == null) throw new Exception("Пользователь не найден");
        return existingUser.ToDto();
    }

    public async Task DeleteUser(int userId)
    {
        var existingUser = await _context
            .UserProfiles
            .FirstOrDefaultAsync(x => x.Id == userId);
        if (existingUser == null) throw new Exception("Пользователь не найден");
        _context.UserProfiles.Remove(existingUser);
        await _context.SaveChangesAsync();
    }
}