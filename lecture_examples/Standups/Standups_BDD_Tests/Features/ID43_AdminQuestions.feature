@Scot
Feature: ID43_AdminQuestions

As an admin I would like to be able to ask students questions so that I can get feedback on a course.

This one assumes a single admin user.  It also needs students that already have a group set.  We can do this
in the seeding or we can ensure it in the Background (approach taken here).

Background:
	Given the following users exist
	  | UserName   | Email                 | FirstName | LastName | Password  |
	  | TaliaK     | knott@example.com     | Talia     | Knott    | Hello123# |
	And I am a user with first name 'Talia'
	And I login
	And I am on the "SelectGroup" page
	And I select a group
	And I logout

@admin
Scenario: Admin can create questions that students can see
	Given I am the admin
		And I login
		And I am on the "AdminQuestions" page
	When I click on Create New question
		And I create a new "active" question called "question"
		And I logout
		And I am a user with first name 'Talia'
		And I login
	Then I can see a link for "question" on the page
