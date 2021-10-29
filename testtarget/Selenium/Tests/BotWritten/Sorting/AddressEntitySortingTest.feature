###
# @bot-written
#
# WARNING AND NOTICE
# Any access, download, storage, and/or use of this source code is subject to the terms and conditions of the
# Full Software Licence as accepted by you before being granted access to this source code and other materials,
# the terms of which can be accessed on the Codebots website at https://codebots.com/full-software-licence. Any
# commercial use in contravention of the terms of the Full Software Licence may be pursued by Codebots through
# licence termination and further legal action, and be required to indemnify Codebots for any loss or damage,
# including interest and costs. You are deemed to have accepted the terms of the Full Software Licence on any
# access, download, storage, and/or use of this source code.
#
# BOT WARNING
# This file is bot-written.
# Any changes out side of "protected regions" will be lost next time the bot makes any changes.
###
# % protected region % [Override feature properties here] off begin
@sorting @BotWritten @ignore
# WARNING: These Tests have been flagged as unstable and have been ignored until they are updated.
# % protected region % [Override feature properties here] end

Feature: Sort AddressEntity
	@AddressEntity
	Scenario: Sort AddressEntity
	Given I login to the site as a user
	And I navigate to the AddressEntity backend page
	When I sort AddressEntity by Unit
	Then I assert that Unit in AddressEntity of type String is properly sorted in descending
	When I sort AddressEntity by Unit
	Then I assert that Unit in AddressEntity of type String is properly sorted in ascending
	When I sort AddressEntity by Address line 1
	Then I assert that Address line 1 in AddressEntity of type String is properly sorted in descending
	When I sort AddressEntity by Address line 1
	Then I assert that Address line 1 in AddressEntity of type String is properly sorted in ascending
	When I sort AddressEntity by Address line 2
	Then I assert that Address line 2 in AddressEntity of type String is properly sorted in descending
	When I sort AddressEntity by Address line 2
	Then I assert that Address line 2 in AddressEntity of type String is properly sorted in ascending
	When I sort AddressEntity by Suburb
	Then I assert that Suburb in AddressEntity of type String is properly sorted in descending
	When I sort AddressEntity by Suburb
	Then I assert that Suburb in AddressEntity of type String is properly sorted in ascending
	When I sort AddressEntity by Postcode
	Then I assert that Postcode in AddressEntity of type int is properly sorted in descending
	When I sort AddressEntity by Postcode
	Then I assert that Postcode in AddressEntity of type int is properly sorted in ascending
	When I sort AddressEntity by City
	Then I assert that City in AddressEntity of type String is properly sorted in descending
	When I sort AddressEntity by City
	Then I assert that City in AddressEntity of type String is properly sorted in ascending
	When I sort AddressEntity by Country
	Then I assert that Country in AddressEntity of type String is properly sorted in descending
	When I sort AddressEntity by Country
	Then I assert that Country in AddressEntity of type String is properly sorted in ascending

