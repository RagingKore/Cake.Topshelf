using Cake.Core;
using Cake.Core.IO;
using Cake.Core.IO.Arguments;

namespace Cake.Topshelf
{
    public sealed class TopshelfHelper
    {
        private readonly ICakeContext cake;

        public static TopshelfHelper Using(ICakeContext cake)
        {
            return new TopshelfHelper(cake);
        }

        internal TopshelfHelper(ICakeContext cake)
        {
            this.cake = cake;
        }

        public void InstallService(string serviceExecutablePath, TopshelfSettings settings)
        {
            this.cake.ProcessRunner.Start(serviceExecutablePath, new ProcessSettings
            {
                Arguments = "install " + (settings != null ? GetArguments(settings) : string.Empty)
            });
        }

        public void UninstallService(string serviceExecutablePath, string instance = null)
        {
            this.cake.ProcessRunner.Start(serviceExecutablePath, new ProcessSettings
            {
                Arguments = "uninstall " + (instance ?? string.Empty)
            });
        }

        public void StartService(string serviceExecutablePath, string instance = null)
        {
            this.cake.ProcessRunner.Start(serviceExecutablePath, new ProcessSettings
            {
                Arguments = "start " + (instance ?? string.Empty)
            });
        }

        public void StopService(string serviceExecutablePath, string instance = null)
        {
            this.cake.ProcessRunner.Start(serviceExecutablePath, new ProcessSettings
            {
                Arguments = "stop " + (instance ?? string.Empty)
            });
        }

        private string GetArguments(TopshelfSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            if (!string.IsNullOrWhiteSpace(settings.Username))
            {
                builder.Append(new TextArgument("-username"));
                builder.Append(new QuotedArgument(new TextArgument(settings.Username)));
            }

            if (settings.Password != null)
            {
                builder.Append(new TextArgument("-password"));
                builder.Append(new QuotedArgument(new TextArgument(settings.Password)));
            }

            if(!string.IsNullOrWhiteSpace(settings.Instance))
            {
                builder.Append(new TextArgument("-instance"));
                builder.Append(new QuotedArgument(new TextArgument(settings.Instance)));
            }

            if(settings.Autostart)
                builder.Append(new TextArgument("--autostart"));
            else
                builder.Append(new TextArgument("--manual"));

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

            if(string.IsNullOrWhiteSpace(settings.ServiceName))
            {
                builder.Append(new TextArgument("--servicename"));
                builder.Append(new QuotedArgument(new TextArgument(settings.Description)));
            }

            if(string.IsNullOrWhiteSpace(settings.Description))
            {
                builder.Append(new TextArgument("--description"));
                builder.Append(new QuotedArgument(new TextArgument(settings.Description)));
            }

            if(string.IsNullOrWhiteSpace(settings.DisplayName))
            {
                builder.Append(new TextArgument("--displayname"));
                builder.Append(new QuotedArgument(new TextArgument(settings.DisplayName)));
            }

            return builder.Render();
        }
    }
}