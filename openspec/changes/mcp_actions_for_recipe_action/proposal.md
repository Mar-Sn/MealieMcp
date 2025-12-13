# Proposal: MCP Actions for Recipe Management

## Problem
The current MealieMcp server lacks the ability to interact with the Mealie API. Users want to be able to use AI agents to create, retrieve, and update recipes in Mealie, including creating recipes from URLs or images.

## Solution
Implement MCP tools that wrap the Mealie API endpoints for recipe management. This includes tools for:
- Listing recipes
- Getting a recipe by slug
- Creating a recipe (from raw data, URL, or image)
- Updating a recipe

## Scope
- Add an HTTP client to the MealieMcp server to communicate with the Mealie API.
- Implement authentication using an API token.
- Expose the following MCP tools:
  - `list_recipes`: Search/list recipes.
  - `get_recipe`: Get details of a single recipe.
  - `create_recipe`: Create a new recipe.
  - `create_recipe_from_url`: Import a recipe from a URL.
  - `update_recipe`: Update an existing recipe.
