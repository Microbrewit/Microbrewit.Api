using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.Elasticsearch.Interface;
using Microbrewit.Api.Helper;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Repository.Interface;
using Microbrewit.Api.Service.Interface;
using Nest;

namespace Microbrewit.Api.Service.Component
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserElasticsearch _userElasticsearch;
        private readonly IEmailService _emailService;
        
        public UserService(IUserRepository userRepository, IUserElasticsearch userElasticsearch, IEmailService emailService)
        {
            _emailService = emailService;
            _userRepository = userRepository;
            _userElasticsearch = userElasticsearch;
        }

        public async Task<UserDto> GetSingleByUsernameAsync(string username)
        {
            var user = await _userRepository.GetSingleByUsernameAsync(username);
            return AutoMapper.Mapper.Map<Model.Database.User, UserDto>(user);
        }

        public async Task<UserDto> AddAsync(UserPostDto userPostDto)
        {
            var user = AutoMapper.Mapper.Map<UserPostDto, Model.Database.User>(userPostDto);
            await _userRepository.AddAsync(user);
            user = await _userRepository.GetSingleByUserIdAsync(user.UserId);
            var userDto = AutoMapper.Mapper.Map<Model.Database.User, UserDto>(user);
            //await _userElasticsearch.UpdateAsync(userDto);
            return userDto;
        }

        public Task<UserDto> DeleteAsync(string username)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync(int from, int size)
        {
            var users = await _userRepository.GetAllAsync();
            return AutoMapper.Mapper.Map<IEnumerable<Model.Database.User>,IList<UserDto>>(users);
        }

        public async Task<UserDto> GetSingleByUserIdAsync(string userId)
        {
            var user = await _userRepository.GetSingleByUserIdAsync(userId);
            return AutoMapper.Mapper.Map<Model.Database.User, UserDto>(user);
        }

        public Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(string username)
        {
            throw new NotImplementedException();
        }

        public Task ReIndexBeerRelationElasticSearch(BeerDto beerDto)
        {
            throw new NotImplementedException();
        }

        public Task ReIndexBreweryRelationElasticSearch(BreweryDto breweryDto)
        {
            throw new NotImplementedException();
        }

        public Task ReIndexElasticSearch()
        {
            throw new NotImplementedException();
        }

        public Task ReIndexUserElasticSearch(string username)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserDto>> SearchAsync(string query, int from, int size)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(UserDto userDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateNotification(string username, NotificationDto notificationDto)
        {
            throw new NotImplementedException();
        }

        public bool ExistsUsername(string username)
        {
            return _userRepository.ExistsUsername(username);
        }

        public bool ExistsEmail(string email)
        {
            return _userRepository.ExistsEmail(email);
        }

        public async Task ResetPassword(string email)
        {
            var user = await _userRepository.GetSingleByEmailAsync(email);
            if (user == null)
            {
                //TODO: Send email about attempt to reset email.
                return;
            }

            var token = RandomToken.Create();
            await _userRepository.SetResetPasswordToken(user.UserId,token);
            await _emailService.SendResetPasswordMailAsync(email, token);
        }
    }
}