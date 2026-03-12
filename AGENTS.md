# Repository Guidelines

## Project structure & module organization
This project is starting as hand-made terminal user interface library. Eventually we will build a terminal based email client on top of it.

The solution file is `Howsit.sln`. Production code lives under `src/` with two projects: `src/Howsit.UI` for the terminal UI framework and `src/Howsit.App` for the executable host and demo runner. Tests live in `tests/Howsit.UI.Tests` and currently target the UI library. Treat `bin/` and `obj/` as generated output. Demo assets used by the app live in `src/Howsit.App/var/`.

## Build, test, and development commands
Use the .NET CLI from the repository root:

- `dotnet restore Howsit.sln` restores NuGet packages.
- `dotnet build Howsit.sln -nodeReuse:false -maxcpucount:1` builds all projects.
- `dotnet run --project src/Howsit.App/HowsIt.App.csproj` starts the terminal app.
- `dotnet test Howsit.sln -nodeReuse:false -maxcpucount:1` runs the xUnit suite.

The single-process flags are useful in constrained environments and still work fine locally.

## Agents role in this project
The role of agents working on this project is to provide architectural advice and guidance. You are to assist in technical planning. In general you will not be committing code to this project. You may be asked to assist in writing test cases or other boilerplate when needed. 

## Packages and external dependencies
This project aims to use a minimum of external dependencies. There may be exceptions to this rule, but in general you should not install external pacakges.

## Coding style & naming conventions
This repository is C# on `net10.0` with nullable reference types enabled and implicit usings disabled. Follow the existing style in `.editorconfig`: opening braces stay on the same line, and `else`, `catch`, and `finally` do not start on a new line. Use 4-space indentation, `PascalCase` for types and public members, `_camelCase` for private fields, and keep namespaces aligned to folder structure such as `Howsit.UI.Style`.

## Testing guidelines
Tests use xUnit with files named after the subject under test, for example `RendererTests.cs` and `CellTests.cs`. Prefer focused `[Fact]` tests with descriptive method names such as `IdenticalBuffersDiffIsEmpty`. Add tests for renderer diffs, buffer sizing, and edge cases whenever behavior changes. Coverage is not currently enforced, but new logic should ship with matching tests.

Tests that fail for legitimate reasons are good. Do not force tests to pass to satisfy a need to show "all green". If there are failing tests and you can spot the reason that they are failing, you can make suggestions on how what to do to fix the issue.

## Commit & pull request guidelines
Recent commits use short, imperative, lowercase summaries like `add basic renderer` and `fix timing bugs in demo runner`. Keep commit messages specific and under a single sentence. Pull requests should explain the behavior change, note how it was validated, and include terminal output or screenshots when UI rendering changes are visible.

## Configuration notes
Do not commit secrets or machine-specific settings. If you add local-only data for demos or experiments, keep it under ignored output directories or document it clearly in the PR.
