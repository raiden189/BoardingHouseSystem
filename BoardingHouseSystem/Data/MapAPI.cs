using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace BoardingHouseSystem
{
    public class MapAPI
    {
        public static async Task<List<string>> GetPossibleAddressAsync(string addressPrefix)
        {
            Geocoder geoCoder = new Geocoder();
            IEnumerable<Position> approxPositions = await geoCoder.GetPositionsForAddressAsync(addressPrefix);
            List<string> address = new List<string>();
            foreach (Position position in approxPositions)
            {
                IEnumerable<string> possibleAddresses = await geoCoder.GetAddressesForPositionAsync(position);
                address.AddRange(possibleAddresses);
            }
            return address;
        }

        public static async Task<string> GetPossibleAddressByPosition(Position position)
        {
            Geocoder geoCoder = new Geocoder();
            IEnumerable<string> approxAddress = await geoCoder.GetAddressesForPositionAsync(position);
            return approxAddress.FirstOrDefault();
        }

        public static async Task<Position> GetPossiblePositionByAddress(string address)
        {
            Geocoder geoCoder = new Geocoder();
            IEnumerable<Position> approxPosition = await geoCoder.GetPositionsForAddressAsync(address);
            return approxPosition.FirstOrDefault();
        }

        public static async Task<DISTANCE_RANGE> GetDistanceByPosition(Position origin)
        {
            Distance distance = Distance.BetweenPositions(origin, Constants.JRMSUMapPostion);
            double meters = distance.Meters;
            if (meters > 0 && meters <= 300)
                return DISTANCE_RANGE.NEAR;
            else if (meters > 300 && meters <= 600)
                return DISTANCE_RANGE.MEDUIM;
            else
                return DISTANCE_RANGE.FAR;
        }
    }
}
