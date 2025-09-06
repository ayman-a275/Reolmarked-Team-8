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
        private decimal _rackPrice;

        public Rack(int rackNumber, decimal rackPrice)
        {
            RackNumber = rackNumber;
            RackPrice = rackPrice;
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

        public decimal RackPrice
        {
            get => _rackPrice;
            set
            {
                _rackPrice = value;
            }
        }
    }
}
