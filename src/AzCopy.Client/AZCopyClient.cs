﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using AzCopy.Contract;
using Newtonsoft.Json;

namespace AzCopy.Client
{
    public class AZCopyClient : IAZCopyClient
    {
        private Process process = default;

        public event EventHandler<ListJobSummaryResponse> JobStatusMsgHandler;

        public event EventHandler<JsonOutputTemplate> OutputMsgHandler;

        public event EventHandler<InitMsgJsonTemplate> InitMsgHandler;

        public event EventHandler<JsonOutputTemplate> ErrorMsgHanlder;

        public event EventHandler<JsonOutputTemplate> InfoMsgHanlder;

        public async Task CopyAsync(LocationBase src, LocationBase dst, CopyOption option, CancellationToken ct = default)
        {
            option.OutputType = "json";
            var args = $"copy {src} {dst} {option} --cancel-from-stdin";
            await this.StartAZCopyAsync(args, ct);
        }

        public async Task RemoveAsync(LocationBase dst, RemoveOption option, CancellationToken ct = default)
        {
            // Lcations must be in quotes. It could have spaces in the name and the CLI would interpret as separate parameters.
            option.OutputType = "json";
            var args = $"rm \"{dst}\" {option} --output-type=json --cancel-from-stdin";
            await this.StartAZCopyAsync(args, ct);
        }

        public async Task JobsCleanAsync(JobsCleanOption option, CancellationToken ct)
        {
            option.OutputType = "json";
            var args = $"jobs clean {option} --cancel-from-stdin";
            await this.StartAZCopyAsync(args, ct);
        }

        public async Task JobsListAsync(JobsListOption option, CancellationToken ct)
        {
            option.OutputType = "json";
            var args = $"jobs list {option} --cancel-from-stdin";
            await this.StartAZCopyAsync(args, ct);
        }

        public async Task JobsRemoveAsync(string jobId, JobsRemoveOption option, CancellationToken ct)
        {
            option.OutputType = "json";
            var args = $"jobs remove {jobId} {option} --cancel-from-stdin";
            await this.StartAZCopyAsync(args, ct);
        }

        public async Task JobsResumeAsync(string jobId, JobsResumeOption option, CancellationToken ct)
        {
            option.OutputType = "json";
            var args = $"jobs resume {jobId} {option} --cancel-from-stdin";
            await this.StartAZCopyAsync(args, ct);
        }

        public async Task JobsShowAsync(string jobId, JobsShowOption option, CancellationToken ct)
        {
            option.OutputType = "json";
            var args = $"jobs show {jobId} {option} --cancel-from-stdin";
            await this.StartAZCopyAsync(args, ct);
        }

        public async Task BenchAsync(LocationBase destination, BenchOption option, CancellationToken ct)
        {
            option.OutputType = "json";
            var args = $"bench {destination} {option} --cancel-from-stdin";
            await this.StartAZCopyAsync(args, ct);
        }

        public async Task EnvAsync(EnvOption option, CancellationToken ct)
        {
            option.OutputType = "json";
            var args = $"env {option} --cancel-from-stdin";
            await this.StartAZCopyAsync(args, ct);
        }

        public async Task ListAsync(LocationBase location, ListOption option, CancellationToken ct)
        {
            option.OutputType = "json";
            var args = $"list {location} {option} --cancel-from-stdin";
            await this.StartAZCopyAsync(args, ct);
        }

        public async Task LoginAsync(LoginOption option, CancellationToken ct)
        {
            option.OutputType = "json";
            var args = $"login {option} --cancel-from-stdin";
            await this.StartAZCopyAsync(args, ct);
        }

        public async Task LogoutAsync(LogoutOption option, CancellationToken ct)
        {
            option.OutputType = "json";
            var args = $"logout {option} --cancel-from-stdin";
            await this.StartAZCopyAsync(args, ct);
        }

        public async Task MakeAsync(LocationBase dst, MakeOption option, CancellationToken ct)
        {
            option.OutputType = "json";
            var args = $"make {dst} {option} --cancel-from-stdin";
            await this.StartAZCopyAsync(args, ct);
        }

        public async Task SyncAsync(LocationBase src, LocationBase dst, SyncOption option, CancellationToken ct)
        {
            option.OutputType = "json";
            var args = $"sync {src} {dst} {option} --cancel-from-stdin";
            await this.StartAZCopyAsync(args, ct);
        }

        private static string GetAzCopyPath()
        {
            try
            {
                // First check if $env.AzCopyPath exists
                if (Environment.GetEnvironmentVariable("AZCOPYPATH") != null)
                {
                    return Environment.GetEnvironmentVariable("AZCOPYPATH");
                }

                var assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                string azCopyPath = Path.Combine(assemblyFolder, "azcopy");
                if (!File.Exists(azCopyPath))
                {
                    throw new FileNotFoundException();
                }

                return azCopyPath;
            }
            catch (FileNotFoundException)
            {
                throw new Exception(@"Can't find azcopy. Make sure you install azcopy and set its path to $AZCOPYPATH on your system, or use one of the following nuget package: AzCopy.WinX64, AzCopy.LinuxX64, AzCopy.OsxX64.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task StartAZCopyAsync(string args, CancellationToken ct = default, Dictionary<string, string> envs = default)
        {
            string azCopyPath = GetAzCopyPath();
            var procInfo = new ProcessStartInfo(azCopyPath);
            procInfo.Arguments = args;
            procInfo.RedirectStandardOutput = true;
            procInfo.RedirectStandardError = true;
            procInfo.RedirectStandardInput = true;

            if (envs != null)
            {
                foreach (var kv in envs)
                {
                    procInfo.Environment.Add(kv);
                }
            }

            // set Environment Info for OAuth Location
            // only for test output
            procInfo.UseShellExecute = false;
            await Task.Run(() =>
            {
                this.process = Process.Start(procInfo);

                // cancellation
                ct.Register(() => this.process.StandardInput.WriteLine("cancel"));

                this.process.OutputDataReceived += this.Process_OutputDataReceived;
                this.process.ErrorDataReceived += this.Process_OutputDataReceived;

                this.process.BeginOutputReadLine();
                this.process.BeginErrorReadLine();

                this.process.WaitForExit();
            });
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                var message = JsonConvert.DeserializeObject<JsonOutputTemplate>(e.Data);
                this.OutputMsgHandler?.Invoke(sender, message);

                switch (message.MessageType)
                {
                    case MessageType.Init:
                        var initMsg = JsonConvert.DeserializeObject<InitMsgJsonTemplate>(message.MessageContent);
                        this.InitMsgHandler?.Invoke(sender, initMsg);
                        break;
                    case MessageType.Error:
                        this.ErrorMsgHanlder?.Invoke(sender, message);
                        break;
                    case MessageType.Info:
                        this.InfoMsgHanlder?.Invoke(sender, message);
                        break;
                    default:
                        var statusMsg = JsonConvert.DeserializeObject<ListJobSummaryResponse>(message.MessageContent);
                        this.JobStatusMsgHandler?.Invoke(sender, statusMsg);
                        break;
                }
            }
        }
    }
}
