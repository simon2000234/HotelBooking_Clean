using System;
using System.Collections.Generic;
using HotelBooking.Core;
using Moq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Xunit;


namespace BDDSpecflowTests.Steps
{
    [Binding]
    public class CreateBookingSteps
    {
        private Mock<IRepository<Booking>> moqBookingRepository = new Mock<IRepository<Booking>>();
        private Mock<IRepository<Room>> moqRoomRepository = new Mock<IRepository<Room>>();
        private IBookingManager bookingManagerWithMoqRepos;
        private List<Booking> currentBookings;
        private List<Room> currentRooms;
        private bool result;
        public CreateBookingSteps()
        {
            bookingManagerWithMoqRepos = new BookingManager(moqBookingRepository.Object, moqRoomRepository.Object);
        }


        [Given(@"I have the following Rooms:")]
        public void GivenIHaveTheFollowingRooms(Table table)
        {
            currentRooms = table.CreateSet<Room>() as List<Room>;
            moqRoomRepository.Setup(x => x.GetAll()).Returns(currentRooms);
        }

        [Given(@"I have the following Bookings:")]
        public void GivenIHaveTheFollowingBookings(Table table)
        {
            currentBookings = table.CreateSet<Booking>() as List<Booking>;
            moqBookingRepository.Setup(x => x.GetAll()).Returns(currentBookings);
        }

        [When(@"the Following Booking is created:")]
        public void WhenTheFollowingBookingIsCreated(Table table)
        {
            Booking newBooking = table.CreateInstance<Booking>();
            result = bookingManagerWithMoqRepos.CreateBooking(newBooking);
        }
        
        [Then(@"the new booking should be rejected")]
        public void ThenTheNewBookingShouldBeRejected()
        {
            Assert.False(result);
        }
    }
}
