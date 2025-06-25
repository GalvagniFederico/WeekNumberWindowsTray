# WeekNumberWindowsTray

A simple Windows tray application that displays the current week number in the system tray. This project is designed to provide an at-a-glance reference for the current week, featuring a configurable user interface with options for font size, text color, and automatic system theme adaptation.

![image](https://github.com/user-attachments/assets/7597cb82-5e20-4079-a880-ae1ff9aaaf2f)

## Features

- **Week Number Display**: Shows the current week number as an icon in the Windows system tray.
- **Customizable Appearance**: Adjust the font size (Small, Medium, Large) and text color to suit your preference.
  
  ![image](https://github.com/user-attachments/assets/820b0544-5cd9-4c0a-b3be-767ae8d606fb)

- **System Theme Integration**: Option to automatically match the text color with the system theme (light or dark mode).
- **Persistent Settings**: Save your preferred settings (font size and text color) for use across sessions.
- **Runs in Background**: The app runs silently in the background, without taking up taskbar space.

## How It Works

- The app calculates the current week number based on the system date using the "First Four-Day Week" rule.
- The icon is generated dynamically, with the week number drawn onto it in the specified font size and color.
- A context menu allows you to adjust settings or exit the application.

## Requirements

- Windows OS
- .NET Framework 4.7.2 or later

## Installation 
### Method 1: build your own application

1. Clone the repository:
   ```
   git clone https://github.com/yourusername/WeekNumberWindowsTray.git
   ```
2. Open the solution in Visual Studio.
3. Build and run the project.

### Method 2: install using the prebuilt installer
1. Clone the repository:
   ```
   git clone https://github.com/yourusername/WeekNumberWindowsTray.git
   ```
2. In the repository main folder open the Standalone folder
3. From here run the setup.exe to install it
4. Search the application from Windows Start Menu

## Usage

- Once the application starts, it will minimize to the system tray.
- Right-click the tray icon to access options for changing the font size, text color, or to exit the application.

### Optional
Add the application to the startup folder so you don't have to start it manually. To do that follow the next step:
1. Search WeekNumberTrayIcon in Windows Start Menu, right click the application and select Open file location
2. Now you should see an application reference, copy that file.
3. Execute ⊞ + R (Windows key + R) combination in the windows type in shell:startup
4. Paste there the application reference

Allow WeekNumberTrayIcon to be shown in the tray bar, if you don't follow this step the icon will be hidden by default
1. Right click on the task bar and select Taskbar settings
2. Now navigate to Other system tray icons
3. Find WeekNumberTrayIcon and enable it

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

