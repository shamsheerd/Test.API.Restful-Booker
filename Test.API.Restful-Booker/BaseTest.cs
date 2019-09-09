using Test.API.Framework;
using Test.API.Framework.Models;
using System;

namespace Test.API.Restful_Booker
{
    public class BaseTest
    {
        #region Members

        protected RestClient client;

        public BaseTest()
        {
            client = new RestClient();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create Booking data.
        /// </summary>
        /// <param name="firstName">First name.</param>
        /// <param name="lastName">Last name.</param>
        /// <param name="price">Price.</param>
        /// <param name="paid">Paid.</param>
        /// <param name="bookingDates">Booking dates.</param>
        /// <param name="notes">Additional notes.</param>
        /// <returns>Booking object.</returns>
        protected Booking CreateBookingData(string firstName, string lastName, int price, bool paid, Bookingdates bookingDates, string notes)
        {
            return new Booking() { firstname = firstName, lastname = lastName, totalprice = price, depositpaid = paid, bookingdates = bookingDates, additionalneeds = notes };
        }

        /// <summary>
        /// Create Booking.
        /// </summary>
        /// <returns>New Booking object.</returns>
        protected CreatedBooking CreateBooking()
        {
            Booking newBooking = CreateBookingData(
                Helper.GenerateRandomName(),
                Helper.GenerateRandomName(),
                Helper.GenerateRandomInteger(),
                true,
                new Bookingdates() { checkin = DateTime.Now.AddDays(-100).ToString("yyyy-MM-dd"), checkout = DateTime.Now.AddDays(-10).ToString("yyyy-MM-dd") },
                Helper.GenerateRandomName(100));


            return client.Post<Booking, CreatedBooking >("booking", newBooking);
        }

        #endregion
    }
}
