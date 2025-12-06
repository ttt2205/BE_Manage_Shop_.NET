namespace Manage_Store.Models.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public required string Username { get; set; }

        public required string FullName { get; set; }
        public required string Role { get; set; }

        // 🔹 Tạo builder
        public static UserDtoBuilder Builder()
        {
            return new UserDtoBuilder();
        }

        // 🔹 Lớp builder lồng bên trong
        public class UserDtoBuilder
        {
            private int _id;
            private string _username = string.Empty;
            private string _fullName = string.Empty;
            private string _role = string.Empty;

            public UserDtoBuilder WithId(int id)
            {
                _id = id;
                return this;
            }

            public UserDtoBuilder WithUsername(string username)
            {
                _username = username;
                return this;
            }

            public UserDtoBuilder WithFullName(string fullName)
            {
                _fullName = fullName;
                return this;
            }

            public UserDtoBuilder WithRole(string role)
            {
                _role = role;
                return this;
            }

            // 🔹 Phương thức Build() cuối cùng
            public UserDto Build()
            {
                return new UserDto
                {
                    Id = _id,
                    Username = _username,
                    // Email = _email,
                    FullName = _fullName,
                    Role = _role
                };
            }
        }
    }
}
