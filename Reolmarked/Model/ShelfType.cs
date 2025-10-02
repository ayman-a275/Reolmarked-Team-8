using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reolmarked.Model
{
    public class ShelfType
    {
        private int _shelfTypeId;
        private string _shelfTypeName;
        private string _shelfTypeDescription;
        private decimal _shelfTypePrice;

        public ShelfType(string shelfTypeName, string shelfTypeDescription, decimal shelfTypePrice)
        {
            ShelfTypeName = shelfTypeName;
            ShelfTypeDescription = shelfTypeDescription;
            ShelfTypePrice = shelfTypePrice;
        }

        [Key]
        public int ShelfTypeId
        {
            get => _shelfTypeId;
            set
            {
                _shelfTypeId = value;
            }
        }

        public string ShelfTypeName
        {
            get => _shelfTypeName;
            set
            {
                _shelfTypeName = value;
            }
        }

        public string ShelfTypeDescription
        {
            get => _shelfTypeDescription;
            set
            {
                _shelfTypeDescription = value;
            }
        }

        public decimal ShelfTypePrice
        {
            get => _shelfTypePrice;
            set
            {
                _shelfTypePrice = value;
            }
        }
    }
}