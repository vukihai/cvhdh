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
@BotWritten @Filter
Feature: StaffEntity filtered by enum values
# % protected region % [Override feature properties here] end

@StaffEntity
Scenario: StaffEntity filtered by enum values
Given I login to the site as a user
Given I have 10 valid StaffEntity entities
And I have 1 valid StaffEntity entities with fixed string values BlaBla_StringToSearch_BlaBla
And I navigate to the StaffEntity backend page
When I click the filter Button on the StaffEntity page
Then The filter panel shows up with correct information
When I enter the enum filter role with the same value in the entity just created and click
Then The enum value created for Role is in each row of the the collection content
