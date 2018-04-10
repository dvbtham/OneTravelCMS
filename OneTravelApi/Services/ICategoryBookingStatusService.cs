using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OneTravelApi.DataLayer;
using OneTravelApi.EntityLayer;
using OneTravelApi.Responses;

namespace OneTravelApi.Services
{
    public interface ICategoryBookingStatusService
    {
        IActionResult GetAll(int? pageSize = 10, int? pageNumber = 1, string q = null);
        Task<IActionResult> GetAsync(int id);
        Task<IActionResult> Create(CategoryBookingStatus resource);
        Task<IActionResult> Update(int id, CategoryBookingStatus resource);
        Task<IActionResult> Delete(int id);
    }

    public class CategoryBookingStatusService : ICategoryBookingStatusService
    {
        private readonly IRepository<CategoryBookingStatus> _cateBookingRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryBookingStatusService> _logger;

        public CategoryBookingStatusService(IRepository<CategoryBookingStatus> cateBookingRepository, IMapper mapper, ILogger<CategoryBookingStatusService> logger)
        {
            _cateBookingRepository = cateBookingRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public IActionResult GetAll(int? pageSize, int? pageNumber, string q = null)
        {

            var response = new ListModelResponse<CategoryBookingStatus>
            {
                PageSize = (int)pageSize,
                PageNumber = (int)pageNumber
            };
            var query = _cateBookingRepository.Query().Skip((response.PageNumber - 1) * response.PageSize)
                .Take(response.PageSize).ToList();

            if (!string.IsNullOrEmpty(q) && query.Any())
            {
                q = q.ToLower();
                query = query.Where(x => x.BookingStatusName.ToLower().Contains(q)
                || x.BookingStatusCode.Contains(q)).ToList();
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
            var response = new SingleModelResponse<CategoryBookingStatus>();

            try
            {
                var entity = await _cateBookingRepository.Query().FirstOrDefaultAsync(x => x.Id == id);

                if (entity == null)
                {
                    response.DidError = true;
                    response.ErrorMessage = ResponseMessageConstants.NotFound;
                    return response.ToHttpResponse();
                }

                var resource = new CategoryBookingStatus();
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

        public async Task<IActionResult> Create(CategoryBookingStatus resource)
        {
            var response = new SingleModelResponse<CategoryBookingStatus>();

            if (resource == null)
            {
                response.DidError = true;
                response.ErrorMessage = ResponseMessageConstants.NotFound;
                return response.ToHttpResponse();
            }

            try
            {
                var entity = new CategoryBookingStatus();
                _mapper.Map(resource, entity);

                var entityAdded = await _cateBookingRepository.AddAsync(entity);

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

        public async Task<IActionResult> Update(int id, CategoryBookingStatus resource)
        {
            var response = new SingleModelResponse<CategoryBookingStatus>();

            if (resource == null)
            {
                response.DidError = true;
                response.ErrorMessage = ResponseMessageConstants.NotFound;
                return response.ToHttpResponse();
            }
            try
            {
                var entity = await _cateBookingRepository.FindAsync(x => x.Id == id);

                if (entity == null)
                {
                    response.DidError = true;
                    response.ErrorMessage = ResponseMessageConstants.NotFound;
                    return response.ToHttpResponse();
                }

                entity.BookingStatusName = resource.BookingStatusName;
                entity.BookingStatusCode = resource.BookingStatusCode;
                entity.Position = resource.Position;
                entity.IsActive = resource.IsActive;

                await _cateBookingRepository.UpdateAsync(entity);

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
            var response = new SingleModelResponse<CategoryBookingStatus>();

            try
            {
                var entity = await _cateBookingRepository.DeleteAsync(id);

                var resource = new CategoryBookingStatus();
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
