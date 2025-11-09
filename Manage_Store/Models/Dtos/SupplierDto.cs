namespace Manage_Store.Models.Dtos
{
    public class SupplierDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }

        public static SupplierDtoBuilder Builder()
        {
            return new SupplierDtoBuilder();
        }

        public class SupplierDtoBuilder
        {
            private int _id;
            private string _name = string.Empty;
            private string? _phone;
            private string? _email;
            private string? _address;

            public SupplierDtoBuilder WithId(int id)
            {
                _id = id;
                return this;
            }

            public SupplierDtoBuilder WithName(string name)
            {
                _name = name;
                return this;
            }

            public SupplierDtoBuilder WithPhone(string? phone)
            {
                _phone = phone;
                return this;
            }

            public SupplierDtoBuilder WithEmail(string? email)
            {
                _email = email;
                return this;
            }

            public SupplierDtoBuilder WithAddress(string? address)
            {
                _address = address;
                return this;
            }

            public SupplierDto Build()
            {
                return new SupplierDto
                {
                    Id = _id,
                    Name = _name,
                    Phone = _phone,
                    Email = _email,
                    Address = _address
                };
            }
        }
    }
}