using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reolmarked.Model
{
    public class RentedRack
    {
        private int _rentedRackId;
        private int _rackNumber;
        private int _renterId;
        private decimal _rentedRackAgreedPrice;

        public RentedRack(int rackNumber, int renterId, decimal rentedRackAgreedPrice)
        {
            RackNumber = rackNumber;
            RenterId = renterId;
            RentedRackAgreedPrice = rentedRackAgreedPrice;
        }

        [Key]
        public int RentedRackId
        {
            get => _rentedRackId;
            set
            {
                _rentedRackId = value;
            }
        }

        public int RackNumber
        {
            get => _rackNumber;
            set
            {
                _rackNumber = value;
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

        public decimal RentedRackAgreedPrice
        {
            get => _rentedRackAgreedPrice;
            set
            {
                _rentedRackAgreedPrice = value;
            }
        }
    }
}
