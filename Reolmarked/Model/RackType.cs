using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reolmarked.Model
{
    public class RackType
    {
        private int _rackTypeId;
        private string _rackTypeName;
        private string _rackTypeDescription;
        private decimal _rackTypePrice;

        public RackType(string rackTypeName, string rackTypeDescription, decimal rackTypePrice)
        {
            RackTypeName = rackTypeName;
            RackTypeDescription = rackTypeDescription;
            RackTypePrice = rackTypePrice;
        }

        [Key]
        public int RackTypeId
        {
            get => _rackTypeId;
            set
            {
                _rackTypeId = value;
            }
        }

        public string RackTypeName
        {
            get => _rackTypeName;
            set
            {
                _rackTypeName = value;
            }
        }

        public string RackTypeDescription
        {
            get => _rackTypeDescription;
            set
            {
                _rackTypeDescription = value;
            }
        }

        public decimal RackTypePrice
        {
            get => _rackTypePrice;
            set
            {
                _rackTypePrice = value;
            }
        }
    }
}