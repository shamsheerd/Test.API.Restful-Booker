
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Test.API.Framework.Models
{
    public class BookingList
    {
        public List<Bookings> bookings { get; set; }
    }

    public class Bookings
    {
        [JsonProperty("bookingid")]
        public long Bookingid { get; set; }
    }

    public class Bookingdates
    {
        public string checkin { get; set; }
        public string checkout { get; set; }
    }

    public class Booking
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public int totalprice { get; set; }
        public bool depositpaid { get; set; }
        public Bookingdates bookingdates { get; set; }
        public string additionalneeds { get; set; }
    }

    public class CreatedBooking
    {
        public Booking booking { get; set; }
        public int bookingid { get; set; }
    }

}
