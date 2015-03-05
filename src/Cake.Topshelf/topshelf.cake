
///////////////////////////////////////////////////////////////////////////////
// TOPSHELF INSTALLER ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

//-username         The username to run the service
//-password         The password for the specified username
//-instance         An instance name if registering the service multiple times
//--autostart       The service should start automatically (default)
//--disabled        The service should be set to disabled
//--manual          The service should be started manually
//--delayed         The service should start automatically (delayed)
//--localsystem     Run the service with the local system account
//--localservice    Run the service with the local service account
//--networkservice  Run the service with the network service permission
//--interactive     The service will prompt the user at installation for the service credentials
//--sudo            Prompts for UAC if running on Vista/W7/2008
//-servicename      The name that the service should use when installing
//-description      The service description the service should use when installing
//-displayname      The display name the the service should use when installing

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var configuration   = Argument<string>("configuration", "Release");
var sourcePath      = Argument<string>("sourcepath");
var installPath     = Argument<string>("installpath");
var arguments       = Argument<string>("arguments", "--autostart --localservice");

///////////////////////////////////////////////////////////////////////////////
// TASK DEFINITIONS
///////////////////////////////////////////////////////////////////////////////

Task("Uninstall-Service")
    .WithCriteria(
        HasArgument("sourcepath") && 
        HasArgument("installpath"))
    .Does(() =>
{
    Verbose("Arguments");
    Verbose(" -Source Path:     {0}", sourcePath);
    Verbose(" -Install Path:    {0}", installPath);
    Verbose(" -Arguments Path:  {0}", arguments);

    if(DirectoryExists(installPath))
    {
        var installedServiceExecutablePath = GetFiles(installPath + @"\*.exe").SingleOrDefault();

        if (installedServiceExecutablePath != null) 
        {
            Verbose("Service executable found: {0}", installedServiceExecutablePath);

            if(StartProcess(installedServiceExecutablePath, new ProcessSettings{ Arguments = "uninstall" }) != 0)
                throw new Exception("Failed to uninstall service!");
        }
        else Information("Service executable not found. Skipping task...");    
    }
    else Information("Instalation folder not found. Skipping task...");      
});

Task("Prepare-Installation-Folder")
    .IsDependentOn("Uninstall-Service")
    .Does(() =>
{
    if (DirectoryExists(installPath)) 
    {
        CleanDirectory(installPath);
        Information("Installation folder cleaned.");
    }
    else 
    {
        CreateDirectory(installPath);
        Information("Installation folder created.");  
    }
});

Task("Copy-Files")
    .IsDependentOn("Prepare-Installation-Folder")
    .Does(() =>
{
    CopyFiles(sourcePath + "*.*", installPath);
    Information("Files copied to installation folder.");
});

Task("Install-Service")
    .IsDependentOn("Copy-Files")
    .Does(() =>
{
    var serviceExecutablePath = GetFiles(sourcePath + "*.exe").SingleOrDefault();

    if (serviceExecutablePath == null) 
        throw new Exception("Failed to find service executable!");

    if(StartProcess(serviceExecutablePath, new ProcessSettings{ Arguments = "install " + arguments }) < 0)
        throw new Exception("Failed to install service!");
});

///////////////////////////////////////////////////////////////////////////////
// EXECUTION
///////////////////////////////////////////////////////////////////////////////

RunTarget("Install-Service");