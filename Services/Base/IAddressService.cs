using E_Commerce.DTOs;
using E_Commerce.Models;

namespace E_Commerce.Services.Base
{
    public interface IAddressService
    {
        Task<IEnumerable<Address>> GetAddressesToUser(int userId);

        Task<Address> AddAddress(AddressDto dto);

        Task<Address> GetAddressById(int addressId);

        Task<Address> UpdateAddress(int id , AddressDto dto);

        Task<bool> DeleteAddress(int addressId);

    }
}
