using ChatApp.Core.Entities;


namespace ChatApp.Entities.Models
{
    public  class User:BaseEntity
    {
        public string Name { get; set; }=string.Empty;
       // public string Password { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public string Status { get; set; }=string.Empty;
    }

}
