# Flight Price Checker

(Work in progress) 
Periodically checks the price of specified flights, and emails you when they are below a threshold.

## Reasons to avoid this program

- Unimplemented features
  - Doesn't email you
  - Only checks when you tell it to

## Dependencies

- C# runtime ([OS X](http://xamarin.com/platform), [Linux](http://www.mono-project.com/docs/getting-started/install/linux/))
- [JSON.NET](http://json.codeplex.com/documentation) (included)
- [QPX Express](https://developers.google.com/qpx-express/) API key

## Compiling

Put the API key in `FlightPriceChecker/QpxExpressApiKey.cs`.

**Windows, OS X:** Open the project file and press the compile button

**Linux:** Probably `$ xbuild FlightPriceChecker/FlightPriceChecker.csproj`

## Usage

I haven't decided yet.

## Author

[Kevin Chen](http://kevinchen.co)

