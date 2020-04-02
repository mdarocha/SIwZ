namespace Server.Models
{
    public class User
    {
        public int Id { set; get; }

        public string Login { set; get; }

        public string Password { set; get; }

        public string Token { set; get; }

        public bool IsAdmin { set; get; }
    }
}