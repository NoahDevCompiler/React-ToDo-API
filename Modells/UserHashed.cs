namespace React__User_Control__API.Modells
{
    public class UserHashed
    {
        public string UserName { get; set; }

        public byte[] PasswordHashed { get; set; }

        public byte[] PasswordSalt { get; set; }
    }
}
