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