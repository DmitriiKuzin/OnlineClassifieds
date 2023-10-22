using DAL;
using Riok.Mapperly.Abstractions;
using UserService.Dto;

namespace UserService.Mapping;

[Mapper]
public static partial class UserMapper
{
    public static partial UserProfile ToDbModel(this CreateUserDto user);

    public static partial UserDto ToDto(this UserProfile user);

    [MapperIgnoreSource(nameof(UpdateUserDto.Id))]
    public static partial void UpdateDbModel(this UpdateUserDto userDto, UserProfile user);
}