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
    public interface ITravelRequestService
    {
        IActionResult GetAll(int? pageSize = 10, int? pageNumber = 1, string q = null);
        Task<IActionResult> GetAsync(int id, bool related = false);
        Task<IActionResult> Create(TravelRequestSaveResource resource);
        Task<IActionResult> Update(int id, TravelRequestSaveResource resource);
        Task<IActionResult> Delete(int id);
    }

    public class TravelRequestService : ITravelRequestService
    {
        private readonly IRepository<TravelRequest> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<TravelRequestService> _logger;

        public TravelRequestService(IRepository<TravelRequest> repository,
            IMapper mapper, ILogger<TravelRequestService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public IActionResult GetAll(int? pageSize, int? pageNumber, string q = null)
        {

            var response = new ListModelResponse<TravelRequestResource>
            {
                PageSize = (int)pageSize,
                PageNumber = (int)pageNumber
            };
            var query = _repository.Query()
                .Include(x => x.Partner)
                .Include(x => x.PartnerContact)
                .Include(x => x.CategoryRequest)
                .Include(x => x.User)
                .Include(x => x.CategoryRequestStatus)
                .Skip((response.PageNumber - 1) * response.PageSize)
                .Take(response.PageSize).ToList();

            if (!string.IsNullOrEmpty(q) && query.Any())
            {
                q = q.ToLower();
                query = query.Where(x => x.RequestName.ToLower().Contains(q)
                || x.GuestEmail.ToLower().Contains(q)
                || x.GuestMobile.ToLower().Contains(q)
                || x.RequestInfo.ToLower().Contains(q)
                || x.RequestName.ToLower().Contains(q)).ToList();
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

        public async Task<IActionResult> GetAsync(int id, bool related = false)
        {
            var response = new SingleModelResponse<TravelRequestResource>();

            try
            {
                if (related)
                {
                    var entity = await _repository.Query()
                        .Include(x => x.Partner)
                        .Include(x => x.PartnerContact)
                        .Include(x => x.CategoryRequest)
                        .Include(x => x.User)
                        .Include(x => x.CategoryRequestStatus)
                        .FirstOrDefaultAsync(x => x.Id == id);

                    if (entity == null)
                    {
                        response.DidError = true;
                        response.ErrorMessage = ResponseMessageConstants.NotFound;
                        return response.ToHttpResponse();
                    }

                    response.Model = _mapper.Map(entity, response.Model);
                }
                else
                {
                    var entity = await _repository.Query()
                        .FirstOrDefaultAsync(x => x.Id == id);

                    if (entity == null)
                    {
                        response.DidError = true;
                        response.ErrorMessage = ResponseMessageConstants.NotFound;
                        return response.ToHttpResponse();
                    }
                    response.Model = _mapper.Map(entity, response.Model);
                }
            }
            catch (Exception ex)
            {
                response.DidError = true;
                response.ErrorMessage = ex.Message;
                _logger.LogError(ex.InnerException.ToString());
            }

            return response.ToHttpResponse();
        }

        public async Task<IActionResult> Create(TravelRequestSaveResource resource)
        {
            var response = new SingleModelResponse<TravelRequestResource>();

            if (resource == null)
            {
                response.DidError = true;
                response.ErrorMessage = ResponseMessageConstants.NotFound;
                return response.ToHttpResponse();
            }

            try
            {
                var entity = new TravelRequest();
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

        public async Task<IActionResult> Update(int id, TravelRequestSaveResource resource)
        {
            var response = new SingleModelResponse<TravelRequestResource>();

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
            var response = new SingleModelResponse<TravelRequestResource>();

            try
            {
                var model = await _repository.GetAsync(id);

                if (model == null)
                {
                    response.DidError = true;
                    response.ErrorMessage = ResponseMessageConstants.NotFound;
                    return response.ToHttpResponse();
                }

                await _repository.DeleteAsync(id);

                response.Model = _mapper.Map(model, response.Model);
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