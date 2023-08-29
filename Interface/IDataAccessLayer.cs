using Humanizer.Localisation;
using ASP_Minesweeper.Models;

namespace ASP_Minesweeper.Interface
{
    public interface IDataAccessLayer
    {
        public void AddUser(UserData user);
        public void EditUser(UserData user);
        public UserData GetUserById(int? id);
        public IEnumerable<UserData> GetUserData();
        public void RemoveUser(int? id);

    }
}
