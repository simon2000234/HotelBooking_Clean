using System;
using System.Collections.Generic;
using HotelBooking.Core;
using HotelBooking.UnitTests.Fakes;
using Xunit;
using Moq;

namespace HotelBooking.UnitTests
{
    public class BookingManagerTests
    {
        private IBookingManager bmWithMockData;
        private IBookingManager bookingManager;

        public BookingManagerTests(){

            IList<Room> rooms = new List<Room>
            {
                new Room {Id = 1, Description = "Room 1 Description"},
                new Room {Id = 2, Description = "Room 2 Description"},
                new Room {Id = 3, Description = "Room 3 Description"},
                new Room {Id = 4, Description = "Room 4 Description"},
            };

            IList<Customer> customers = new List<Customer>
            {
                new Customer {Id = 1, Email = "simon@dabmail.com", Name = "Simon"},
                new Customer {Id = 2, Email = "casper@mail.com", Name = "Casper"},
                new Customer {Id = 3, Email = "christian@mail.com", Name = "Christian"},
                new Customer {Id = 4, Email = "richart@mail.com", Name = "Richart"}
            };

            IList<Booking> bookings = new List<Booking>
            {
                new Booking
                {
                    Id = 1,
                    CustomerId = 1,
                    Customer = customers[0],
                    Room = rooms[0],
                    RoomId = 1,
                    IsActive = true,
                    StartDate = DateTime.Today.AddDays(1),
                    EndDate = DateTime.Today.AddDays(3)
                },
                new Booking
                {
                    Id = 2,
                    CustomerId = 2,
                    Customer = customers[1],
                    Room = rooms[1],
                    RoomId = 2,
                    IsActive = true,
                    StartDate = DateTime.Today.AddDays(2),
                    EndDate = DateTime.Today.AddDays(4)
                },
                new Booking
                {
                    Id = 3,
                    CustomerId = 3,
                    Customer = customers[2],
                    Room = rooms[2],
                    RoomId = 3,
                    IsActive = true,
                    StartDate = DateTime.Today.AddDays(3),
                    EndDate = DateTime.Today.AddDays(5)
                },
                new Booking
                {
                    Id = 4,
                    CustomerId = 4,
                    Customer = customers[3],
                    Room = rooms[3],
                    RoomId = 4,
                    IsActive = true,
                    StartDate = DateTime.Today.AddDays(2),
                    EndDate = DateTime.Today.AddDays(6)
                }
            };

            Mock<IRepository<Room>> mockRoomRepo = new Mock<IRepository<Room>>();
            Mock<IRepository<Customer>> mockCustomerRepo = new Mock<IRepository<Customer>>();
            Mock<IRepository<Booking>> mockBookingRepo = new Mock<IRepository<Booking>>();

            // Mock GetAll() method
            mockRoomRepo.Setup(mrr => mrr.GetAll()).Returns(rooms);
            mockCustomerRepo.Setup(mcr => mcr.GetAll()).Returns(customers);
            mockBookingRepo.Setup(mbr => mbr.GetAll()).Returns(bookings);

            bmWithMockData = new BookingManager(mockBookingRepo.Object, mockRoomRepo.Object);


            DateTime start = DateTime.Today.AddDays(10);
            DateTime end = DateTime.Today.AddDays(20);
            IRepository<Booking> bookingRepository = new FakeBookingRepository(start, end);
            IRepository<Room> roomRepository = new FakeRoomRepository();
            bookingManager = new BookingManager(bookingRepository, roomRepository);
        }

        [Fact]
        public void GetFullyOccupiedDates_WithOneFullyOccupiedDate()
        {
            // Arrange
            DateTime startDate = DateTime.Today.AddDays(1);
            DateTime endDate = DateTime.Today.AddDays(4);

            // Act
            List<DateTime> result = bmWithMockData.GetFullyOccupiedDates(startDate, endDate);

            // Assert
            Assert.Single(result);
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

        [Fact]
        public void TestPipelines()
        {
            Assert.Equal(1,1);
        }

    }
}
