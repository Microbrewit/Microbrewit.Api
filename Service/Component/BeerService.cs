using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Repository.Interface;
using Microbrewit.Api.Service.Interface;

namespace Microbrewit.Api.Service.Component
{
    public class BeerService : IBeerService
    {
        private readonly IBeerRepository _beerRepository;
        private readonly IBreweryService _breweryService;

        public BeerService(IBeerRepository beerRepository, IBreweryService breweryService)
        {
            _beerRepository = beerRepository;
            _breweryService = breweryService;
        }
        public async Task<IEnumerable<BeerDto>> GetAllAsync(int @from, int size)
        {
            var beers = await _beerRepository.GetAllAsync(from, size);
            return AutoMapper.Mapper.Map<IEnumerable<Beer>, IEnumerable<BeerDto>>(beers);
        }

        public async Task<BeerDto> GetSingleAsync(int id)
        {
            var beer = await _beerRepository.GetSingleAsync(id);
            return AutoMapper.Mapper.Map<Beer, BeerDto>(beer);
        }

        public async Task<BeerDto> AddAsync(BeerDto beerDto)
        {
            var beer = AutoMapper.Mapper.Map<BeerDto, Beer>(beerDto);
            if (beerDto.Recipe != null)
            {
                //BeerCalculations(beer);
            }
            beer.BeerStyle = null;
            beer.CreatedDate = DateTime.Now;
            beer.UpdatedDate = DateTime.Now;

            await _beerRepository.AddAsync(beer);
            var result = await _beerRepository.GetSingleAsync(beer.BeerId);
            var mappedResult = AutoMapper.Mapper.Map<Beer, BeerDto>(result);
            // elasticsearch relation managing
            return mappedResult;
        }

        public async Task<BeerDto> AddAsync(BeerDto beerDto, string username)
        {
            if (beerDto.Brewers != null && beerDto.Brewers.All(b => b.Username != username))
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
                if (beerDto.Brewers.Any(b => b.Username != username))
                    beerDto.Brewers.Add(new DTOUser { Username = username });
            }
            var returnBeer = await AddAsync(beerDto);
            return returnBeer;
        }

        public async Task<BeerDto> DeleteAsync(int id)
        {
            var beer = await _beerRepository.GetSingleAsync(id);
            if (beer != null) await _beerRepository.RemoveAsync(beer);
            return AutoMapper.Mapper.Map<Beer,BeerDto>(beer);
        }

        public async Task UpdateAsync(BeerDto beerDto)
        {
            var beer = AutoMapper.Mapper.Map<BeerDto, Beer>(beerDto);
            if (beer.Recipe != null)
            {
                //BeerCalculations(beer);
            }
            await _beerRepository.UpdateAsync(beer);
            var result = await _beerRepository.GetSingleAsync(beerDto.Id);
        }

        public async Task<IEnumerable<BeerDto>> SearchAsync(string query, int @from, int size)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<BeerDto>> GetUserBeersAsync(string username)
        {
            var beers = await _beerRepository.GetAllUserBeerAsync(username);
            return AutoMapper.Mapper.Map<IEnumerable<Beer>, IEnumerable<BeerDto>>(beers);
        }

        public async Task ReIndexElasticSearch(string index)
        {
            throw new NotImplementedException();
        }

        public async Task ReIndexSingleElasticSearchAsync(int beerId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<BeerDto>> GetLastAsync(int @from, int size)
        {
            var lastBeers = await _beerRepository.GetLastAsync(from, size);
            return AutoMapper.Mapper.Map<IEnumerable<Beer>, IEnumerable<BeerDto>>(lastBeers);
        }

        public async Task<IEnumerable<BeerDto>> GetAllUserBeerAsync(string username)
        {
            var userBeers = _beerRepository.GetAllUserBeerAsync(username).Result;
            return AutoMapper.Mapper.Map<IEnumerable<Beer>, IEnumerable<BeerDto>>(userBeers);
        }

        public async Task<IEnumerable<BeerDto>> GetAllBreweryBeersAsync(int breweryId)
        {
            var beers = _beerRepository.GetAllBreweryBeersAsync(breweryId).Result;
            return AutoMapper.Mapper.Map<IEnumerable<Beer>, IEnumerable<BeerDto>>(beers);
        }

        private static void BeerCalculations(Beer beer)
        {
            if (beer.Recipe.FG <= 0) beer.Recipe.FG = 1.015;
            if (beer.Recipe.Efficiency <= 0) beer.Recipe.Efficiency = 75;

            //beer.Recipe.OG = Calculation.CalculateOG(beer.Recipe);
            //var abv = Calculation.CalculateABV(beer.Recipe);
            //beer.ABV = abv;
            //var srm = Calculation.CalculateSRM(beer.Recipe);
            //beer.SRM = srm;
            //var ibu = Calculation.CalculateIBU(beer.Recipe);
            //beer.IBU = ibu;
        }
    }
}
