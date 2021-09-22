using System;
using System.Collections.Generic;
using System.IO;
using HotelBooking.Core;
using HotelBooking.UnitTests.Fakes;
using Xunit;

namespace HotelBooking.UnitTests
{
    public class BookingManagerTests
    {
        private IBookingManager bookingManager;

        public BookingManagerTests(){
            DateTime start = DateTime.Today.AddDays(10);
            DateTime end = DateTime.Today.AddDays(20);
            IRepository<Booking> bookingRepository = new FakeBookingRepository(start, end);
            IRepository<Room> roomRepository = new FakeRoomRepository();
            bookingManager = new BookingManager(bookingRepository, roomRepository);
        }

        [Fact]
        public void FindAvailableRoom_StartDateNotInTheFuture_ThrowsArgumentException()
        {
            //Arrange
            DateTime date = DateTime.Today;

            //Act
            Action act = () => bookingManager.FindAvailableRoom(date, date);

            //Assert
            Assert.Throws<ArgumentException>(act);
        }


        [Theory]
        [MemberData(nameof(GetDateData))]
        public void FindAvailableRoom_StartDateNotInvalid_ThrowsArgumentException(DateTime startDate, DateTime endDate)
        {
            //Arange
            Action act = () => bookingManager.FindAvailableRoom(startDate, endDate);

            //Act
            Exception ex = Assert.Throws<ArgumentException>(act);

            //Assert
            Assert.Equal("The start date cannot be in the past or later than the end date.", ex.Message);
        }

        [Fact]
        public void FindAvailableRoom_RoomAvailable_RoomIdNotMinusOne()
        {
            // Arrange
            DateTime date = DateTime.Today.AddDays(1);
            // Act
            int roomId = bookingManager.FindAvailableRoom(date, date);
            // Assert
            Assert.NotEqual(-1, roomId);
        }

        public static IEnumerable<object[]> GetDateData()
        {
            var data = new List<object[]>
            {
            new object[] {DateTime.Today.AddDays(-1), DateTime.Today.AddDays(+2) },
            new object[] {DateTime.Today.AddDays(+2), DateTime.Today.AddDays(-1) }
            };
            return data;
        }
    }
}
