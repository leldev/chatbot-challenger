Feature: Room
As a chat user
I want to create, join room and post message
So I can chat with other user in a room

@positive
Scenario: Create a room
	Given a new room data
	When create new room
	Then room should be created

@positive
Scenario: Get a room detail
	Given a valid room
	When get room detail
	Then room should be retrieved

@positive
Scenario: Get a list of room
	Given a valid list of room
	When get room list
	Then rooms should be retrieved

@positive
Scenario: Delete a room
	Given a valid room
	When delete room
	Then room should be deleted

@negative
Scenario: Create a room with invalid name
	Given a new room with invalid name
	When create new room
	Then room should not be created with invalid name error

@negative
Scenario: Create a room with name lenght exceeds characters
	Given a new room with long name
	When create new room
	Then room should not be created with name lenght exceeds characters
