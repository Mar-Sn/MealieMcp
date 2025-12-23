# Change: Manage Tags

## Why
Users need to be able to manage tags in Mealie directly through the MCP server. This includes listing, creating, updating, and deleting tags. Tags are essential for organizing recipes.

## What Changes
- Add `tag_management` capability.
- Implement `list_tags` tool.
- Implement `get_tag` tool.
- Implement `create_tag` tool.
- Implement `update_tag` tool.
- Implement `delete_tag` tool.

## Impact
- **Affected specs:** `tag_management` (new)
- **Affected code:**
    - `MealieMcp/Tools/TagTools.cs` (new)
    - `MealieMcp/Program.cs` (register new tools)
