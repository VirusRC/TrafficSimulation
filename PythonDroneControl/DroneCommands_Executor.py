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

client = MongoClient('193.171.53.69', 27017)
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
				except rospy.ROSInterruptException:
        				pass
				_id=currentDocument["_id"]
				collection.delete_one({'_id': ObjectId(_id)})
			else:
				print("No command available!")
			time.sleep(2)
	except KeyboardInterrupt:
		print("Closing programm!")

