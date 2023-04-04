Feature: Home "Hello World" for SpecFlow and Selenium

As a visitor I would like to have a home page that tells me where I am

@static_content
Scenario: Home page title contains Standup Meetings
	Given I am a visitor
	When I am on the "Home" page
	Then The page title contains "Standup Meetings"

# Need to do this one after logging in to have any cookies
@support
Scenario: We can save cookies
	Given I am a visitor
	When I am on the "Home" page
	Then I can save cookies