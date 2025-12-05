using Application.Interfaces.Auth;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Core.Entities; 
using Core.Models.Users; 
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtProvider _jwtProvider;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IMapper _mapper; 

        public UserService(
            IUserRepository userRepository,
            IJwtProvider jwtProvider,
            IPasswordHasher passwordHasher,
            IMapper mapper
        )
        {
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null || !_passwordHasher.VerifyPassword(password, user.PasswordHash))
            {
                throw new ApplicationException("Invalid credentials."); 
            }
            return _jwtProvider.GenerateToken(user);
        }

        public async Task RegisterAsync(string name, string email, string password, string role)
        {
            var hashedPassword = _passwordHasher.HashPassword(password);
            var success = await _userRepository.TryCreateAsync(name, email, hashedPassword, role);
            if (!success)
            {
                throw new ApplicationException("User with this email already exists.");
            }
        }

        public async Task<UserResponse> GetFromTokenAsync(string token)
        {
            var userId = _jwtProvider.GetUserIdFromToken(token); 
            if (!Guid.TryParse(userId, out Guid userGuid))
            {
                throw new ArgumentException("Invalid user ID in token.");
            }
            var userEntity = await _userRepository.GetByIdAsync(userGuid);
            if (userEntity == null)
            {
                throw new ApplicationException("User not found.");
            }
            return _mapper.Map<UserResponse>(userEntity);
        }

        public async Task<string> GetIdFromTokenAsync(string token)
        {
            return _jwtProvider.GetUserIdFromToken(token);
        }

        public async Task<UserResponse> GetAsync(Guid id) 
        {
            var userEntity = await _userRepository.GetByIdAsync(id);
            if (userEntity == null)
            {
                throw new ApplicationException($"User with ID {id} not found.");
            }
            return _mapper.Map<UserResponse>(userEntity);
        }

        public async Task UpdateAsync(Guid id, string name) 
        {
            await _userRepository.UpdateAsync(id, name);
        }

        public async Task<string> GetRoleAsync(string token)
        {
            var userId = _jwtProvider.GetUserIdFromToken(token);
            if (!Guid.TryParse(userId, out Guid userGuid))
            {
                throw new ArgumentException("Invalid user ID in token.");
            }
            var role = await _userRepository.GetRoleByIdAsync(userGuid);
            if (role == null)
            {
                throw new ApplicationException("User not found or role missing.");
            }
            return role;
        }

        public async Task<IEnumerable<UserResponse>> GetAsync(IEnumerable<Guid> ids) 
            var userEntities = await _userRepository.GetManyByIdAsync(ids);
            return _mapper.Map<IEnumerable<UserResponse>>(userEntities);
        }

        public async Task DeleteAsync(string token)
        {
            var userId = _jwtProvider.GetUserIdFromToken(token);
            if (!Guid.TryParse(userId, out Guid userGuid))
            {
                throw new ArgumentException("Invalid user ID in token.");
            }
            await _userRepository.DeleteByIdAsync(userGuid);
        }

       
        public async Task<UserEntity?> GetUserByIdAsync(Guid id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<UserEntity>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }
}

