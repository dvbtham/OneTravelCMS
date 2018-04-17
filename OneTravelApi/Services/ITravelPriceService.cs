using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OneTravelApi.DataLayer;
using OneTravelApi.EntityLayer;
using OneTravelApi.Resources;
using OneTravelApi.Responses;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OneTravelApi.Services
{
    public interface ITravelPriceService : IBaseService<TravelPriceSaveResource>
    {
    }

    public class TravelPriceService : ITravelPriceService
    {
        private readonly IRepository<TravelPrice> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<TravelPriceService> _logger;

        public TravelPriceService(IRepository<TravelPrice> repository, IMapper mapper, ILogger<TravelPriceService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public IActionResult GetAll(int? pageSize, int? pageNumber, string q = null)
        {
            var response = new ListModelResponse<TravelPriceResource>
            {
                PageSize = (int)pageSize,
                PageNumber = (int)pageNumber
            };

            var query = _repository.Query()
                .Include(x => x.CategoryLocalTravel)
                .Include(x => x.CategoryPrice)
                .Include(x => x.User)
                .Skip((response.PageNumber - 1) * response.PageSize)
                .Take(response.PageSize).ToList();

            if (!string.IsNullOrEmpty(q) && query.Any())
            {
                q = q.ToLower();
                query = query.Where(x => x.PriceName.ToLower().Contains(q)).ToList();
            }

            try
            {
                response.Model = _mapper.Map(query, response.Model);

                response.Message = string.Format("Total of records: {0}", response.Model.Count());
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.Message;
                _logger.LogError(ex.InnerException.ToString());
            }

            return response.ToHttpResponse();
        }

        public async Task<IActionResult> GetAsync(int id)
        {
            var response = new SingleModelResponse<TravelPriceResource>();

            try
            {
                var entity = await _repository.Query().FirstOrDefaultAsync(x => x.Id == id);

                if (entity == null)
                {
                    response.DidError = true;
                    response.ErrorMessage = ResponseMessageConstants.NotFound;
                    return response.ToHttpResponse();
                }

                response.Model = _mapper.Map(entity, response.Model);
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.Message;
                _logger.LogError(ex.InnerException.ToString());
            }

            return response.ToHttpResponse();
        }

        public async Task<IActionResult> Create(TravelPriceSaveResource resource)
        {
            var response = new SingleModelResponse<TravelPriceResource>();

            if (resource == null)
            {
                response.DidError = true;
                response.ErrorMessage = ResponseMessageConstants.NotFound;
                return response.ToHttpResponse();
            }

            try
            {
                var entity = new TravelPrice();
                _mapper.Map(resource, entity);
               
                var entityAdded = await _repository.AddAsync(entity);

                response.Model = _mapper.Map(entityAdded, response.Model);
                response.Message = ResponseMessageConstants.Success;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.ToString();
                _logger.LogError(ex.InnerException.ToString());
            }

            return response.ToHttpResponse();
        }

        public async Task<IActionResult> Update(int id, TravelPriceSaveResource resource)
        {
            var response = new SingleModelResponse<TravelPriceResource>();

            if (resource == null)
            {
                response.DidError = true;
                response.ErrorMessage = ResponseMessageConstants.NotFound;
                return response.ToHttpResponse();
            }
            try
            {
                var entity = await _repository.FindAsync(x => x.Id == id);

                if (entity == null)
                {
                    response.DidError = true;
                    response.ErrorMessage = ResponseMessageConstants.NotFound;
                    return response.ToHttpResponse();
                }

                _mapper.Map(resource, entity);

                await _repository.UpdateAsync(entity);

                response.Model = _mapper.Map(entity, response.Model);
                response.Message = ResponseMessageConstants.Success;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.ToString();
                _logger.LogInformation(ex.Message);
                _logger.LogTrace(ex.InnerException.ToString());
            }

            return response.ToHttpResponse();
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = new SingleModelResponse<TravelPriceResource>();

            try
            {
                var entity = await _repository.DeleteAsync(id);

                response.Model = _mapper.Map(entity, response.Model);
                response.Message = ResponseMessageConstants.Delete;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.Message;
                _logger.LogError(ex.InnerException.ToString());
            }

            return response.ToHttpResponse();
        }
    }
}
