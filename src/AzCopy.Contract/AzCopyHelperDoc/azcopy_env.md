## azcopy env

Shows the environment variables that you can use to configure the behavior of AzCopy.

### Synopsis

Shows the environment variables that you can use to configure the behavior of AzCopy.

If you set an environment variable by using the command line, that variable will be readable in your command line history. Consider clearing variables that contain credentials from your command line history.  To keep variables from appearing in your history, you can use a script to prompt the user for their credentials, and to set the environment variable.

```
azcopy env [flags]
```

### Options

```
  -h, --help             help for env
      --show-sensitive   Shows sensitive/secret environment variables.
```

### Options inherited from parent commands

```
      --cap-mbps float                      Caps the transfer rate, in megabits per second. Moment-by-moment throughput might vary slightly from the cap. If this option is set to zero, or it is omitted, the throughput isn't capped.
      --log-level string                    Define the log verbosity for the log file, available levels: INFO(all requests/responses), WARNING(slow responses), ERROR(only failed requests), and NONE(no output logs). (default 'INFO'). (default "INFO")
      --output-level string                 Define the output verbosity. Available levels: essential, quiet. (default "default")
      --output-type string                  Format of the command's output. The choices include: text, json. The default value is 'text'. (default "text")
      --trusted-microsoft-suffixes string   Specifies additional domain suffixes where Azure Active Directory login tokens may be sent.  The default is '*.core.windows.net;*.core.chinacloudapi.cn;*.core.cloudapi.de;*.core.usgovcloudapi.net;*.storage.azure.net'. Any listed here are added to the default. For security, you should only put Microsoft Azure domains here. Separate multiple entries with semi-colons.
```

### SEE ALSO

* [azcopy](azcopy.md)	 - AzCopy is a command line tool that moves data into and out of Azure Storage.

###### Auto generated by spf13/cobra on 15-Dec-2022
