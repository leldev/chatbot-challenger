Feature: ChatUser
As a chat user
I want to register, login and join room
So I can chat with other user in a room

@positive
Scenario: Create a chat user
	Given a new user data
	When create new user
	Then user should be created

@positive
Scenario: Get a chat user detail
	Given a valid user
	When get user detail
	Then user should be retrieved

@positive
Scenario: Delete a chat user detail
	Given a valid user
	When delete user
	Then user should be deleted

@negative
Scenario: Create a chat user with invalid name
	Given a new user with invalid name
	When create new user
	Then user should not be created with invalid name error

@negative
Scenario: Create a chat user with name lenght exceeds characters
	Given a new user with long name
	When create new user
	Then user should not be created with name lenght exceeds characters

@negative
Scenario: Create a chat user with invalid password
	Given a new user with invalid password
	When create new user
	Then user should not be created with invalid password error

@negative
Scenario: Create a chat user with password lenght exceeds characters
	Given a new user with long password
	When create new user
	Then user should not be created with password lenght exceeds characters