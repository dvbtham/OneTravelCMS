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
    public interface ICategoryGroupPartnerService : IBaseService<CategoryGroupPartnerResource>
    {
    }

    public class CategoryGroupPartnerService : ICategoryGroupPartnerService
    {
        private readonly IRepository<CategoryGroupPartner> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryGroupPartnerService> _logger;

        public CategoryGroupPartnerService(IRepository<CategoryGroupPartner> repository, IMapper mapper, ILogger<CategoryGroupPartnerService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public IActionResult GetAll(int? pageSize, int? pageNumber, string q = null)
        {

            var response = new ListModelResponse<CategoryGroupPartnerResource>
            {
                PageSize = (int)pageSize,
                PageNumber = (int)pageNumber
            };
            var query = _repository.Query().Skip((response.PageNumber - 1) * response.PageSize)
                .Take(response.PageSize).ToList();

            if (!string.IsNullOrEmpty(q) && query.Any())
            {
                q = q.ToLower();
                query = query.Where(x =>
                                    x.GroupPartnerCode.ToLower().Contains(q) ||
                                    x.GroupPartnerName.ToLower().Contains(q)).ToList();
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
            var response = new SingleModelResponse<CategoryGroupPartnerResource>();

            try
            {
                var entity = await _repository.Query().FirstOrDefaultAsync(x => x.Id == id);

                if (entity == null)
                {
                    response.DidError = true;
                    response.ErrorMessage = ResponseMessageConstants.NotFound;
                    return response.ToHttpResponse();
                }

                var resource = new CategoryGroupPartnerResource();
                _mapper.Map(entity, resource);
                response.Model = resource;
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.Message;
                _logger.LogError(ex.InnerException.ToString());
            }

            return response.ToHttpResponse();
        }

        public async Task<IActionResult> Create(CategoryGroupPartnerResource resource)
        {
            var response = new SingleModelResponse<CategoryGroupPartnerResource>();

            if (resource == null)
            {
                response.DidError = true;
                response.ErrorMessage = ResponseMessageConstants.NotFound;
                return response.ToHttpResponse();
            }

            try
            {
                var entity = new CategoryGroupPartner();
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

        public async Task<IActionResult> Update(int id, CategoryGroupPartnerResource resource)
        {
            var response = new SingleModelResponse<CategoryGroupPartnerResource>();

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

                entity.GroupPartnerCode = resource.GroupPartnerCode;
                entity.GroupPartnerName = resource.GroupPartnerName;
                entity.IsLocal = resource.IsLocal;
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
            var response = new SingleModelResponse<CategoryGroupPartnerResource>();

            try
            {
                var entity = await _repository.DeleteAsync(id);

                var resource = new CategoryGroupPartnerResource();
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
