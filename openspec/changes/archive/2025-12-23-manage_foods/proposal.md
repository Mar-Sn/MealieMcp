# Change: Manage Foods

## Why
Users need to be able to manage the ingredients database (foods) in Mealie directly through the MCP server. This includes listing, creating, updating, and deleting foods. This is essential for maintaining a clean and accurate ingredient list for recipes.

## What Changes
- Add `food_management` capability.
- Implement `list_foods` tool.
- Implement `get_food` tool.
- Implement `create_food` tool.
- Implement `update_food` tool.
- Implement `delete_food` tool.

## Impact
- **Affected specs:** `food_management` (new)
- **Affected code:**
    - `MealieMcp/Tools/FoodTools.cs` (new)
    - `MealieMcp/Program.cs` (register new tools)
