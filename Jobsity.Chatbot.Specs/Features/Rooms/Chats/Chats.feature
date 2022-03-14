Feature: Chats
As a chat user
I want to post a message
So I can chat with other user in a room

@positive
Scenario: Create a chat
	Given a valid user
	And a valid room
	And a new chat data
	When create new chat
	Then chat should be created

@positive
Scenario: Create a chat with a bot key
	Given a valid user
	And a valid room
	And a new chat data with bot key
	When create new chat
	Then chat bot should be retrieved

@positive
Scenario: Create multiple chats
	Given a valid user
	And a valid room
	And multiple chats data
	When create multiple chats
	Then multiple chats should be created

@positive
Scenario: Get a room with no more than limit chats to show
	Given a valid user
	And a valid room
	And more chats data than limit to show
	When get room detail
	Then room chat should be equal to limit to show

@negative
Scenario: Create a chat with invalid message
	Given a valid user
	And a valid room
	And a new chat with invalid name
	When create new chat
	Then chat should not be created with invalid name error

@negative
Scenario: Create a chat with message lenght exceeds characters
	Given a valid user
	And a valid room
	And a new chat with long name
	When create new chat
	Then chat should not be created with message lenght exceeds characters
