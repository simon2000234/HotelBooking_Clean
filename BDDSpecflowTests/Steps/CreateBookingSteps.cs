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


        [Given(@"There is a period where all rooms are booked")]
        public void GivenThereIsAPeriodWhereAllRoomsAreBooked()
        {
            currentRooms = new List<Room>()
                { new Room { Id = 1, Description = "Nice room" }, new Room { Id = 2, Description = "Shit room"}};
            moqRoomRepository.Setup(x => x.GetAll()).Returns(currentRooms);
            currentBookings = new List<Booking>()
            {
                new Booking(){Id = 1, StartDate = DateTime.Now.AddDays(15), EndDate = DateTime.Now.AddDays(25), CustomerId = 1, IsActive = true, RoomId = 1},
                new Booking(){Id = 2, StartDate = DateTime.Now.AddDays(15), EndDate = DateTime.Now.AddDays(25), CustomerId = 2, IsActive = true, RoomId = 2}
            };
            moqBookingRepository.Setup(x => x.GetAll()).Returns(currentBookings);

        }


        [When(@"i create a booking with a start date before this period and an end date after this period")]
        public void WhenICreateABookingWithAStartDateBeforeThisPeriodAndAnEndDateAfterThisPeriod()
        {
            Booking newBooking = new Booking() { Id = 3, StartDate = DateTime.Now.AddDays(14), EndDate = DateTime.Now.AddDays(26), CustomerId = 3, IsActive = false, RoomId = 1 };
            result = bookingManagerWithMoqRepos.CreateBooking(newBooking);
        }

        [When(@"i create a booking with a start date before this period and an end date before this period")]
        public void WhenICreateABookingWithAStartDateBeforeThisPeriodAndAnEndDateBeforeThisPeriod()
        {
            Booking newBooking = new Booking() { Id = 3, StartDate = DateTime.Now.AddDays(13), EndDate = DateTime.Now.AddDays(14), CustomerId = 3, IsActive = false, RoomId = 1 };
            result = bookingManagerWithMoqRepos.CreateBooking(newBooking);
        }

        [Then(@"the new booking should be accepted")]
        public void ThenTheNewBookingShouldBeAccepted()
        {
            Assert.True(result);
        }

        [When(@"i create a booking with a start date after this period and an end date after this period")]
        public void WhenICreateABookingWithAStartDateAfterThisPeriodAndAnEndDateAfterThisPeriod()
        {
            Booking newBooking = new Booking() { Id = 3, StartDate = DateTime.Now.AddDays(26), EndDate = DateTime.Now.AddDays(27), CustomerId = 3, IsActive = false, RoomId = 1 };
            result = bookingManagerWithMoqRepos.CreateBooking(newBooking);
        }

        [When(@"i create a booking with a start date before this period and an end date at the end of this period")]
        public void WhenICreateABookingWithAStartDateBeforeThisPeriodAndAnEndDateAtTheEndOfThisPeriod()
        {
            Booking newBooking = new Booking() { Id = 3, StartDate = DateTime.Now.AddDays(14), EndDate = DateTime.Now.AddDays(24), CustomerId = 3, IsActive = false, RoomId = 1 };
            result = bookingManagerWithMoqRepos.CreateBooking(newBooking);
        }

        [When(@"i create a booking with a start date before this period and an end date at the beginning of this period")]
        public void WhenICreateABookingWithAStartDateBeforeThisPeriodAndAnEndDateAtTheBeginingOfThisPeriod()
        {
            Booking newBooking = new Booking() { Id = 3, StartDate = DateTime.Now.AddDays(14), EndDate = DateTime.Now.AddDays(16), CustomerId = 3, IsActive = false, RoomId = 1 };
            result = bookingManagerWithMoqRepos.CreateBooking(newBooking);
        }

        [When(@"i create a booking with a start date at the beginning of this period and an end date after this period")]
        public void WhenICreateABookingWithAStartDateAtTheBeginningOfThisPeriodAndAnEndDateAfterThisPeriod()
        {
            Booking newBooking = new Booking() { Id = 3, StartDate = DateTime.Now.AddDays(16), EndDate = DateTime.Now.AddDays(26), CustomerId = 3, IsActive = false, RoomId = 1 };
            result = bookingManagerWithMoqRepos.CreateBooking(newBooking);
        }

        [When(@"i create a booking with a start date at the end of this period and an end date after this period")]
        public void WhenICreateABookingWithAStartDateAtTheEndOfThisPeriodAndAnEndDateAfterThisPeriod()
        {
            Booking newBooking = new Booking() { Id = 3, StartDate = DateTime.Now.AddDays(24), EndDate = DateTime.Now.AddDays(26), CustomerId = 3, IsActive = false, RoomId = 1 };
            result = bookingManagerWithMoqRepos.CreateBooking(newBooking);
        }

        [When(@"i create a booking with a start date at the beginning of this period and an end at the end of this period")]
        public void WhenICreateABookingWithAStartDateAtTheBeginningOfThisPeriodAndAnEndAtTheEndOfThisPeriod()
        {
            Booking newBooking = new Booking() { Id = 3, StartDate = DateTime.Now.AddDays(16), EndDate = DateTime.Now.AddDays(24), CustomerId = 3, IsActive = false, RoomId = 1 };
            result = bookingManagerWithMoqRepos.CreateBooking(newBooking);
        }

        [When(@"i create a booking with a start date at the end of this period and an end at the end of this period")]
        public void WhenICreateABookingWithAStartDateAtTheEndOfThisPeriodAndAnEndAtTheEndOfThisPeriod()
        {
            Booking newBooking = new Booking() { Id = 3, StartDate = DateTime.Now.AddDays(23), EndDate = DateTime.Now.AddDays(24), CustomerId = 3, IsActive = false, RoomId = 1 };
            result = bookingManagerWithMoqRepos.CreateBooking(newBooking);
        }

        [When(@"i create a booking with a start date at the beginning of this period and an end at the beginning of this period")]
        public void WhenICreateABookingWithAStartDateAtTheBeginningOfThisPeriodAndAnEndAtTheBeginningOfThisPeriod()
        {
            Booking newBooking = new Booking() { Id = 3, StartDate = DateTime.Now.AddDays(16), EndDate = DateTime.Now.AddDays(17), CustomerId = 3, IsActive = false, RoomId = 1 };
            result = bookingManagerWithMoqRepos.CreateBooking(newBooking);
        }




    }
}
