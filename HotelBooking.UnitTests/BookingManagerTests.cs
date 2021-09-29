using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using HotelBooking.Core;
using HotelBooking.UnitTests.Fakes;
using Moq;
using Xunit;
using Moq;

namespace HotelBooking.UnitTests
{
    public class BookingManagerTests
    {
        private IBookingManager bmWithMockData;
        private IBookingManager bookingManager;
        private Mock<IRepository<Booking>> moqBookingRepository;
        private Mock<IRepository<Room>> moqRoomRepository;
        private IBookingManager bookingManagerWithMoqRepos;

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

            //Assert
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

            //Assert
            Assert.False(createBooking);
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
        public void GetFullyOccupiedDates_StartDataBiggerThanEndDate_ThrowsArgumentException()
        {
            // Arrange
            DateTime startDate = DateTime.Today.AddDays(6);
            DateTime endDate = DateTime.Today.AddDays(4);

            // Act
            Action action = () => bmWithMockData.GetFullyOccupiedDates(startDate, endDate);

            // Asssert
            Assert.Throws<ArgumentException>(action);
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
