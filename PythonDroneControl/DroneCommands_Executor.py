#!/usr/bin/python

import pymongo
import rospy
import os
import time

from pymongo import MongoClient
from geometry_msgs.msg import PoseStamped
from geometry_msgs.msg import Twist
from geometry_msgs.msg import Vector3
from std_msgs.msg import Empty
from std_msgs.msg import String
from bson import ObjectId

#Drone commands for takeoff and 
droneTakeoff = rospy.Publisher("/bebop/takeoff",Empty)
droneLanding = rospy.Publisher("/bebop/land",Empty)
droneControl = rospy.Publisher("/bebop/cmd_vel",Twist)

client = MongoClient('193.171.53.68', 27017)
db = client['DroneDB']
collection = db['DroneCommands']

if __name__ == '__main__':
	try:
		rospy.init_node('talker', anonymous=True)
    		rate = rospy.Rate(10) # 10hz
		while True:
			cursor = list(collection.find())
			if cursor:
				print("Executing command!")
				currentDocument = cursor[0]
				command = currentDocument["Command"]
				try:
					if command == "takeoff":
        					print("Drone takes off...") 
						droneTakeoff.publish(Empty())
   					if command == "land":
        					print("Drone is landing...") 
						droneLanding.publish(Empty())
					if command == "control":
						print("Drone is moving...")
						k = currentDocument["Velocity_y"]
						#if float(k) < 0:
						#	droneControl.publish(Twist(Vector3(0,0,-0.1),Vector3(0,0,0)))
						#if float(k) > 0:
						#	droneControl.publish(Twist(Vector3(0,0,0.1),Vector3(0,0,0)))
						velocity_y = 0.5 * float(k)
						droneControl.publish(Twist(Vector3(0,0,velocity_y),Vector3(0,0,0)))
				except rospy.ROSInterruptException:
        				pass
				_id=currentDocument["_id"]
				collection.delete_one({'_id': ObjectId(_id)})
			else:
				print("No command available!")
			#time.sleep(1)
	except KeyboardInterrupt:
		print("Closing programm!")

