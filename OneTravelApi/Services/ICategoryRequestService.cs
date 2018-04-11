using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OneTravelApi.DataLayer;
using OneTravelApi.EntityLayer;
using OneTravelApi.Resources;
using OneTravelApi.Responses;

namespace OneTravelApi.Services
{
    public interface ICategoryRequestService : IBaseService<CategoryRequest>
    {
        
    }

    public class CategoryRequestService : ICategoryRequestService
    {
        private readonly IRepository<CategoryRequest> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryRequestService> _logger;

        public CategoryRequestService(IRepository<CategoryRequest> repository, IMapper mapper, ILogger<CategoryRequestService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public IActionResult GetAll(int? pageSize, int? pageNumber, string q = null)
        {

            var response = new ListModelResponse<CategoryRequest>
            {
                PageSize = (int)pageSize,
                PageNumber = (int)pageNumber
            };
            var query = _repository.Query().Skip((response.PageNumber - 1) * response.PageSize)
                .Take(response.PageSize).ToList();

            if (!string.IsNullOrEmpty(q) && query.Any())
            {
                q = q.ToLower();
                query = query.Where(x => x.RequestName.ToLower().Contains(q)).ToList();
            }

            try
            {
                response.Model = query;

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
            var response = new SingleModelResponse<CategoryRequest>();

            try
            {
                var entity = await _repository.Query().FirstOrDefaultAsync(x => x.Id == id);

                if (entity == null)
                {
                    response.DidError = true;
                    response.ErrorMessage = ResponseMessageConstants.NotFound;
                    return response.ToHttpResponse();
                }

                response.Model = entity;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.Message;
                _logger.LogError(ex.InnerException.ToString());
            }

            return response.ToHttpResponse();
        }

        public async Task<IActionResult> Create(CategoryRequest resource)
        {
            var response = new SingleModelResponse<CategoryRequest>();

            if (resource == null)
            {
                response.DidError = true;
                response.ErrorMessage = ResponseMessageConstants.NotFound;
                return response.ToHttpResponse();
            }

            try
            {
                var entity = new CategoryRequest();
                _mapper.Map(resource, entity);

                var entityAdded = await _repository.AddAsync(entity);

                response.Model = _mapper.Map(entityAdded, resource);
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

        public async Task<IActionResult> Update(int id, CategoryRequest resource)
        {
            var response = new SingleModelResponse<CategoryRequest>();

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

                entity.RequestName = resource.RequestName;
                entity.Position = resource.Position;
                entity.IsActive = resource.IsActive;

                await _repository.UpdateAsync(entity);

                response.Model = _mapper.Map(entity, resource);
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
            var response = new SingleModelResponse<CategoryRequest>();

            try
            {
                var entity = await _repository.DeleteAsync(id);

                var resource = new CategoryRequest();
                response.Model = _mapper.Map(entity, resource);
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