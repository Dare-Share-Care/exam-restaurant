Feature: CreateMenuItem
	Function to add menu item to the restaurant menu

@CreateMenuItem
Scenario: Add two numbers
	Given a restaurant owner is logged into the system
	And a restaurant already exists in the system
	When the owner creates a new menu item,
	Then the system should save the new menu in the database.