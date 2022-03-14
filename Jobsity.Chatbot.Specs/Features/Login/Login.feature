Feature: Login
As a chat user
I want to login
So I can chat with other user in a room

@positive
Scenario: Login with a chat user
	Given a valid user name and password
	When user login
	Then user should be logged

@negative
Scenario: Login with a invalid chat user
	Given a invalid user name and password
	When user login
	Then user should not be logged

@negative
Scenario: Login a chat user with invalid name
	Given a login user with invalid name
	When user login
	Then user login should not be created with invalid name error

@negative
Scenario: Login a chat user with name lenght exceeds characters
	Given a login user with long name
	When user login
	Then user login should not be created with name lenght exceeds characters

@negative
Scenario: Login a chat user with invalid password
	Given a login user with invalid password
	When user login
	Then user login should not be created with invalid password error

@negative
Scenario: Login a chat user with password lenght exceeds characters
	Given a login user with long password
	When user login
	Then user login should not be created with password lenght exceeds characters