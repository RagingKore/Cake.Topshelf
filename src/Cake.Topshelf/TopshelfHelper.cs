using System.IO;
using System.Linq;
using Cake.Common.Diagnostics;
using Cake.Common.IO;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.IO.Arguments;

namespace Cake.Topshelf
{
    public sealed class TopshelfHelper
    {
        private const int DefaultProcessTimeout = 10000;

        private readonly ICakeContext cake;

        public TopshelfHelper(ICakeContext cake)
        {
            this.cake = cake;
        }

        public static TopshelfHelper Using(ICakeContext cake)
        {
            return new TopshelfHelper(cake);
        }

        public void DeployService(string sourcePath, string deploymentPath, TopshelfSettings settings = null)
        {
            // Uninstall service
            this.cake.Debug("Uninstalling service...");

            if(this.cake.DirectoryExists(deploymentPath))
            {
                var installedServiceExecutablePath = this.FindFile(deploymentPath, "*.exe");

                if(installedServiceExecutablePath != null)
                {
                    this.cake.Verbose("Service executable found: {0}", installedServiceExecutablePath);
                    this.StopService(installedServiceExecutablePath, settings != null ? settings.Instance : null);
                    this.UninstallService(installedServiceExecutablePath, settings != null ? settings.Instance : null);
                }
                else this.cake.Information("Service executable not found. Skipping uninstall...");
            }
            else this.cake.Information("Instalation folder not found. Skipping uninstall...");

            // Prepare deployment folder
            this.cake.Debug("Preparing deployment folder...");

            if(this.cake.DirectoryExists(deploymentPath))
            {
                this.cake.CleanDirectory(deploymentPath);
                this.cake.Information("Deployment folder cleaned.");
            }
            else
            {
                this.cake.CreateDirectory(deploymentPath);
                this.cake.Information("Deployment folder created.");
            }

            // Copy Files
            this.cake.Debug("Copying files to deployment folder...");
            this.cake.CopyFiles(sourcePath + "/*.*", deploymentPath);
            this.cake.Information("Files copied to deployment folder.");

            // Install Service
            this.cake.Debug("Installing service...");

            var serviceExecutablePath = this.FindFile(deploymentPath, "*.exe");

            if(serviceExecutablePath == null)
                throw new FileNotFoundException("Failed to find service executable!");

            this.cake.Verbose("Service executable found: {0}", serviceExecutablePath);

            this.InstallService(serviceExecutablePath, settings);
        }

        public void InstallService(string serviceExecutablePath, TopshelfSettings settings)
        {
            this.cake.ProcessRunner.Start(
                serviceExecutablePath, 
                new ProcessSettings
                {
                    Timeout   = DefaultProcessTimeout, 
                    Arguments = "install " + (settings != null
                        ? this.GetArguments(settings)
                        : string.Empty)
                });
        }

        public void StartService(string serviceExecutablePath, string instance = null)
        {
            this.cake.ProcessRunner.Start(
                serviceExecutablePath, 
                new ProcessSettings
                {
                    Timeout   = DefaultProcessTimeout, 
                    Arguments = "start " + (instance ?? string.Empty)
                });
        }

        public void StopService(string serviceExecutablePath, string instance = null)
        {
            this.cake.ProcessRunner.Start(
                serviceExecutablePath, 
                new ProcessSettings
                {
                    Timeout   = DefaultProcessTimeout, 
                    Arguments = "stop " + (instance ?? string.Empty)
                });
        }

        public void UninstallService(string serviceExecutablePath, string instance = null)
        {
            this.cake.ProcessRunner.Start(
                serviceExecutablePath, 
                new ProcessSettings
                {
                    Timeout   = DefaultProcessTimeout, 
                    Arguments = "uninstall " + (instance ?? string.Empty)
                });
        }

        private string GetArguments(TopshelfSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            if(!string.IsNullOrWhiteSpace(settings.Username))
            {
                builder.Append(new TextArgument("-username"));
                builder.Append(new QuotedArgument(new TextArgument(settings.Username)));
            }

            if(!string.IsNullOrWhiteSpace(settings.Password))
            {
                builder.Append(new TextArgument("-password"));
                builder.Append(new QuotedArgument(new TextArgument(settings.Password)));
            }

            if(!string.IsNullOrWhiteSpace(settings.Instance))
            {
                builder.Append(new TextArgument("-instance"));
                builder.Append(new QuotedArgument(new TextArgument(settings.Instance)));
            }

            builder.Append(
                settings.Autostart
                    ? new TextArgument("--autostart")
                    : new TextArgument("--manual"));

            if(settings.Disabled)
                builder.Append(new TextArgument("--disabled"));

            if(settings.Delayed)
                builder.Append(new TextArgument("--delayed"));

            if(settings.LocalSystem)
                builder.Append(new TextArgument("--localsystem"));

            if(settings.LocalService)
                builder.Append(new TextArgument("--localservice"));

            if(settings.NetworkService)
                builder.Append(new TextArgument("--networkservice"));

            if(!string.IsNullOrWhiteSpace(settings.ServiceName))
            {
                builder.Append(new TextArgument("--servicename"));
                builder.Append(new QuotedArgument(new TextArgument(settings.Description)));
            }

            if(!string.IsNullOrWhiteSpace(settings.Description))
            {
                builder.Append(new TextArgument("--description"));
                builder.Append(new QuotedArgument(new TextArgument(settings.Description)));
            }

            if(!string.IsNullOrWhiteSpace(settings.DisplayName))
            {
                builder.Append(new TextArgument("--displayname"));
                builder.Append(new QuotedArgument(new TextArgument(settings.DisplayName)));
            }

            return builder.Render();
        }

        private string FindFile(string path, string searchPattern)
        {
            return Directory
                .EnumerateFileSystemEntries(path, searchPattern)
                .SingleOrDefault();
        }
    }
}