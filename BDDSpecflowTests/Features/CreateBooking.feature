Feature: CreateBooking
		In order to avoid creating a wrong booking
		As a Customer
		I want to be told that the booking is wrong

@mytag
Scenario: Create A booking When no rooms are availiable
	Given I have the following Rooms:
	| Id | Description |
	| 1  | Nice Room   |
	| 2  | Shit Room   |
	Given I have the following Bookings:
	| Id | StartDate  | EndDate    | IsActive | CustomerId | RoomId |
	| 1  | 08/10/2040 | 08/15/2040 | True     | 1          | 1      |
	| 2  | 08/10/2040 | 08/15/2040 | True     | 2          | 2      |
	When the Following Booking is created:
	| Id | StartDate  | EndDate    | IsActive | CustomerId | RoomId |
	| 3  | 08/11/2040 | 08/16/2040 | False    | 3          | 1      |
	Then the new booking should be rejected


Scenario: Create a booking for testcase 1
	Given There is a period where all rooms are booked
	When i create a booking with a start date before this period and an end date after this period
	Then the new booking should be rejected


Scenario: Create a booking for testcase 2
	Given There is a period where all rooms are booked
	When i create a booking with a start date before this period and an end date before this period
	Then the new booking should be accepted

Scenario: Create a booking for testcase 3
	Given There is a period where all rooms are booked
	When i create a booking with a start date after this period and an end date after this period
	Then the new booking should be accepted

Scenario: Create a booking for testcase 4
	Given There is a period where all rooms are booked
	When i create a booking with a start date before this period and an end date at the end of this period
	Then the new booking should be rejected

Scenario: Create a booking for testcase 5
	Given There is a period where all rooms are booked
	When i create a booking with a start date before this period and an end date at the beginning of this period
	Then the new booking should be rejected

Scenario: Create a booking for testcase 6
	Given There is a period where all rooms are booked
	When i create a booking with a start date at the beginning of this period and an end date after this period
	Then the new booking should be rejected

Scenario: Create a booking for testcase 7
	Given There is a period where all rooms are booked
	When i create a booking with a start date at the end of this period and an end date after this period
	Then the new booking should be rejected

Scenario: Create a booking for testcase 8
	Given There is a period where all rooms are booked
	When i create a booking with a start date at the beginning of this period and an end at the end of this period
	Then the new booking should be rejected

Scenario: Create a booking for testcase 9
	Given There is a period where all rooms are booked
	When i create a booking with a start date at the end of this period and an end at the end of this period
	Then the new booking should be rejected

Scenario: Create a booking for testcase 10
	Given There is a period where all rooms are booked
	When i create a booking with a start date at the beginning of this period and an end at the beginning of this period
	Then the new booking should be rejected




