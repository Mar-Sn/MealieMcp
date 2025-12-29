# Change: Add E2E Testing Suite

## Why
Currently, there is no automated end-to-end verification of the MCP server. Testing relies on manual verification or unit tests that mock the backend. We need to ensure the MCP server correctly integrates with a real Mealie instance and adheres to the Model Context Protocol.

## What Changes
- Add a new test project `MealieMcp.E2E` using `xUnit`.
- Integrate `.NET Aspire` testing features (e.g. `Aspire.Hosting.Testing`) to spin up a disposable Mealie instance during tests.
- Integrate `Verify.Xunit` for snapshot testing of complex object responses.
- Implement a test harness to run `MealieMcp` as a subprocess and communicate via StdIO.
- Add E2E scenarios for:
    - Tool discovery (`list_recipes`, `get_recipe`, `create_recipe`, etc.)
    - Recipe retrieval (`get_recipe`)
    - Recipe creation (`create_recipe`, including ingredients and steps)
    - Recipe editing (`update_recipe`, `update_recipe_ingredients`, `update_recipe_instructions`)

## Impact
- **Affected specs**: `e2e-testing` (New Capability)
- **Affected code**: New project `MealieMcp.E2E`.
