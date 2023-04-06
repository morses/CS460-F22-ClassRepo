@Scot
Feature: Home "Hello World" for SpecFlow and Selenium

A short summary of the feature


Scenario: Home page title contains Standup Meetings
	Given I am a visitor
	When I am on the "Home" page
	Then The page title contains "Standup Meetings"

Scenario: Home page has a Register button
	Given I am a visitor
	When I am on the "Home" page
	Then The page presents a Register button
