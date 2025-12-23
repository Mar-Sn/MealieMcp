# tag_management Specification

## Purpose
TBD - created by archiving change manage_tags. Update Purpose after archive.
## Requirements
### Requirement: List Tags Tool
The server MUST provide a `list_tags` tool to retrieve a list of tags from Mealie.

#### Scenario: Listing tags
- **WHEN** the `list_tags` tool is called with optional pagination parameters
- **THEN** the server makes a GET request to `/api/organizers/tags`
- **AND** returns a JSON list of tags including their ID, name, and slug

### Requirement: Get Tag Tool
The server MUST provide a `get_tag` tool to retrieve details of a specific tag.

#### Scenario: Getting a tag by ID
- **WHEN** the `get_tag` tool is called with `id="some-uuid"`
- **THEN** the server makes a GET request to `/api/organizers/tags/some-uuid`
- **AND** returns the full tag details

### Requirement: Create Tag Tool
The server MUST provide a `create_tag` tool to create a new tag.

#### Scenario: Creating a tag
- **WHEN** the `create_tag` tool is called with a name
- **THEN** the server makes a POST request to `/api/organizers/tags`
- **AND** returns the created tag's details

### Requirement: Update Tag Tool
The server MUST provide an `update_tag` tool to modify an existing tag.

#### Scenario: Updating a tag
- **WHEN** the `update_tag` tool is called with `id="some-uuid"` and new data (e.g., name)
- **THEN** the server makes a PUT request to `/api/organizers/tags/some-uuid`
- **AND** returns the updated tag details

### Requirement: Delete Tag Tool
The server MUST provide a `delete_tag` tool to remove a tag from the database.

#### Scenario: Deleting a tag
- **WHEN** the `delete_tag` tool is called with `id="some-uuid"`
- **THEN** the server makes a DELETE request to `/api/organizers/tags/some-uuid`
- **AND** returns a confirmation of deletion

