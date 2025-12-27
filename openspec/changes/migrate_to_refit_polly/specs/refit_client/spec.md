# Spec: Refit Client Migration

## ADDED Requirements

### Requirement: Refit Interface
- The application MUST use `IMealieClient` interface decorated with Refit attributes for API communication.
- The interface MUST define methods for the following **Recipe** operations: List, Get, Create, Scrape (Create from URL), Update.
- The interface MUST define methods for the following **Food** operations: List, Get, Create, Update, Delete.
- The interface MUST define methods for the following **Tag** operations: List, Get, Create, Update, Delete.
#### Scenario: Retrieve Recipes
- Given the `IMealieClient` is configured
- When `GetRecipesAsync` is called with page and per_page
- Then it should return a paginated list of recipes.

### Requirement: Polly Resilience
- The HTTP client MUST implement a retry policy for transient errors (HTTP 5xx, 408).
- The retry policy MUST use exponential backoff.
#### Scenario: Transient Failure
- Given the API endpoint is temporarily unavailable (503)
- When a request is made
- Then the client should retry the request automatically.

## REMOVED Requirements

### Requirement: Kiota Client
- The application MUST NOT use `MealieClient` (Kiota generated).
- The build process MUST NOT generate the client code using Kiota.
#### Scenario: Build Verification
- Given the source code
- When the build process runs
- Then it should not attempt to run Kiota generation.
