# Install Python 3.6.3
Python has been installed as an 'executable installer' for this project
https://www.python.org/downloads/windows/

# Configure installer
  1- Choose Customize installing
  2- Checkbox only the pip option, then click Next
  3- Checkbox only the Add Python to environment variables
  4- (optional) Modify install location (For this project, python folder is
      placed near Project folder, if so please copy this path, we will need it later)
  5- install

# Install Dependencies
  1- Search 'cmd' in Start
  2- write:
    cd path\where\python\installed
  3- run:
    pip install -r path\To\requiremenst.txt
  4- Dependencies should be installing
  5- A Directory called 'Scripts' should be created
  6- Search 'System Environment Variables' in 'Start'
  7- At the bottom of windows click on 'Environment Variables'
  8- In 'User Variables' Edit 'PATH'
  9- Add Path to 'Scripts' folder in PATH:
    example:  path\where\python\installed\Scripts
  10- Save everything with ok

# Start Program
  now you are ready to go, Start the Project
