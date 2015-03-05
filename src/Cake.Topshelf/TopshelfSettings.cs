namespace Cake.Topshelf
{
    public class TopshelfSettings
    {
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

        /// <summary>
        /// Gets or sets the username to run the service.
        /// </summary>
        ///
        /// <value>
        /// The username.
        /// </value>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password for the specified username.
        /// </summary>
        ///
        /// <value>
        /// The password.
        /// </value>
        public string Password { get; set; }

        public string Instance { get; set; }

        public bool Autostart { get; set; }

        public bool Disabled { get; set; }

        //public bool Manual { get; set; }

        public bool Delayed { get; set; }

        public bool LocalSystem { get; set; }

        public bool LocalService { get; set; }

        public bool NetworkService { get; set; }

        //public bool Interactive { get; set; }

        //public bool Sudo { get; set; }

        public string ServiceName { get; set; }

        public string Description { get; set; }

        public string DisplayName { get; set; }
    }
}