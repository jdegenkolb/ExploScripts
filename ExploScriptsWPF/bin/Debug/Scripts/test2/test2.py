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

# PUT THE FILES YOU WANT TO COPY TO DEST IN THIS LIST AND MAKE SURE THEY ARE IN THE SCRIPT DIRECTORY.
import shutil
files = ["test.txt", "test2.txt"]

for file in files:
    print("Copying file " + file + "...")
    shutil.copyfile(src_path(file), dest_path(file))

print("Done.")
