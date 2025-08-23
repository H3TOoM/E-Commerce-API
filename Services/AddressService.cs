using E_Commerce.DTOs;
using E_Commerce.Models;
using E_Commerce.Repoistries.Base;
using E_Commerce.Services.Base;

namespace E_Commerce.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepoistory _addressRepoistory;
        private readonly IMainRepoistory<Address> _mainRepoistory;
        private readonly IUnitOfWork _unitOfWork;
        public AddressService(IAddressRepoistory addressRepoistory, IUnitOfWork unitOfWork,IMainRepoistory<Address>  mainRepoistory)
        {
            _addressRepoistory = addressRepoistory;
            _unitOfWork = unitOfWork;
            _mainRepoistory = mainRepoistory;
        }


        public async Task<IEnumerable<Address>> GetAddressesToUser( int userId )
        {
            var addresses = await _addressRepoistory.GetAddressesToUser( userId );

            if (addresses == null || !addresses.Any())
                throw new Exception( "No addresses found for the user." );


            return addresses;
        }

        public async Task<Address> GetAddressById( int addressId )
        {
            var address = await _mainRepoistory.GetByIdAsync( addressId );

            if (address == null)
                throw new Exception( "Address not found." );

            return address;
        }


        public async Task<Address> AddAddress( AddressDto dto )
        {
            // Check if the dto is valid
            if (dto == null)
                throw new ArgumentNullException( nameof( dto ), "Address data is required" );


            var address = new Address
            {
                UserId = dto.UserId,
                Street = dto.Street,
                City = dto.City,
                Country = dto.Country
            };

            await _mainRepoistory.AddAsync( address );
            await _unitOfWork.SaveChangesAsync();

            return address;

        }


        public async Task<Address> UpdateAddress( int id, AddressDto dto )
        {
            var address = await GetAddressById( id );
            if (address == null)
                throw new Exception( "Address not found." );

            address.Street = dto.Street;
            address.City = dto.City;
            address.Country = dto.Country;

            await _mainRepoistory.UpdateAsync( id, address );
            await _unitOfWork.SaveChangesAsync();

            return address;
        }



        public async Task<bool> DeleteAddress( int addressId )
        {
            var address =await GetAddressById( addressId );

            if (address == null)
                throw new Exception( "Address not found." );

            await _mainRepoistory.DeleteAsync( addressId );
            await _unitOfWork.SaveChangesAsync();

            if (await _mainRepoistory.GetByIdAsync( addressId ) != null)
                throw new Exception( "Failed to delete the address." );

            return true;
        }

       
       
    }
}
