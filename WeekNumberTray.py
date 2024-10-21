from infi.systray import SysTrayIcon
from PIL import Image, ImageDraw, ImageFont
from datetime import datetime
import threading
import time
import os

# Function to create the tray icon as an image
def create_image(text, background_color, text_color):
    width, height = 64, 64
    image = Image.new('RGB', (width, height), color=background_color)  # Opaque background
    draw = ImageDraw.Draw(image)

    try:
        # Use a bold font file for better visibility
        font = ImageFont.truetype("C:\\Windows\\Fonts\\arial.ttf", 60)  # Full path to font
    except IOError:
        font = ImageFont.load_default()

    # Calculate the size of the text and position to center it
    bbox = draw.textbbox((0, 0), text, font=font)
    textwidth, textheight = bbox[2] - bbox[0], bbox[3] - bbox[1]
    x = (width - textwidth)  / 2
    y = (height - textheight) / 2

    # Draw the text with the specified color
    draw.text((x, y-8), text, font=font, fill=text_color)

    # Save the image temporarily as an ICO file
    icon_path = "week_number_icon.ico"
    image.save(icon_path, format="ICO")
    return icon_path

# Function to update the tray icon every hour
def update_icon(systray):
    while True:
        week_number = datetime.now().isocalendar()[1]
        taskbar_color = (28, 28, 28)  # Dark color for the background
        icon_path = create_image(str(week_number), taskbar_color, (255, 255, 255))  # Create the icon with week number
        systray.update(icon=icon_path)  # Update the system tray icon
        time.sleep(3600)  # Update every hour

# Function to handle quitting and cleanup
def on_quit_callback(systray):
    print("Shutting down...")
    if os.path.exists("week_number_icon.ico"):
        os.remove("week_number_icon.ico")  # Remove the temporary icon file

# Create the initial system tray icon
week_number = datetime.now().isocalendar()[1]
initial_icon_path = create_image(str(week_number), (28, 28, 28), (255, 255, 255))

# Create the system tray icon using infi.systray
systray = SysTrayIcon(initial_icon_path, "Week Number", on_quit=on_quit_callback)

# Start the update thread for the icon
thread = threading.Thread(target=update_icon, args=(systray,), daemon=True)
thread.start()

# Run the tray icon
systray.start()
