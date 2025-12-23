## ADDED Requirements
### Requirement: List Foods Tool
The server MUST provide a `list_foods` tool to retrieve a list of foods (ingredients) from Mealie.

#### Scenario: Listing foods
- **WHEN** the `list_foods` tool is called with optional pagination parameters
- **THEN** the server makes a GET request to `/api/foods`
- **AND** returns a JSON list of foods including their ID, name, and description

### Requirement: Get Food Tool
The server MUST provide a `get_food` tool to retrieve details of a specific food.

#### Scenario: Getting a food by ID
- **WHEN** the `get_food` tool is called with `id="some-uuid"`
- **THEN** the server makes a GET request to `/api/foods/some-uuid`
- **AND** returns the full food details

### Requirement: Create Food Tool
The server MUST provide a `create_food` tool to create a new food.

#### Scenario: Creating a food
- **WHEN** the `create_food` tool is called with a name and optional description
- **THEN** the server makes a POST request to `/api/foods`
- **AND** returns the created food's details

### Requirement: Update Food Tool
The server MUST provide an `update_food` tool to modify an existing food.

#### Scenario: Updating a food
- **WHEN** the `update_food` tool is called with `id="some-uuid"` and new data (e.g., name, description)
- **THEN** the server makes a PUT request to `/api/foods/some-uuid`
- **AND** returns the updated food details

### Requirement: Delete Food Tool
The server MUST provide a `delete_food` tool to remove a food from the database.

#### Scenario: Deleting a food
- **WHEN** the `delete_food` tool is called with `id="some-uuid"`
- **THEN** the server makes a DELETE request to `/api/foods/some-uuid`
- **AND** returns a confirmation of deletion
