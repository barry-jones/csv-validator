# CSV Validator

[![](https://img.shields.io/github/release/barry-jones/csv-validator.svg)](https://github.com/barry-jones/csv-validator/releases/tag/v1.0.4)
[![Version](https://img.shields.io/nuget/vpre/CsvValidator.svg)](https://www.nuget.org/packages/CsvValidator)

.NET Core CSV text file validator. Enables the quick verification of column separated data files. Columns can be checked against multiple requirements for correctness.

This application is provided via a CLI and a NuGet package. Details for using both are provided below.

## CLI Usage

The application is command line based, and has two arguments:

``` bash
validate --file "input-datafile.csv" --with "configuration.json"
```

## Configuration

To configure the verification a JSON file is used with the following format:

``` json
{
	"rowSeperator": "\r\n",
	"columnSeperator": ",",
	"hasHeaderRow": true,
	"columns": {
		"1": {
			"name": "ID",
			"isRequired": true,
			"unique": true,
			"trim": true
		},
		"2": {
			"name": "DOB",
			"pattern": "^\\d\\d\\d\\d-\\d\\d-\\d\\d$"
		},
		"3": {
			"name": "NOTES",
			"maxLength": "250"
		}
	}
}
```

The `pattern` property uses regular expressions but it is important to escape the characters else the application will fail when reading the configuration file.

`rowSeperator` can be any number of characters, rows can also be separated by characters and do not need the new line characters to be available in the input file.

`columnSeperator` can be one or more characters.

The columns __require__ the number, which is the ordinal of the column in the input file, you do not need to specify all columns, only those that are to be validated.

### Supported validation

```
{
	// trim value before checking
	"trim": true|false,
    // validates the column has content
    "isRequired": true|false,
    // validates the content is unique in this column across the full file
    "unique": true|false,
    // validates a string against a regular expression
    "pattern": "regular expression string",
    // Maximum allowable length for a column
    "maxLength": "int",
    // Check if content is numerical
    "isNumeric": true|false
}
```

## API Usage

CSV Validator is also available as a NuGet package, to enable in application validation of text files. The API conforms to _netstandard 2.0_.

### Installation

``` cli
dotnet add package csvvalidator
```

``` xml
<ItemGroup>
  <PackageReference Include="csvvalidator" Version="1.0.1" />
</ItemGroup>
```

### Usage

``` csharp
Validator validator = Validator.FromJson(config);
RowValidationError[] errors = validator.Validate(inputStream);
```

__Dealing with validation errors.__

Errors are reported heirarchicly, by row and then columns.

``` csharp
foreach(RowValidationError current in errors) 
{
	// row errors provides details of the row number and the content
	Console.WriteLine($"Errors in row[{current.Row}]: {current.Content}");
	foreach(ValidationError error in current.Errors)
	{
		// all errors then that occur on that row are reported in the error collection
		Console.WriteLine($"{error.Message} at {error.AtCharacter}");
	}
}
```
