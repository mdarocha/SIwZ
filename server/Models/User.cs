namespace Server.Models
{
    public class User
    {
        public int ID_User { set; get; }
        
        public string Login { set; get; }
        
        public string Password { set; get; }
        
        public string Token { set; get; }
        
        public bool Is_Admin { set; get; }
        
    }
}