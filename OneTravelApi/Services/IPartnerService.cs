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
    public interface IPartnerService : IBaseService<PartnerResource>
    {
    }

    public class PartnerService : IPartnerService
    {
        private readonly IRepository<Partner> _repository;
        private readonly IRepository<PartnerContact> _partnerContactRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PartnerService> _logger;

        public PartnerService(IRepository<Partner> repository, IRepository<PartnerContact> partnerContactRepository,
            IMapper mapper, ILogger<PartnerService> logger)
        {
            _repository = repository;
            _partnerContactRepository = partnerContactRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public IActionResult GetAll(int? pageSize, int? pageNumber, string q = null)
        {

            var response = new ListModelResponse<PartnerResource>
            {
                PageSize = (int)pageSize,
                PageNumber = (int)pageNumber
            };
            var query = _repository.Query()
                .Include(x => x.GroupPartner)
                .Include(x => x.PartnerContacts)
                .Skip((response.PageNumber - 1) * response.PageSize)
                .Take(response.PageSize).ToList();

            if (!string.IsNullOrEmpty(q) && query.Any())
            {
                q = q.ToLower();
                query = query.Where(x => x.PartnerName.ToLower().Contains(q)
                || x.Email.ToLower().Contains(q)
                || x.Summary.ToLower().Contains(q)).ToList();
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
            var response = new SingleModelResponse<PartnerResource>();

            try
            {
                var entity = await _repository.Query()
                    .Include(x => x.GroupPartner)
                    .Include(x => x.PartnerContacts)
                    .FirstOrDefaultAsync(x => x.Id == id);

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

        public async Task<IActionResult> Create(PartnerResource resource)
        {
            var response = new SingleModelResponse<PartnerResource>();

            if (resource == null)
            {
                response.DidError = true;
                response.ErrorMessage = ResponseMessageConstants.NotFound;
                return response.ToHttpResponse();
            }

            try
            {
                var entity = new Partner();
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

        public async Task<IActionResult> Update(int id, PartnerResource resource)
        {
            var response = new SingleModelResponse<PartnerResource>();

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
            var response = new SingleModelResponse<Partner>();

            try
            {
                var partner = _repository.Query()
                    .Include(x => x.PartnerContacts)
                    .FirstOrDefault(x => x.Id == id);

                if (partner == null)
                {
                    response.DidError = true;
                    response.ErrorMessage = ResponseMessageConstants.NotFound;
                    return response.ToHttpResponse();
                }

                await _partnerContactRepository.DeleteRangeAsync(partner.PartnerContacts.ToList());

                var entity = await _repository.DeleteAsync(id);

                response.Model = entity;
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