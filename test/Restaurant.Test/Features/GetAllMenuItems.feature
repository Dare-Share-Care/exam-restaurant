Feature: GetAllMenuItems
	Function to retrieve all menu items

@GetAllMenuItems
Scenario: A customer wants to see all menu items
	Given A restaurant exists in the system
	And Menu item exists in the system,
	And A customer is logged in,
	When the user accesses a given restaurant,
	Then they should see the list of all existing menu items.