using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TheCodeCamp.Dtos.Request
{
    public class TalkReqDto
    {
        public int TalkId { get; set; }
        public string Title { get; set; }
        public string Abstract { get; set; }
        public int Level { get; set; }
    }
}