# Flight Price Checker

(Work in progress) 
Periodically checks the price of specified flights, and emails you when they are below a threshold.

## Reasons to avoid this program

- Only checks when you tell it to (scheduling via cron jobs, lol)
- Not very easy to use

## Dependencies

- C# runtime ([OS X](http://xamarin.com/platform), [Linux](http://www.mono-project.com/docs/getting-started/install/linux/))
  - Mono runtimes do not trust any certificates by default. You need to [import Mozilla's root certificates](http://www.mono-project.com/docs/faq/security/#does-ssl-work-for-smtp-like-gmail) or set `mailer.acceptAnyCertificate` to `true` in the config file.
- [JSON.NET](http://json.codeplex.com/documentation) (included)
- [QPX Express](https://developers.google.com/qpx-express/) API key

## Compiling

Put the API key in `FlightPriceChecker/QpxExpressApiKey.cs`.

**Windows, OS X:** Open the project file and press the compile button

**Linux:** Probably `$ xbuild FlightPriceChecker/FlightPriceChecker.csproj`

## Usage

This is not final yet:

Pass in a configuration file as the first argument. An example is provided in `Config.json`. Please note:

- In `search`, the properties `origins`, `destinations`, and `carriers` must be IATA codes.
- If you set `mailer.encryption` to `true`, the SMTP server must support STARTTLS.

## Author

[Kevin Chen](http://kevinchen.co)
