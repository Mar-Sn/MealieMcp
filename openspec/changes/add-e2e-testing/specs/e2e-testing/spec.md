## ADDED Requirements
### Requirement: Automated E2E Testing
The system SHALL provide an automated End-to-End test suite that validates the integration between the MCP server and a real Mealie instance.

#### Scenario: Verify Tool Discovery
- **WHEN** the MCP server is started against a running Mealie instance
- **AND** a `tools/list` request is sent
- **THEN** the server returns a list of available tools including `get_recipe` and `list_recipes`

#### Scenario: Verify Recipe Retrieval
- **WHEN** the Mealie instance is seeded with a recipe
- **AND** a `tools/call` request is sent for `get_recipe` with the valid slug
- **THEN** the server returns the recipe details in the response

#### Scenario: Verify Recipe Creation
- **WHEN** a `tools/call` request is sent for `create_recipe` with name, description, and initial settings
- **AND** subsequent requests are made to `update_recipe_ingredients` and `update_recipe_instructions`
- **THEN** the recipe exists in Mealie with the correct structure (verified via snapshot)

#### Scenario: Verify Recipe Editing
- **WHEN** an existing recipe is modified using `update_recipe`, `update_recipe_ingredients`, or `update_recipe_instructions`
- **THEN** the changes are reflected in the retrieved recipe details
