using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Test.API.Framework;
using Test.API.Framework.Models;

namespace Test.API.Restful_Booker
{
    [TestClass]
    public class BookerTest : BaseTest
    {

        #region Test Methods
        
        [TestCategory("Booking"), Owner("TestAssignment"), Priority(1)]
        [TestMethod, Description("Verify Get booking returns proper data set.")]
        public void Verify_GetBookings_Returns_Valid_Data_Test()
        {
            // Create new booking.
            CreatedBooking newBooking = CreateBooking();

            // Get all bookings post creation of new booking.
            List<Bookings> actBookings = client.Get<List<Bookings>>("booking");

            // Validate get all bookings 
            AssertManager.Execute(() => Assert.AreEqual(actBookings.Where(b => b.Bookingid == newBooking.bookingid).Count() , 1),
                string.Format("Booking Id '{0}' of the bookings should exist when get all bookings fetched.", newBooking.bookingid));

            // Validate assertions.
            AssertManager.ValidateTest();
        }

        [TestCategory("Booking"), Owner("TestAssignment"), Priority(1)]
        [TestMethod, Description("Verify new booking is saved properly.")]
        public void Verify_New_Booking_Test()
        {
            // Create new booking
            CreatedBooking newBooking = CreateBooking();

            // Validate new booking.
            AssertManager.Execute(() => Assert.IsTrue(newBooking.bookingid > 0),
               string.Format("Booking Id '{0}' of the bookings should not be 0.", newBooking.bookingid));

            // Validate complete booking details.
            Booking booking = client.Get<Booking>(string.Format("booking/{0}", newBooking.bookingid));
            AssertManager.Execute(() => Assert.IsTrue(booking.firstname.Equals(newBooking.booking.firstname)),
                string.Format("New booking first name '{0}' should match.", booking.firstname));
            AssertManager.Execute(() => Assert.IsTrue(booking.lastname.Equals(newBooking.booking.lastname)),
                string.Format("New booking last name '{0}' should match.", booking.lastname));
            AssertManager.Execute(() => Assert.IsTrue(booking.totalprice == newBooking.booking.totalprice),
                string.Format("New booking total price '{0}' should match.", booking.totalprice));
            AssertManager.Execute(() => Assert.IsTrue(booking.depositpaid.Equals(newBooking.booking.depositpaid)),
                string.Format("New booking deposit paid'{0}' should match.", booking.depositpaid));
            AssertManager.Execute(() => Assert.IsTrue(booking.bookingdates.checkin.Equals(newBooking.booking.bookingdates.checkin)),
                string.Format("New booking check-in date '{0}' should match.", booking.bookingdates.checkin));
            AssertManager.Execute(() => Assert.IsTrue(booking.bookingdates.checkout.Equals(newBooking.booking.bookingdates.checkout)),
                string.Format("New booking check-out date '{0}' should match.", booking.bookingdates.checkout));

            // Validate assertions.
            AssertManager.ValidateTest();
        }


        [TestCategory("Booking"), Owner("TestAssignment"), Priority(1)]
        [TestMethod, Description("Verify deleting existing booking.")]
        public void Verify_Delete_Booking_Test()
        {
            // Create new booking
            CreatedBooking newBooking = CreateBooking();

            // Delete the created booking.
            client.Delete(string.Format("booking/{0}", newBooking.bookingid));

            // Get all bookings post deletion of new booking.
            List<Bookings> actBookings = client.Get<List<Bookings>>("booking");

            // Validate get all bookings 
            AssertManager.Execute(() => Assert.AreEqual(actBookings.Where(b => b.Bookingid == newBooking.bookingid).Count(), 0),
                string.Format("Get all bookings should not have deleted '{0}' booking id.", newBooking.bookingid));

            // Validate assertions.
            AssertManager.ValidateTest();
        }


        #endregion

        #region Helpers

        #endregion
    }
}
