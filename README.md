# ExploScripts
## Quick Overview
is a lightweight Windows explorer context menu python script manager. ExploScripts offers you simple possibility to manage the context menu of the Windows explorer and link it with customizable Python scripts.

## Installation
1. Download the latest executable.
2. Recommended: Create a ExploScripts directory, for example within your My Documents folder.
3. Run it the first time! The configuration program needs admin privileges.

## Detailed Insight
After running ExploScripts the first time, the following file structure is generated:
* ExploScripts
  * Scripts
  * Templates
  * ExploScripts.exe
  * Scripts.xml

**Scripts**
Every created context menu entry and therefore script will be stored within a subdirectory of this folder. For example the script MyScript1 will create the folder MyScript1 containing the Python script MyScript1.py. One may add additional files to this folder; whatever is needed for the script.

**Templates**
To fasten up the process of creating new scripts, templates can be used. A chosen template file will automatically be copied into the new Scripts sub-directory.

**ExploScripts.exe**
The configuration executable.

**Scripts.xml**
The local database. Storing the status, caption and name of all scripts. Running the executable will synchronize the context menu with this database file.
