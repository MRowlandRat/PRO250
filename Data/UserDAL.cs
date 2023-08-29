using ASP_Minesweeper.Interface;
using ASP_Minesweeper.Models;

namespace ASP_Minesweeper.Data
{
    public class UserDAL : IDataAccessLayer
    {

        public void AddUser(UserData user)
        {
            
        }

        public void EditUser(UserData user)
        {
            throw new NotImplementedException();
        }

        public UserData GetUserById(int? id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserData> GetUserData()
        {
            throw new NotImplementedException();
        }

        public void RemoveUser(int? id)
        {
            throw new NotImplementedException();
        }
    }
}
