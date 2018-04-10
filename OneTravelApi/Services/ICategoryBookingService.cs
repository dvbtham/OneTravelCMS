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
    public interface ICategoryBookingService : IBaseService<CategoryBooking>
    {
    }
    public class CategoryBookingService : ICategoryBookingService
    {
        private readonly IRepository<CategoryBooking> _cateBookingRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryBooking> _logger;

        public CategoryBookingService(IRepository<CategoryBooking> cateBookingRepository, IMapper mapper, ILogger<CategoryBooking> logger)
        {
            _cateBookingRepository = cateBookingRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public IActionResult GetAll(int? pageSize, int? pageNumber, string q = null)
        {
            
            var response = new ListModelResponse<CategoryBooking>
            {
                PageSize = (int)pageSize,
                PageNumber = (int)pageNumber
            };
            var query = _cateBookingRepository.Query().Skip((response.PageNumber - 1) * response.PageSize)
                .Take(response.PageSize).ToList();

            if (!string.IsNullOrEmpty(q) && query.Any())
            {
                q = q.ToLower();
                query = query.Where(x => x.BookingName.ToLower().Contains(q.ToLower())).ToList();
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
            var response = new SingleModelResponse<CategoryBooking>();

            try
            {
                var entity = await _cateBookingRepository.Query().FirstOrDefaultAsync(x => x.Id == id);

                if (entity == null)
                {
                    response.DidError = true;
                    response.ErrorMessage = ResponseMessageConstants.NotFound;
                    return response.ToHttpResponse();
                }

                var resource = new CategoryBooking();
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

        public async Task<IActionResult> Create(CategoryBooking resource)
        {
            var response = new SingleModelResponse<CategoryBooking>();

            if (resource == null)
            {
                response.DidError = true;
                response.ErrorMessage = ResponseMessageConstants.NotFound;
                return response.ToHttpResponse();
            }

            try
            {
                var area = new CategoryBooking();
                _mapper.Map(resource, area);

                var entity = await _cateBookingRepository.AddAsync(area);

                response.Model = _mapper.Map(entity, resource);
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

        public async Task<IActionResult> Update(int id, CategoryBooking resource)
        {
            var response = new SingleModelResponse<CategoryBooking>();

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

                entity.BookingName = resource.BookingName;
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
            var response = new SingleModelResponse<CategoryBooking>();

            try
            {
                var entity = await _cateBookingRepository.DeleteAsync(id);

                var resource = new CategoryBooking();
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
