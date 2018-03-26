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
# The config.dat layout is given by the config_template.dat within the scripts directory.
# It contains placeholders for the three configuration parameters, which are replaced and then written into
# the new config.dat file.
# Make sure to use unique placeholders.

print("Creating config.dat!")
config1 = input("Value for config1: ")
config2 = input("Value for config2: ")
config3 = input("Value for config3: ")

with open(src_path("config_template.dat"), "r") as cfg_tmp:
    lines = cfg_tmp.read()
    lines = lines.replace("#cfg1", config1)
    lines = lines.replace("#cfg2", config2)
    lines = lines.replace("#cfg3", config3)

    with open(dest_path("config.dat"), "w") as cfg:
        cfg.write(lines)