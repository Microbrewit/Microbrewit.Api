using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microbrewit.Api.ElasticSearch.Interface;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Repository.Interface;
using Microbrewit.Api.Service.Interface;
using Microbrewit.Api.Settings;
using Microbrewit.Api.Calculations;
using Microsoft.Extensions.OptionsModel;

namespace Microbrewit.Api.Service.Component
{
    public class BeerService : IBeerService
    {
        private readonly ICalculation _calculation;
        private readonly ElasticSearchSettings _elasticsearchSettings;
        private readonly IBeerRepository _beerRepository;
        private readonly IBreweryService _breweryService;
        //private IUserService _userService;
        private IBeerElasticsearch _beerElasticsearch;

        public BeerService(IOptions<ElasticSearchSettings> elasticsearchSettings ,IBeerElasticsearch beerElasticsearch, 
        IBeerRepository beerRepository, IBreweryService breweryService, ICalculation calculation)
        {
            _elasticsearchSettings = elasticsearchSettings.Value;
            _beerRepository = beerRepository;
            _breweryService = breweryService;
            _beerElasticsearch = beerElasticsearch;
            _calculation = calculation;
        }
        public async Task<IEnumerable<BeerDto>> GetAllAsync(int @from, int size)
        {
            var beerDtos = await _beerElasticsearch.GetAllAsync(from, size);
            if (beerDtos.Any()) return beerDtos;
            var beers = await _beerRepository.GetAllAsync(from, size);
            return AutoMapper.Mapper.Map<IEnumerable<Beer>, IEnumerable<BeerDto>>(beers);
        }

        public async Task<BeerDto> GetSingleAsync(int id)
        {
             var beerDto = await _beerElasticsearch.GetSingleAsync(id);
            if (beerDto != null) return beerDto;
            var beer = await _beerRepository.GetSingleAsync(id);
            return AutoMapper.Mapper.Map<Beer, BeerDto>(beer);
        }

        public async Task<BeerDto> AddAsync(BeerDto beerDto)
        {
             var beer = AutoMapper.Mapper.Map<BeerDto, Beer>(beerDto);
            if (beerDto.Recipe != null)
            {
                BeerCalculations(beer);
            }
            beer.BeerStyle = null;
            beer.CreatedDate = DateTime.Now;
            beer.UpdatedDate = DateTime.Now;

            await _beerRepository.AddAsync(beer);
            var result = await _beerRepository.GetSingleAsync(beer.BeerId);
            var mappedResult = AutoMapper.Mapper.Map<Beer, BeerDto>(result);
            await _beerElasticsearch.UpdateAsync(mappedResult);
            // elasticsearch relation managing
            if (mappedResult.ForkOfId != null)
                await ReIndexSingleElasticSearchAsync((int)mappedResult.ForkOfId);
            if (mappedResult.Brewers.Any())
                //await _userService.ReIndexBeerRelationElasticSearch(beerDto);
            if (mappedResult.Breweries.Any())
                await _breweryService.ReIndexBeerRelationElasticSearch(beerDto);
            return mappedResult;
        }

        public async Task<BeerDto> AddAsync(BeerDto beerDto, string username)
        {
            if (beerDto.Brewers != null && beerDto.Brewers.All(b => b.UserId != username))
            {
                if (beerDto.Breweries.Any())
                {
                    var breweryMemberships = await _breweryService.GetMembershipsAsync(username);
                    if (beerDto.Breweries.Any(brewery => breweryMemberships.Any(b => b.BreweryId != brewery.Id)))
                        return null;
                }
            }
            else
            {
                if (beerDto.Brewers == null) beerDto.Brewers = new List<DTOUser>();
                if (beerDto.Brewers.Any(b => b.UserId != username))
                    beerDto.Brewers.Add(new DTOUser { UserId = username });
            }
            var returnBeer = await AddAsync(beerDto);
            //await _userService.UpdateNotification(username, new NotificationDto { Id = returnBeer.Id, Type = "UserBeer", Value = true });
            return returnBeer;
        }

        public async Task<BeerDto> DeleteAsync(int id)
        {
            var beer = await _beerRepository.GetSingleAsync(id);
            var beerDto = await _beerElasticsearch.GetSingleAsync(id);
            if (beer != null) await _beerRepository.RemoveAsync(beer);
            if (beerDto == null) return beerDto;
            await _beerElasticsearch.DeleteAsync(id);
            // elasticsearch relation managing
            //await _userService.ReIndexBeerRelationElasticSearch(beerDto);
            await _breweryService.ReIndexBeerRelationElasticSearch(beerDto);
            return beerDto;
        }

        public async Task UpdateAsync(BeerDto beerDto)
        {
             var beer = AutoMapper.Mapper.Map<BeerDto, Beer>(beerDto);
            if (beer.Recipe != null)
            {
                BeerCalculations(beer);
            }
            //Log.Debug(JsonConvert.SerializeObject(beer));
            await _beerRepository.UpdateAsync(beer);
            var result = await _beerRepository.GetSingleAsync(beerDto.Id);
            var mappedResult = AutoMapper.Mapper.Map<Beer, BeerDto>(result);
            await _beerElasticsearch.UpdateAsync(mappedResult);
            // elasticsearch relation managing
            if (mappedResult.Brewers.Any())
                //await _userService.ReIndexBeerRelationElasticSearch(beerDto);
            if (mappedResult.Breweries.Any())
                await _breweryService.ReIndexBeerRelationElasticSearch(beerDto);
        }

        public async Task<IEnumerable<BeerDto>> SearchAsync(string query, int @from, int size)
        {
            return await _beerElasticsearch.SearchAsync(query, from, size);
        }

        public async Task<IEnumerable<BeerDto>> GetUserBeersAsync(string username)
        {
            var beersDto = await _beerElasticsearch.GetUserBeersAsync(username);
            if (beersDto != null) return beersDto;
            var beers = await _beerRepository.GetAllUserBeerAsync(username);
            return AutoMapper.Mapper.Map<IEnumerable<Beer>, IEnumerable<BeerDto>>(beers);
        }

        public async Task ReIndexElasticSearch(string index)
        {
            var size = 50;
            var from = 0;
            while (true)
            {
                if (string.IsNullOrEmpty(index))
                    index = _elasticsearchSettings.Index;
                //Log.DebugFormat("From:{0} Size:{1}", from, size);
                var beers = await _beerRepository.GetAllAsync(from, size);
                var beerDtos = AutoMapper.Mapper.Map<IEnumerable<Beer>, IEnumerable<BeerDto>>(beers).ToList();
                var result = await _beerElasticsearch.ReIndexBulk(beerDtos, index);
                if (beerDtos.Count() < size)
                    break;
                from = from + size;
               
            }
        }

        public async Task ReIndexSingleElasticSearchAsync(int beerId)
        {
            var beer = await _beerRepository.GetSingleAsync(beerId);
            if (beer == null) return;
            var beerDto = AutoMapper.Mapper.Map<Beer, BeerDto>(beer);
            await _beerElasticsearch.UpdateAsync(beerDto);
        }

        public async Task<IEnumerable<BeerDto>> GetLastAsync(int @from, int size)
        {
             var lastBeersDto = await _beerElasticsearch.GetLastAsync(from, size);
            if (lastBeersDto != null) return lastBeersDto;
            var lastBeers = await _beerRepository.GetLastAsync(from, size);
            return AutoMapper.Mapper.Map<IEnumerable<Beer>, IEnumerable<BeerDto>>(lastBeers);
        }

        public async Task<IEnumerable<BeerDto>> GetAllUserBeerAsync(string username)
        {
             var userBeersDto = await _beerElasticsearch.GetUserBeersAsync(username);
            if (userBeersDto != null) return userBeersDto;
            var userBeers =  _beerRepository.GetAllUserBeerAsync(username).Result;
            return AutoMapper.Mapper.Map<IEnumerable<Beer>, IEnumerable<BeerDto>>(userBeers);
        }

        public async Task<IEnumerable<BeerDto>> GetAllBreweryBeersAsync(int breweryId)
        {
            var beersDto = await _beerElasticsearch.GetAllBreweryBeersAsync(breweryId);
            if (beersDto != null) return beersDto;
            var beers = await _beerRepository.GetAllBreweryBeersAsync(breweryId);
            return AutoMapper.Mapper.Map<IEnumerable<Beer>, IEnumerable<BeerDto>>(beers);
        }

        private void BeerCalculations(Beer beer)
        {
            if (beer.Recipe.FG <= 0) beer.Recipe.FG = 1.015;
            if (beer.Recipe.Efficiency <= 0) beer.Recipe.Efficiency = 75;

                
            beer.Recipe.OG = _calculation.CalculateOG(beer.Recipe);
            var abv = _calculation.CalculateABV(beer.Recipe);
            beer.ABV = abv;
            var srm = _calculation.CalculateSRM(beer.Recipe);
            beer.SRM = srm;
            var ibu = _calculation.CalculateIBU(beer.Recipe);
            beer.IBU = ibu;
        }
    }
}
