// Application/Services/UserService.cs
using Application.Interfaces.Auth;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Core.Entities;
using Core.Exceptions;
using Core.Models.Users;
using Core.Structs;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

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

            bool isCreated = await _userRepository.TryCreateAsync(
                name,
                email,
                hashedPassword,
                role
            );

            if (!isCreated)
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
            if (user == null) throw new NotFoundException("Пользователь не найден");
            return _mapper.Map<UserResponse>(user);
        }

        public async Task<UserResponse> GetAsync(string id)
        {
            UserEntity? user = await _userRepository.GetByIdAsync(id);
            if (user == null) throw new NotFoundException("Пользователь не найден");
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

            if (!validationResult.Claims.TryGetValue(CustomClaims.UserId, out object? userIdClaimValue))
            {
                 throw new UnauthorizedException("User ID claim not found in token.");
            }
            string id = userIdClaimValue?.ToString() ?? throw new UnauthorizedException("User ID claim is null.");

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

        public async Task<IEnumerable<UserResponse>> GetAsync(IEnumerable<string> ids)
        {
            var users = await _userRepository.GetManyByIdAsync(ids);
            return _mapper.Map<IEnumerable<UserResponse>>(users);
        }

        public async Task DeleteAsync(string token)
        {
            string id = await GetIdFromTokenAsync(token);
            await _userRepository.DeleteByIdAsync(id);
        }
    }
}
