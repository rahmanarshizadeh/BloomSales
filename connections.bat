@ECHO OFF
SETLOCAL

SET pathArg=%~1

IF NOT "%pathArg%" == "" (
	SET dataDir=%pathArg%
) ELSE (
	SET dataDir=%cd%\App Data
)

ECHO.

ECHO Checking if the destination directory '%dataDir%' exists...

IF EXIST "%dataDir%" (
	ECHO Directory exists.
) ELSE (
	ECHO Creating '%dataDir%'...
	mkdir "%dataDir%"
	ECHO Directory created successfully.
)

ECHO Generating 'connections.config' file ...

ECHO ^<connectionStrings^> > connections.config
ECHO     ^<add name="InventoryDatabase" connectionString="Data Source=(LocalDb)\MSSQLLocalDb;AttachDbFilename=%dataDir%\InventoryDatabase.mdf;Initial Catalog=InventoryDatabase;Integrated Security=True" providerName="System.Data.SqlClient" /^> >> connections.config
ECHO     ^<add name="AccountingDatabase" connectionString="Data Source=(LocalDb)\MSSQLLocalDb;AttachDbFilename=%dataDir%\AccountingDatabase.mdf;Initial Catalog=AccountingDatabase;Integrated Security=True" providerName="System.Data.SqlClient" /^> >> connections.config
ECHO     ^<add name="LocationDatabase" connectionString="Data Source=(LocalDb)\MSSQLLocalDb;AttachDbFilename=%dataDir%\LocationDatabase.mdf;Initial Catalog=LocationDatabase;Integrated Security=True" providerName="System.Data.SqlClient" /^> >> connections.config
ECHO     ^<add name="OrderDatabase" connectionString="Data Source=(LocalDb)\MSSQLLocalDb;AttachDbFilename=%dataDir%\OrderDatabase.mdf;Initial Catalog=OrderDatabase;Integrated Security=True" providerName="System.Data.SqlClient" /^> >> connections.config
ECHO    ^<add name="ShippingDatabase" connectionString="Data Source=(LocalDb)\MSSQLLocalDb;AttachDbFilename=%dataDir%\ShippingDatabase.mdf;Initial Catalog=ShippingDatabase;Integrated Security=True" providerName="System.Data.SqlClient" /^> >> connections.config
ECHO ^</connectionStrings^> >> connections.config

ECHO 'connections.config' created successfully.
