using System.Collections.Generic;

namespace DogGo.Models.ViewModels
{
    public class WalkerProfileViewModel
    {
        public Walker Walker { get; set; }
        public List<Walk> AllWalks { get; set; }
        public List<Walk> RecentWalks { get; set; }
        public string TotalWalkTime
        {
            get
            {
                int totalSecondsWalked = 0;
                foreach (Walk walk in AllWalks)
                {
                    totalSecondsWalked += walk.Duration;
                }
                int totalMinutes = totalSecondsWalked/ 60;
                int hours = totalMinutes/ 60;
                int minutes = totalMinutes % 60;
                return $"Total Walk Time: {hours}hr {minutes}min";
                
            }
        }
        
    }
}