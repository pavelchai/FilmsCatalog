
namespace FilmsCatalog.Models
{
    public static class UserUtils
    {
        public static string GetUserName(this User user)
        {
            return $"{user.FirstName} {user.LastName}";
        }
    }
}