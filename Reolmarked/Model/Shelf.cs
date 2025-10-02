using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reolmarked.Model
{
    public class Shelf
    {
        private int _shelfNumber;
        private bool _shelfRented;
        private int _shelfTypeId;

        public Shelf(int shelfNumber, int shelfTypeId)
        {
            ShelfNumber = shelfNumber;
            ShelfRented = false;
            ShelfTypeId = shelfTypeId;
        }

        [Key]
        public int ShelfNumber
        {
            get => _shelfNumber;
            set
            {
                _shelfNumber = value;
            }
        }

        public bool ShelfRented
        {
            get => _shelfRented;
            set
            {
                _shelfRented = value;
            }
        }

        public int ShelfTypeId
        {
            get => _shelfTypeId;
            set
            {
                _shelfTypeId = value;
            }
        }
    }
}