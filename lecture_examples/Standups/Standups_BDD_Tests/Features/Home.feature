@Scot
Feature: Home "Hello World" for SpecFlow and Selenium

As a visitor I would like to have a home page that tells me where I am

@static_content
Scenario: Home page title contains Standup Meetings
	Given I am a visitor
	When I am on the "Home" page
	Then The page title contains "Standup Meetings"

Scenario: Home page has a Register button
	Given I am a visitor
	When I am on the "Home" page
	Then The page presents a Register button

