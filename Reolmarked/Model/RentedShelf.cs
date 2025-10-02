using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reolmarked.Model
{
    public class RentedShelf
    {
        private int _rentedShelfId;
        private int _shelfNumber;
        private int _renterId;
        private decimal _rentedShelfAgreedPrice;

        public RentedShelf(int shelfNumber, int renterId, decimal rentedShelfAgreedPrice)
        {
            ShelfNumber = shelfNumber;
            RenterId = renterId;
            RentedShelfAgreedPrice = rentedShelfAgreedPrice;
        }

        [Key]
        public int RentedShelfId
        {
            get => _rentedShelfId;
            set
            {
                _rentedShelfId = value;
            }
        }

        public int ShelfNumber
        {
            get => _shelfNumber;
            set
            {
                _shelfNumber = value;
            }
        }

        public int RenterId
        {
            get => _renterId;
            set
            {
                _renterId = value;
            }
        }

        public decimal RentedShelfAgreedPrice
        {
            get => _rentedShelfAgreedPrice;
            set
            {
                _rentedShelfAgreedPrice = value;
            }
        }
    }
}
