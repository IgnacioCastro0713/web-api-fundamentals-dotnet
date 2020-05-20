using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using TheCodeCamp.Data;
using TheCodeCamp.Dtos.Request;
using TheCodeCamp.Dtos.Response;

namespace TheCodeCamp.Controllers
{
    [RoutePrefix("api/camps/{moniker}/talks")]
    public class TalksController : ApiController
    {
        private readonly ICampRepository _campRepository;
        private readonly IMapper _mapper;

        public TalksController(ICampRepository campRepository, IMapper mapper)
        {
            _campRepository = campRepository;
            _mapper = mapper;
        }

        [Route()]
        public async Task<IHttpActionResult> Get(string moniker, bool includeSpeakers = false)
        {
            try
            {
                var tasks = await _campRepository.GetTalksByMonikerAsync(moniker, includeSpeakers);
                return Ok(_mapper.Map<IEnumerable<TalkResDto>>(tasks));
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [Route("{id:int}", Name = "GetTalk")]
        public async Task<IHttpActionResult> Get(string moniker, int id, bool includeSpeakers = false)
        {
            try
            {
                var talk = await _campRepository.GetTalkByMonikerAsync(moniker, id, includeSpeakers);
                if (talk == null) return NotFound();
                return Ok(_mapper.Map<TalkResDto>(talk));
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [Route()]
        public async Task<IHttpActionResult> Post(string moniker, TalkReqDto talkReqDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var camp = await _campRepository.GetCampAsync(moniker);

                if (camp == null) return NotFound();

                var talk = _mapper.Map<Talk>(talkReqDto);
                talk.Camp = camp;
                _campRepository.AddTalk(talk);

                if (!await _campRepository.SaveChangesAsync()) return InternalServerError();

                return CreatedAtRoute(
                    "GetTalk",
                    new {moniker, id = talk.TalkId},
                    _mapper.Map<TalkReqDto>(talk));
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}
