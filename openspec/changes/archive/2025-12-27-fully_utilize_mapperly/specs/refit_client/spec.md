## MODIFIED Requirements
### Requirement: Refit Interface
- The application MUST use `IMealieClient` interface decorated with Refit attributes for API communication.
- The interface MUST define methods for the following **Recipe** operations: List, Get, Create, Scrape (Create from URL), Update.
- The interface MUST define methods for the following **Food** operations: List, Get, Create, Update, Delete.
- The interface MUST define methods for the following **Tag** operations: List, Get, Create, Update, Delete.
#### Scenario: Retrieve Recipes
- Given the `IMealieClient` is configured
- When `GetRecipesAsync` is called with page and per_page
- Then it should return a paginated list of recipes.
