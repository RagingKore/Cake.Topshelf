//using System;
//using System.Linq;
//using Cake.Common;
//using Cake.Common.Diagnostics;
//using Cake.Common.IO;
//using Cake.Core;
//using Cake.Core.Annotations;
//using Cake.Core.IO;
//using Cake.Core.IO.Arguments;

//namespace Cake.Topshelf
//{
//    public sealed class TopshelfHelperCakeCommonDependent
//    {
//        private readonly ICakeContext cake;

//        public static TopshelfHelper Using(ICakeContext cake)
//        {
//            return new TopshelfHelper(cake);
//        }

//        internal TopshelfHelper(ICakeContext cake)
//        {
//            this.cake = cake;
//        }

//        public void InstallService(string servicePath, TopshelfSettings settings)
//        {
//            this.cake.StartProcess(servicePath, new ProcessSettings
//            {
//                Arguments = "install " + (settings != null ? GetArguments(settings) : string.Empty)
//            });
//        }

//        public void UninstallService(string servicePath, string instance = null)
//        {
//            this.cake.StartProcess(servicePath, new ProcessSettings
//            {
//                Arguments = "uninstall " + (instance ?? string.Empty)
//            });
//        }

//        public void StartService(string servicePath, string instance = null)
//        {
//            this.cake.StartProcess(servicePath, new ProcessSettings
//            {
//                Arguments = "start " + (instance ?? string.Empty)
//            });
//        }

//        public void StopService(string servicePath, string instance = null)
//        {
//            this.cake.StartProcess(servicePath, new ProcessSettings
//            {
//                Arguments = "stop " + (instance ?? string.Empty)
//            });
//        }

//        public void DeployService(string sourcePath, string installationPath, TopshelfSettings settings)
//        {
//            // Uninstall Service
//            if (this.cake.DirectoryExists(installationPath))
//            {
//                var installedServiceExecutablePath = this.cake.GetFiles(installationPath + @"\*.exe").SingleOrDefault();

//                if (installedServiceExecutablePath != null)
//                {
//                    this.cake.Verbose("Service executable found: {0}", installedServiceExecutablePath);

//                    if (this.cake.StartProcess(installedServiceExecutablePath, new ProcessSettings { Arguments = "uninstall" }) != 0)
//                        throw new Exception("Failed to uninstall service!");
//                }
//                else this.cake.Information("Service executable not found. Skipping task...");
//            }
//            else this.cake.Information("Instalation folder not found. Skipping task...");

//            // Prepare Installation Folder
//            if (this.cake.DirectoryExists(installationPath))
//            {
//                this.cake.CleanDirectory(installationPath);
//                this.cake.Information("Installation folder cleaned.");
//            }
//            else
//            {
//                this.cake.CreateDirectory(installationPath);
//                this.cake.Information("Installation folder created.");
//            }

//            // Copy Files
//            this.cake.CopyFiles(sourcePath + "*.*", installationPath);
//            this.cake.Information("Files copied to installation folder.");

//            // Install Service
//            var serviceExecutablePath = this.cake.GetFiles(sourcePath + "*.exe").SingleOrDefault();

//            if (serviceExecutablePath == null)
//                throw new Exception("Failed to find service executable!");

//            if (this.cake.StartProcess(serviceExecutablePath, new ProcessSettings { Arguments = GetArguments(settings) }) < 0)
//                throw new Exception("Failed to install service!");
//        }

//        private string GetArguments(TopshelfSettings settings)
//        {
//            var builder = new ProcessArgumentBuilder();

//            if (!string.IsNullOrWhiteSpace(settings.Username))
//            {
//                builder.Append(new TextArgument("-username"));
//                builder.Append(new QuotedArgument(new TextArgument(settings.Username)));
//            }

//            if (settings.Password != null)
//            {
//                builder.Append(new TextArgument("-password"));
//                builder.Append(new QuotedArgument(new TextArgument(settings.Password)));
//            }

//            if(!string.IsNullOrWhiteSpace(settings.Instance))
//            {
//                builder.Append(new TextArgument("-instance"));
//                builder.Append(new QuotedArgument(new TextArgument(settings.Instance)));
//            }

//            if(settings.Autostart)
//                builder.Append(new TextArgument("--autostart"));
//            else
//                builder.Append(new TextArgument("--manual"));

//            if(settings.Disabled)
//                builder.Append(new TextArgument("--disabled"));

//            if(settings.Delayed)
//                builder.Append(new TextArgument("--delayed"));

//            if(settings.LocalSystem)
//                builder.Append(new TextArgument("--localsystem"));

//            if(settings.LocalService)
//                builder.Append(new TextArgument("--localservice"));

//            if(settings.NetworkService)
//                builder.Append(new TextArgument("--networkservice"));

//            if(string.IsNullOrWhiteSpace(settings.ServiceName))
//            {
//                builder.Append(new TextArgument("--servicename"));
//                builder.Append(new QuotedArgument(new TextArgument(settings.Description)));
//            }

//            if(string.IsNullOrWhiteSpace(settings.Description))
//            {
//                builder.Append(new TextArgument("--description"));
//                builder.Append(new QuotedArgument(new TextArgument(settings.Description)));
//            }

//            if(string.IsNullOrWhiteSpace(settings.DisplayName))
//            {
//                builder.Append(new TextArgument("--displayname"));
//                builder.Append(new QuotedArgument(new TextArgument(settings.DisplayName)));
//            }

//            return builder.Render();
//        }
//    }
//}
