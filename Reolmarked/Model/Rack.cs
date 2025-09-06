using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reolmarked.Model
{
    class Rack
    {
        int _rackNumber;

        public Rack(int rackNumber)
        {
            _rackNumber = rackNumber;
        }

        public int RackNumber
        {
            get => _rackNumber;
            set
            {
                _rackNumber = value;
            }
        }
    }
}
