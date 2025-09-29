using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reolmarked.Model
{
    public class Rack
    {
        private int _rackNumber;
        private bool _rackRented;
        private int _rackTypeId;

        public Rack(int rackNumber, int rackTypeId)
        {
            RackNumber = rackNumber;
            RackRented = false;
            RackTypeId = rackTypeId;
        }

        [Key]
        public int RackNumber
        {
            get => _rackNumber;
            set
            {
                _rackNumber = value;
            }
        }

        public bool RackRented
        {
            get => _rackRented;
            set
            {
                _rackRented = value;
            }
        }

        public int RackTypeId
        {
            get => _rackTypeId;
            set
            {
                _rackTypeId = value;
            }
        }
    }
}