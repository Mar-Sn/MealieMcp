# MealieMcp

## Purpose
MealieMcp is a Model Context Protocol (MCP) server designed to control and interact with Mealie, a self-hosted recipe management application. The goal is to provide an intuitive interface for AI agents to manage recipes, meal plans, and shopping lists within Mealie.

## Tech Stack
- **Language:** C# (.NET)
- **Protocol:** Model Context Protocol (MCP)
- **Target Application:** Mealie (Recipe Management)
- **Containerization:** Docker
- **CI/CD:** GitHub Actions (for building Docker images)

## Project Conventions

### Code Style
- **Priority:** Readability and Clean Code are the absolute highest priorities.
- **Philosophy:** Code must be easily readable by humans. Optimization and performance are secondary to clarity and maintainability.
- **Clean Code:** Adhere to clean code principles (meaningful names, small functions, single responsibility).

### Git Workflow
- **Hosting:** GitHub
- **Artifacts:** Docker images built via GitHub Actions.

## Domain Context
Mealie is an intuitive recipe management application. This MCP server acts as a bridge, allowing LLMs to perform actions such as searching for recipes, adding new ones, or managing meal plans via the Mealie API.

## Configuration Requirements
- The server requires configuration to connect to a specific Mealie instance:
  - **Base URL:** The URL of the Mealie instance.
  - **API Token:** Authentication credentials.

## Important Constraints
- **Readability over Performance:** Optimization should not compromise code clarity.
