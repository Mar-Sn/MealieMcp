## 1. Implementation
- [ ] 1.1 Create `MealieMcp.E2E` xUnit project
- [ ] 1.2 Add `.NET Aspire` testing dependencies (e.g. `Aspire.Hosting.Testing`)
- [ ] 1.3 Add `Verify.Xunit` dependency
- [ ] 1.3 Create `McpServerTestHarness` to manage the SUT (System Under Test) process and StdIO streams
- [ ] 1.5 Implement `ToolDiscovery` test (verify `tools/list` returns expected tools like `list_recipes`, `create_recipe`)
- [ ] 1.6 Implement `RecipeRetrieval` test (seed Mealie, call `tools/call` with `get_recipe`, verify response)
- [ ] 1.7 Implement `RecipeCreation` test (call `tools/call` with `create_recipe`, verify with `Verify`)
- [ ] 1.8 Implement `RecipeEditing` test (call `tools/call` with `update_recipe_ingredients`/`update_recipe_instructions`, verify persistence)
