Github Link: https://github.com/chaycv/bebop_controll/

Documentation:
- The ROS driver need to be running to run the python script otherwise a error message is printed that the master node with "localhost:11311" cannot be found.
- The node "bebop" is added in front of the messages for our drone to run (e.g. pubControllTakeoff = rospy.Publisher("/bebop/takeoff",Empty))
- These commands can easily be adapted and modified for our needs.
- Steps to run the python script:
	- Run our test script on Ubuntu Desktop (ROS_Script) or start the ROS_Driver manually.
	- Start the Talker.py with Python2.7, Python3.5 somehow can´t run the script.
	- Fly the drone as you wish.