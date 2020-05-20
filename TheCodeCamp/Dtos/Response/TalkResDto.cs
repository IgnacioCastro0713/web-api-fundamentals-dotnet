namespace TheCodeCamp.Dtos.Response
{
    public class TalkResDto
    {
        public int TalkId { get; set; }
        public string Title { get; set; }
        public string Abstract { get; set; }
        public int Level { get; set; }
        public SpeakerResDto SpeakerRes { get; set; }
    }
}