using System;
using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.Topshelf
{
    [CakeAliasCategory("Topshelf")]
    public static class TopshelfExtensions
    {
        [CakeMethodAlias]
        public static void InstallService(this ICakeContext context, string serviceExecutablePath, TopshelfSettings settings)
        {
            if(string.IsNullOrWhiteSpace(serviceExecutablePath)) throw new ArgumentNullException("serviceExecutablePath");
            if(settings == null) throw new ArgumentNullException("settings");

            TopshelfHelper
                .Using(context)
                .InstallService(serviceExecutablePath, settings);
        }

        [CakeMethodAlias]
        public static void UninstallService(this ICakeContext context, string serviceExecutablePath)
        {
            if(string.IsNullOrWhiteSpace(serviceExecutablePath)) throw new ArgumentNullException("serviceExecutablePath");

            TopshelfHelper
                .Using(context)
                .UninstallService(serviceExecutablePath);
        }

        [CakeMethodAlias]
        public static void UninstallService(this ICakeContext context, string serviceExecutablePath, string instance)
        {
            if(string.IsNullOrWhiteSpace(serviceExecutablePath)) throw new ArgumentNullException("serviceExecutablePath");

            TopshelfHelper
                .Using(context)
                .UninstallService(serviceExecutablePath, instance);
        }

        [CakeMethodAlias]
        public static void StartService(this ICakeContext context, string serviceExecutablePath)
        {
            if(string.IsNullOrWhiteSpace(serviceExecutablePath)) throw new ArgumentNullException("serviceExecutablePath");

            TopshelfHelper
                .Using(context)
                .StartService(serviceExecutablePath);
        }

        [CakeMethodAlias]
        public static void StartService(this ICakeContext context, string serviceExecutablePath, string instance)
        {
            if(string.IsNullOrWhiteSpace(serviceExecutablePath)) throw new ArgumentNullException("serviceExecutablePath");

            TopshelfHelper
                .Using(context)
                .StartService(serviceExecutablePath, instance);
        }

        [CakeMethodAlias]
        public static void StopService(this ICakeContext context, string serviceExecutablePath)
        {
            if(string.IsNullOrWhiteSpace(serviceExecutablePath)) throw new ArgumentNullException("serviceExecutablePath");

            TopshelfHelper
                .Using(context)
                .StopService(serviceExecutablePath);
        }

        [CakeMethodAlias]
        public static void StopService(this ICakeContext context, string serviceExecutablePath, string instance)
        {
            if(string.IsNullOrWhiteSpace(serviceExecutablePath)) throw new ArgumentNullException("serviceExecutablePath");

            TopshelfHelper
                .Using(context)
                .StopService(serviceExecutablePath, instance);
        }

        [CakeMethodAlias]
        public static void DeployService(this ICakeContext context, string sourcePath, string deploymentPath)
        {
            if (string.IsNullOrWhiteSpace(sourcePath)) throw new ArgumentNullException("sourcePath");
            if (string.IsNullOrWhiteSpace(deploymentPath)) throw new ArgumentNullException("deploymentPath");

            TopshelfHelper
                .Using(context)
                .DeployService(sourcePath, deploymentPath);
        }

        [CakeMethodAlias]
        public static void DeployService(this ICakeContext context, string sourcePath, string deploymentPath, TopshelfSettings settings)
        {
            if(string.IsNullOrWhiteSpace(sourcePath)) throw new ArgumentNullException("sourcePath");
            if(string.IsNullOrWhiteSpace(deploymentPath)) throw new ArgumentNullException("deploymentPath");

            TopshelfHelper
                .Using(context)
                .DeployService(sourcePath, deploymentPath, settings);
        }
    }
}
