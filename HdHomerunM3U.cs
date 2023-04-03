using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace HdHomerunM3U
{
    class HdHomerunM3U
    {
        static void Main(string[] args)
        {
            // Get out of making everything static
            HdHomerunM3U p = new HdHomerunM3U();
            p.RealMain(args);
        }

        public void RealMain(string[] args)
        {
            bool doAll = false;
            int channelCount = 0;

            Console.WriteLine("HdHomerunM3U version " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
            Console.WriteLine("");

            string txtOutputFile =   Path.Combine(System.AppContext.BaseDirectory + "HdHomerun.m3u");
            string selectedDevice = string.Empty;

            // Read in params
            bool paramsOK = true;
            foreach (string arg in args)
            {
                if(arg.ToLower() == "-all")
                {
                    doAll = true;
                    Console.WriteLine("Processing all channels.");
                    continue;
                }

                if (arg.ToLower() == "-?" || arg.ToLower() == "-h" || arg.ToLower() == "-help")
                {
                    DisplayHelp();
                    return;
                }

                switch (arg.Substring(0, arg.IndexOf('=')).ToLower())
                {
                    case "-deviceid":
                        selectedDevice = arg.Substring(arg.IndexOf('=') + 1).Trim();
                        Console.WriteLine("DeviceID: " + selectedDevice);
                        break;

                    case "-out":
                        txtOutputFile = arg.Substring(arg.IndexOf('=') + 1).Trim();
                        break;

                    default:
                        paramsOK = false;
                        Console.WriteLine("Unknown parameter: " + arg);
                        break;
                }
            }

            if (paramsOK == false)
            {
                return;
            }

            Console.WriteLine("Out: " + txtOutputFile);
            Console.WriteLine("");

            List<HdHomerunDevice> deviceList = null;
            try
            {
                deviceList = HdHomerun.GetHdHomerunDeviceList();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception detecting HdHomeRun devices: " + e.Message);
                Console.WriteLine("Press <enter> to exit");
                Console.ReadLine();
                return;
            }

            if (deviceList == null)
            {
                Console.WriteLine("No HdHomeRun devices were detected on your network.");
                Console.WriteLine("Press <enter> to exit");
                Console.ReadLine();
                return;
            }

            try
            {
                foreach (HdHomerunDevice device in deviceList)
                {
                    if (!string.IsNullOrEmpty(selectedDevice))
                    {
                        if (!selectedDevice.Equals(device.DeviceID.Trim(), StringComparison.InvariantCultureIgnoreCase))
                            continue;
                    }

                    Console.WriteLine("Processing DeviceID: " + device.DeviceID);

                    HdHomerunDiscovery discovery = device.GetHdHomerunDiscovery();

                    List<HdHomerunChannel> channeList = discovery.GetHdHomerunChannelList();

                    using (TextWriter tw = new StreamWriter(txtOutputFile, false))
                    {
                        tw.WriteLine("#EXTM3U");
                        foreach (HdHomerunChannel channel in channeList)
                        {
                            if (doAll == true || channel.Favorite == "1")
                            {
                                channelCount++;

                                StringBuilder sb = new StringBuilder();
                                sb.Append("#EXTINF:-1 tvg-id=^" + channel.GuideName + "^ ");
                                sb.Append("tvg-name=^" + channel.GuideName + "^ ");

                                sb.Append("group-title=^HdHomerun^," + channel.GuideNumber + " " + channel.GuideName);

                                tw.WriteLine(sb.ToString().Replace('^', '"'));

                                // http://192.168.1.144:5004/auto/v8.1
                                //  string deviceURL = discovery.BaseURL.Substring(0, discovery.BaseURL.IndexOf(':', 5)) + ":5004/auto/v" + channel.GuideNumber;
                                tw.WriteLine(channel.URL);
                            }
                        }
                    }

                    Console.WriteLine("");
                    Console.WriteLine("Complete. File " + txtOutputFile + " created with " + channelCount.ToString() + " channels.");
                    Console.WriteLine("");
                }

                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception processing HdHomeRun device.");
                Console.WriteLine(e);
            }
        }

        public void DisplayHelp()
        {
            Console.WriteLine("");
            Console.WriteLine("Parameters: (Case Insensitive)");
            Console.WriteLine("");
            Console.WriteLine("-All[Optional] Put all of the HDHomerun channels in the M3U file.  Default: Only use the favorite (starred) channels.");
            Console.WriteLine("DeviceID=[Optional] The device ID of your hdmomerun device. Not needed if you only have one device on your network.");
            Console.WriteLine("Out=[Optional] The fully qualified path where the m3u file that will be created. Default: OTA.m3u in the HdHomerunM3U directory.");
            Console.WriteLine("");

            string example = @"HdHomerunM3U -DeviceID=123456 -Out=#C:\MyDirectory\dummy.m3u# ";

            Console.WriteLine("Example: " + example.Replace('#', '"'));
            Console.WriteLine("");

            Console.WriteLine("Hit enter to continue");
            Console.ReadLine();
        }
    }
}

