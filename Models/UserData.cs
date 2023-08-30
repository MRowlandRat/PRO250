using System.ComponentModel.DataAnnotations;

namespace ASP_Minesweeper.Models
{
    public class UserData
    {
        [Key]
        public Guid User_Id { get; set; }

        public int User_Wins { get; set; } = 0;

        public int User_Losses { get; set; } = 0;

        public float User_WinRate { get; set; } = 0;

        public DateTime Best_O_Time { get; set; } = DateTime.MinValue;

        public DateTime Best_SM_Time { get; set; } = DateTime.MinValue;

        public DateTime Best_M_Time { get; set; } = DateTime.MinValue;

        public DateTime Best_LG_Time { get; set; } = DateTime.MinValue;

        public DateTime Total_Time { get; set; } = DateTime.MinValue;

        public UserData()
        {
        }




    }
}
