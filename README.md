## How to run Hotels app - Option 1
1. Download the binaries for the last release: https://github.com/artzie92/hotels/releases
2. Run `dotnet Hotels.Console.dll` command 

## How to run Hotels app - Option 2

1. Navigate to `src/App/Hotels.Console`
2. Run `dotnet build` command
3. Then you can runn application with `dotnet run` command

## Command line parameters
If you run application with `dotnet run` command, default hotels and bookings files will be loaded. You can find them in `Storage` directory. 
If you want to use your own data files you can pass them with command line arguments.

### Running with arguments
`dotnet run 
-- bookings <<path to your file>>
-- hotels <<path to your file>>`



