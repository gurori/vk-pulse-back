using Application.Interfaces.Auth;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Core.Entities;
using Core.Exceptions;
using Core.Models.Response;
using Core.Structs;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services
{
    public class UserService(
        IPasswordHasher passwordHasher,
        IUserRepository userRepository,
        IJwtProvider jwtProvider,
        IMapper mapper
    ) : IUserService
    {
        private readonly IPasswordHasher _passwordHasher = passwordHasher;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IJwtProvider _jwtProvider = jwtProvider;
        private readonly IMapper _mapper = mapper;

        public async Task RegisterAsync(string name, string email, string password, string role)
        {
            string hashedPassword = _passwordHasher.Generate(password);

            bool isUserExist = !await _userRepository.TryCreateAsync(
                name,
                email,
                hashedPassword,
                role
            );

            if (isUserExist)
                throw new ConflictException("Данный пользователь уже существует");
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var userEntity =
                await _userRepository.GetByEmailAsync(email)
                ?? throw new NotFoundException("Пользователь с данной почтой не зарегистрирован");

            if (!_passwordHasher.Verify(password, userEntity.PasswordHash))
                throw new ConflictException("Неверный пароль");

            var token = await _jwtProvider.GenerateTokenAsync(userEntity.Id, userEntity.Role);

            return token;
        }

        public async Task<UserResponse> GetFromTokenAsync(string token)
        {
            string id = await GetIdFromTokenAsync(token);
            UserEntity? user = await _userRepository.GetByIdAsync(id);

            return _mapper.Map<UserResponse>(user);
        }

        public async Task<UserResponse> GetAsync(string id)
        {
            UserEntity? user = await _userRepository.GetByIdAsync(id);
            return _mapper.Map<UserResponse>(user);
        }

        public async Task UpdateAsync(string id, string name)
        {
            await _userRepository.UpdateAsync(id, name);
        }

        public async Task<string> GetIdFromTokenAsync(string token)
        {
            TokenValidationResult validationResult = await _jwtProvider.ValidateTokenAsync(token);

            if (!validationResult.IsValid)
                throw new UnauthorizedException();

            string id =
                validationResult.Claims[CustomClaims.UserId].ToString()
                ?? throw new UnauthorizedException();

            return id;
        }

        public async Task<string> GetRoleAsync(string token)
        {
            string id = await GetIdFromTokenAsync(token);
            string role =
                await _userRepository.GetRoleByIdAsync(id)
                ?? throw new NotFoundException("Пользователь не найден");

            return role;
        }

        public async Task<IEnumerable<UserResponse>> GetManyAsync(IEnumerable<string> ids)
        {
            IEnumerable<UserEntity> users = await _userRepository.GetManyByIdAsync(ids);
            return _mapper.Map<UserResponse[]>(users);
        }

        public async Task DeleteAsync(string token)
        {
            string id = await GetIdFromTokenAsync(token);

            await _userRepository.DeleteByIdAsync(id);
        }

        public async Task<IEnumerable<UserResponse>> GetAsync(IEnumerable<string> ids)
        {
            IEnumerable<UserEntity> users = await _userRepository.GetManyByIdAsync(ids);

            return _mapper.Map<UserResponse[]>(users);
        }
    }
}
