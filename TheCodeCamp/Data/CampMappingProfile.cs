﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using TheCodeCamp.Dtos.Request;
using TheCodeCamp.Dtos.Response;

namespace TheCodeCamp.Data
{
    public class CampMappingProfile: Profile
    {
        public CampMappingProfile()
        {
            CreateMap<Camp, CampResDto>()
                .ForMember(c => c.Venue, opt=> opt.MapFrom(m => m.Location.VenueName))
                .ReverseMap();

            CreateMap<Camp, CampReqDto>()
                .ReverseMap();
        }
    }
}