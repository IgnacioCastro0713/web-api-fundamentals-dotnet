using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using TheCodeCamp.Data;
using TheCodeCamp.Dtos.Request;
using TheCodeCamp.Dtos.Response;

namespace TheCodeCamp.Controllers
{
    [RoutePrefix("api/camps")]
    public class CampsController : ApiController
    {
        private readonly ICampRepository _campRepository;
        private readonly IMapper _mapper;

        public CampsController(ICampRepository campRepository, IMapper mapper)
        {
            _campRepository = campRepository;
            _mapper = mapper;
        }

        [Route()]
        public async Task<IHttpActionResult> Get(bool includeTalks = false)
        {
            try
            {
                var result = await _campRepository.GetAllCampsAsync(includeTalks);
                var mapperResult = _mapper.Map<IEnumerable<CampResDto>>(result);
                return Ok(mapperResult);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [Route("{moniker}", Name = "GetCamp")]
        public async Task<IHttpActionResult> Get(string moniker, bool includeTalks = false)
        {
            try
            {
                var result = await _campRepository.GetCampAsync(moniker, includeTalks);

                if (result == null) return NotFound();

                return Ok(_mapper.Map<CampResDto>(result));
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [Route("searchByDate/{eventDate:datetime}")]
        [HttpGet]
        public async Task<IHttpActionResult> SearchByEventDate(DateTime eventDate, bool includeTalks = false)
        {
            try
            {
                var result = await _campRepository.GetAllCampsByEventDate(eventDate, includeTalks);
                return Ok(_mapper.Map<CampResDto[]>(result));
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [Route()]
        public async Task<IHttpActionResult> Post(CampReqDto reqDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var camp = _mapper.Map<Camp>(reqDto);
                _campRepository.AddCamp(camp);

                if (!await _campRepository.SaveChangesAsync()) return InternalServerError();
                
                var newCamp = _mapper.Map<CampReqDto>(camp);
                return CreatedAtRoute("GetCamp", new {moniker = newCamp.Moniker}, newCamp);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

        }

        [Route("{moniker}")]
        public async Task<IHttpActionResult> Put(string moniker, CampReqDto req)
        {
            try
            {
                var camp = await _campRepository.GetCampAsync(moniker);

                if (camp == null) return NotFound();

                _mapper.Map(req, camp);

                if (!await _campRepository.SaveChangesAsync())
                    return InternalServerError();

                return Ok(_mapper.Map<CampReqDto>(camp));
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [Route("{moniker}")]
        public async Task<IHttpActionResult> Delete(string moniker)
        {
            try
            {
                var camp = await _campRepository.GetCampAsync(moniker);

                if (camp == null) return NotFound();

                _campRepository.DeleteCamp(camp);

                if (!await _campRepository.SaveChangesAsync())
                    return InternalServerError();

                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}