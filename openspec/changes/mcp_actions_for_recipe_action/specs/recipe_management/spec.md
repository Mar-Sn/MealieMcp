# Recipe Management Specification

## ADDED Requirements

### Requirement: Mealie API Configuration
The server MUST accept configuration for the Mealie instance URL and API token.

#### Scenario: Configuration via Environment Variables
- Given the server is started with environment variables `MEALIE_API_URL` and `MEALIE_API_TOKEN` set
- When a tool that requires API access is called
- Then the server uses these values to authenticate the request

### Requirement: List Recipes Tool
The server MUST provide a `list_recipes` tool to retrieve a list of recipes from Mealie.

#### Scenario: Listing recipes
- Given the user asks to "list recipes" or "find recipes"
- When the `list_recipes` tool is called with optional pagination parameters
- Then the server makes a GET request to `/api/recipes`
- And returns a JSON list of recipes

### Requirement: Get Recipe Tool
The server MUST provide a `get_recipe` tool to retrieve details of a specific recipe.

#### Scenario: Getting a recipe by slug
- Given the user asks for details of a recipe with slug "pancakes"
- When the `get_recipe` tool is called with `slug="pancakes"`
- Then the server makes a GET request to `/api/recipes/pancakes`
- And returns the full recipe details

### Requirement: Create Recipe Tool
The server MUST provide a `create_recipe` tool to create a new recipe from provided data.

#### Scenario: Creating a recipe
- Given the user provides recipe data (name, ingredients, etc.)
- When the `create_recipe` tool is called with this data
- Then the server makes a POST request to `/api/recipes`
- And returns the created recipe's details (including the new slug)

### Requirement: Create Recipe from URL Tool
The server MUST provide a `create_recipe_from_url` tool to import a recipe from a web URL.

#### Scenario: Importing from URL
- Given the user provides a URL to a recipe online
- When the `create_recipe_from_url` tool is called with `url="https://example.com/recipe"`
- Then the server makes a POST request to `/api/recipes/create/url`
- And returns the created recipe's details

### Requirement: Update Recipe Tool
The server MUST provide an `update_recipe` tool to modify an existing recipe.

#### Scenario: Updating a recipe
- Given the user wants to update the recipe with slug "pancakes"
- When the `update_recipe` tool is called with `slug="pancakes"` and the new data
- Then the server makes a PUT request to `/api/recipes/pancakes`
- And returns the updated recipe details
