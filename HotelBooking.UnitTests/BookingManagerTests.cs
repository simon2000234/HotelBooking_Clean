using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using HotelBooking.Core;
using HotelBooking.UnitTests.Fakes;
using Moq;
using Xunit;

namespace HotelBooking.UnitTests
{
    public class BookingManagerTests
    {
        private IBookingManager bookingManager;
        private Mock<IRepository<Booking>> moqBookingRepository;
        private Mock<IRepository<Room>> moqRoomRepository;
        private IBookingManager bookingManagerWithMoqRepos;



        public BookingManagerTests()
        {
            DateTime start = DateTime.Today.AddDays(10);
            DateTime end = DateTime.Today.AddDays(20);
            IRepository<Booking> bookingRepository = new FakeBookingRepository(start, end);
            IRepository<Room> roomRepository = new FakeRoomRepository();
            bookingManager = new BookingManager(bookingRepository, roomRepository);
            moqBookingRepository = new Mock<IRepository<Booking>>();
            moqRoomRepository = new Mock<IRepository<Room>>();
            bookingManagerWithMoqRepos = new BookingManager(moqBookingRepository.Object, moqRoomRepository.Object);
        }

        public static IEnumerable<object[]> GetNewBookings()
        {
            var customer = new Customer
            {
                Email = "email@email.com",
                Id = 1,
                Name = "email Emailsen"
            };
            var room = new Room
            {
                Id = 3,
                Description = "C"
            };
            var bookings = new List<object[]>
            {
                new object[]
                {
                    new Booking
                    {
                    Customer = customer,
                    CustomerId = 1,
                    Id = 1,
                    Room = room,
                    EndDate = DateTime.Now.AddDays(10),
                    IsActive = false,
                    RoomId =3,
                    StartDate = DateTime.Now
                  }
                }
            };
            return bookings;
        }



        [Theory]
        [MemberData(nameof(GetNewBookings))]
        public void CreateBooking_RoomsAvailiable_ReturnTrue(Booking booking)
        {
            //Arrange
            moqBookingRepository.Setup(x => x.GetAll()).Returns(new List<Booking>
            {
                new Booking { Id=1, StartDate=DateTime.Now, EndDate=DateTime.Now.AddDays(4), IsActive=true, CustomerId=1, RoomId=1 },
                new Booking { Id=2, StartDate=DateTime.Now, EndDate=DateTime.Now.AddDays(4), IsActive=true, CustomerId=2, RoomId=2 },
            });
            moqRoomRepository.Setup(x => x.GetAll()).Returns(new List<Room>
            {
                new Room { Id=1, Description="A" },
                new Room { Id=2, Description="B" },
                new Room { Id=3, Description="C"}
            });

            //Act
            var createBooking = bookingManagerWithMoqRepos.CreateBooking(booking);

            //
            Assert.True(createBooking);
        }

        [Theory]
        [MemberData(nameof(GetNewBookings))]
        public void CreateBooking_RoomsNotAvailiable_ReturnFalse(Booking booking)
        {
            //Arrange
            moqBookingRepository.Setup(x => x.GetAll()).Returns(new List<Booking>
            {
                new Booking { Id=1, StartDate=DateTime.Now, EndDate=DateTime.Now.AddDays(4), IsActive=true, CustomerId=1, RoomId=1 },
                new Booking { Id=2, StartDate=DateTime.Now, EndDate=DateTime.Now.AddDays(4), IsActive=true, CustomerId=2, RoomId=2 },
            });
            moqRoomRepository.Setup(x => x.GetAll()).Returns(new List<Room>
            {
                new Room { Id=1, Description="A" },
                new Room { Id=2, Description="B" },
            });

            //Act
            var createBooking = bookingManagerWithMoqRepos.CreateBooking(booking);

            //
            Assert.False(createBooking);
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
        [Fact]
        public void TestPipelines()
        {
            Assert.Equal(1, 1);
        }

    }
}
