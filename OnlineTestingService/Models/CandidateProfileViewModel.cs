namespace OnlineTestingService.Models
{
    public class CandidateProfileViewModel
    {
        public int Id { get; set; }
        public int[] PerfectSkills { get; set; }
        public int[] GoodSkills { get; set; }
        public int[] BasicSkills { get; set; }

        public CandidateProfileViewModel()
        {
            PerfectSkills = new int[0];
            GoodSkills = new int[0];
            BasicSkills = new int[0];
        }
    }
}