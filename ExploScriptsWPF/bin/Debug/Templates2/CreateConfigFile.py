# ExploScript Template
import sys
import os

# src: Path to the source directory (where the python script is located)
src = os.path.dirname(os.path.realpath(__file__))
# dest: Path to the destination directory (where the context menu was opened)
dest = sys.argv[1]

# Two methods to quickly get the absolute paths by just using the relative paths as parameters.
def src_path(relpath):
    return os.path.join(src, relpath)

def dest_path(relpath):
    return os.path.join(dest, relpath)

# Start your code here:

# This example creates the config file config.dat with the three user entered options config1, config2 and config3.
# The config.dat template looks like this:
# <config1> <config2>
# <config3>
# Make sure to include the line breaks (\n) in the write part!

print("Creating config.dat!")
config1 = input("Value for config1: ")
config2 = input("Value for config2: ")
config3 = input("Value for config3: ")

with open(dest_path("config.dat"), "w") as cfg:
    cfg.write(config1 + " " + config2 + "\n")
    cfg.write(config3)