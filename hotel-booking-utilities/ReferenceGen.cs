using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_booking_utilities
{
    public static class ReferenceGen
    {
        /// <summary>
        /// Generates a randowm number
        /// </summary>
        /// <returns>integer of randowm number</returns>
        public static int Generate()
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            return rand.Next(100000000, 999999999);
        }
    }
}
