using ElectronNET.API;
using ElectronNET.API.Entities;
using System.Linq;

namespace Desktop.ElectronConfig
{
    public static class ElectronMenuFactory
    {
        public static MenuItem[] CreateMenu()
        {
            return new MenuItem[] {
                new MenuItem { Label = "View", Submenu = new MenuItem[] {
                    new MenuItem
                    {
                        Label = "Reload",
                        Accelerator = "CmdOrCtrl+R",
                        Click = () =>
                        {
                            // on reload, start fresh and close any old
                            // open secondary windows
                            int mainWindowId = Electron.WindowManager.BrowserWindows.ToList().First().Id;
                            Electron.WindowManager.BrowserWindows.ToList().ForEach(browserWindow => {
                                if(browserWindow.Id != mainWindowId)
                                {
                                    browserWindow.Close();
                                }
                                else
                                {
                                    browserWindow.Reload();
                                }
                            });
                        }
                    },
                    new MenuItem
                    {
                        Label = "Toggle Full Screen",
                        Accelerator = "F11",
                        Click = async () =>
                        {
                            bool isFullScreen = await Electron.WindowManager.BrowserWindows.First().IsFullScreenAsync();
                            Electron.WindowManager.BrowserWindows.First().SetFullScreen(!isFullScreen);
                        }
                    },
                    new MenuItem
                    {
                        Label = "Open Developer Tools",
                        Accelerator = "CmdOrCtrl+Shift+I",
                        Click = () => Electron.WindowManager.BrowserWindows.First().WebContents.OpenDevTools()
                    },
                    //new MenuItem
                    //{
                    //    Label = "Inspect Element",
                    //    Accelerator = "CmdOrCtrl+Shift+C",
                    //    Click = () => Electron.WindowManager.BrowserWindows.First().WebContents.
                    //},
                    new MenuItem
                    {
                        Type = MenuType.separator
                    },
                    new MenuItem
                    {
                        Label = "App Menu Demo",
                        Click = async () => {
                            MessageBoxOptions options = new MessageBoxOptions("This demo is for the Menu section, showing how to create a clickable menu item in the application menu.");
                            options.Type = MessageBoxType.info;
                            options.Title = "Application Menu Demo";
                            await Electron.Dialog.ShowMessageBoxAsync(options);
                        }
                    }
                }
                },
                new MenuItem { Label = "Window", Role = MenuRole.window, Submenu = new MenuItem[] {
                     new MenuItem { Label = "Minimize", Accelerator = "CmdOrCtrl+M", Role = MenuRole.minimize },
                     new MenuItem { Label = "Close", Accelerator = "CmdOrCtrl+W", Role = MenuRole.close }
                }
                },
                new MenuItem { Label = "Help", Role = MenuRole.help, Submenu = new MenuItem[] {
                    new MenuItem
                    {
                        Label = "Learn More",
                        Click = async () => await Electron.Shell.OpenExternalAsync("https://github.com/ElectronNET")
                    }
                }
                }
            };
        }
    }
}
