# CSV Validator

A configurable validator for column-separated text files (CSV-like). Available as a CLI (`validate`) and a NuGet library (`CsvValidator`). See `README.md` for end-user usage and the config format.

## Solution layout

- **`src/Validate.Lib`** — the validation library. Targets **netstandard2.0**; packaged as NuGet `CsvValidator`. All core logic lives here.
- **`src/Validate`** — the CLI executable (assembly `validate`). Targets **net10.0**. A thin wrapper over the library; owns argument parsing, console output, and the process exit code.
- **`test/ValidateTests`** — MSTest tests + fixtures under `Data/`. Targets **net10.0**.

Keep library code netstandard2.0-compatible; only the CLI and tests get net10.0 features.

## Build & test

```
dotnet test FormatValidator.sln
```

.NET 10 SDK. **Run the full suite before and after any change — the baseline must stay green.**

## Architecture — the model to hold before changing anything

Config-driven pipeline: JSON config → `ValidatorConfiguration` (via `JsonReader`/Newtonsoft) → `ConfigurationConvertor` → `ConvertedValidators` → `Validator`.

- **Validation is cell-scoped and column-indexed.** Each `IValidator.IsValid(string)` checks one column's value. Validators are grouped per column index in `RowValidator._columns` (a `ValidatorGroup[]` sized to the **highest configured column index**).
- `RowValidator.IsValid(row)` splits the row (`ColumnSplitter`) and runs the per-column validators. `Validator.Validate(reader)` streams **row-by-row** (`IEnumerable`, no whole-file buffering) and excludes the header row from content validation when `hasHeaderRow` is set.
- **Expected column width** (strict mode) = `_columns.Length` = the highest configured column index. This is the single source of truth — do **not** re-derive width from config dictionary order.
- **Error model:** `RowValidationError` (`Row`, `Content`, `Errors[]`) aggregates `ValidationError` (`Column`, `AtCharacter`, `Message`). Both are **1-based**; a row-level error with no specific cell (e.g. column-count) uses `Column`/`AtCharacter` = `0`.
- **CLI:** `Program.Main` returns an `int` exit code; `Run` returns `0` (pass) / `1` (fail), derived from the same `errors.Count` signal the UI uses for PASSED/FAILED. Arg-parse failure → non-zero. Human output goes through `ConsoleUserInterface : IUserInterface`; the **exit code is the machine signal**.

## Conventions (enforced by review — keep to them)

- **Comments:** XML `///` doc comments on **public API only**. On internal/private members use plain `//` or none — do **not** add XML docs to non-public members.
- **Surface:** internal-by-default; keep new types and members as tightly scoped as possible. To test internals, use `[assembly: InternalsVisibleTo("ValidateTests")]` (already present on both `Validate.Lib` and `Validate`) — do **not** widen something to `public` just to test it.
- **Abstraction:** prefer a little duplication over a premature abstraction (rule of three). Don't grow a class's API to remove a small or one-off duplication.
- **Single source of truth** for any value that must stay consistent across code paths (see expected-width above).
- **Tests:** MSTest (`[TestClass]`/`[TestMethod]`); match the style in `test/ValidateTests` (e.g. `Integration/FunctionalTests.cs`). Fixtures live under `test/ValidateTests/Data` and are registered as `<Content>` (copied to output) in `ValidateTests.csproj` — **new fixture files must be added there**. For small cases prefer inline strings fed through `StreamSourceReader`(over a `MemoryStream`) to avoid fixture churn.

## Gotchas

- **Fixture line endings:** configs use `"rowSeperator": "\r\n"`, so fixture CSV files must be **CRLF** — an LF file parses as a single row.
- **Dictionary key order:** `ValidatorConfiguration.Columns` / `ConvertedValidators.Columns` are `Dictionary<int,...>`. `.Keys.Last()` returns enumeration (insertion) order, **not** the maximum key — use the highest index / `_columns.Length`, never `Keys.Last()`, for width.
