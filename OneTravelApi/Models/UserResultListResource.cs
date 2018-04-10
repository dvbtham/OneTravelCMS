using System;

namespace OneTravelApi.Models
{
    public class UserResultListResource
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public string UserCode { get; set; }
        public string UserIdentifier { get; set; }
    }

    public class SaveUserResource 
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Avatar { get; set; }

        public DateTime LastLogin { get; set; }
    }
}
