# CSV Validator Change Log

## [current](https://github.com/barry-jones/csv-validator/)

* feat: Added `strictColumns` option to report rows (and the header) whose column count does not match the configured schema width [pr30](https://github.com/barry-jones/csv-validator/pull/30)
* fix: Console colour for success messages [pr](https://github.com/barry-jones/csv-validator/pull/17), closes [#16](https://github.com/barry-jones/csv-validator/issues/9)
* feat: Added non-zero exit code for validation failures [pr31](https://github.com/barry-jones/csv-validator/pull/31).
* feat: Added structured output to JSON or CSV with flags [pr32](https://github.com/barry-jones/csv-validator/pull/32).
* feat: allowedValues validator lets users limit values to a specific set [pr33](https://github.com/barry-jones/csv-validator/pull/33).
* feat: minLength validator lets users specify a minimum length for values [pr34](https://github.com/barry-jones/csv-validator/pull/34).

## [v1.0.4](https://github.com/barry-jones/csv-validator/releases/tag/v1.0.4)

* details added to nuget package

## [v1.0.3](https://github.com/barry-jones/csv-validator/releases/tag/v1.0.3)

* bugfix: Fixed issue with capture groups on Regex string split [pr13](https://github.com/barry-jones/csv-validator/pull/13), closes [#12](https://github.com/barry-jones/csv-validator/issues/12)

## [v1.0.2](https://github.com/barry-jones/csv-validator/releases/tag/v1.0.2)

* bugfix: Now handles quoted strings [pr11](https://github.com/barry-jones/csv-validator/pull/11), closes [#9](https://github.com/barry-jones/csv-validator/issues/9)
* bugfix: Added column number to error details [pr10](https://github.com/barry-jones/csv-validator/pull/10), closes [#7](https://github.com/barry-jones/
csv-validator/issues/7)
* bugfix: Added format to validation error messages [pr10](https://github.com/barry-jones/csv-validator/pull/10), closes [#8](https://github.com/barry-jones/csv-validator/issues/8)

## [v1.0.0](https://github.com/barry-jones/csv-validator/releases/tag/v1.0.0)

* feat: Added support to read direct from `Stream`
* feat: Set up as NuGet package