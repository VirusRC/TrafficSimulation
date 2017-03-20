#!/usr/bin/python
import rospy
import os
from geometry_msgs.msg import PoseStamped
import wx
from geometry_msgs.msg import Twist
from geometry_msgs.msg import Vector3
from std_msgs.msg import Empty

from std_msgs.msg import String
from Tkinter import Tk, BOTH
from ttk import Frame, Button, Style
from Tkinter import *
from DronControll import DronControll

#Controllers
pubControllTakeoff = rospy.Publisher("/bebop/takeoff",Empty)
pubControllLanding = rospy.Publisher("/bebop/land",Empty)
pubControllDron = rospy.Publisher("/bebop/cmd_vel",Twist)
pubControllCamera = rospy.Publisher("/bebop/camera_control",Twist)

#KEY
# x -> takeoff
# c -> landing
# a -> izquierda / nach links 
# d -> derecha / nach rechts
# w -> alfrente / vorne
# s -> atras / hinten
# q -> giro izq / Drehung links/rechts
# e -> giro der /Drehung links/rechts

def onKeyPress(event):
    if event.char == "x":
        pubControllTakeoff.publish(Empty())
    if event.char == "c":
        pubControllLanding.publish(Empty())
    if event.char == "a":        
        pubControllDron.publish(Twist(Vector3(0,1,0),Vector3(0,0,0)))
    if event.char == "d":
        pubControllDron.publish(Twist(Vector3(0,-1,0),Vector3(0,0,0)))
    if event.char == "w":
        pubControllDron.publish(Twist(Vector3(2,0,0),Vector3(0,0,0)))
    if event.char == "s":
        pubControllDron.publish(Twist(Vector3(-2,0,0),Vector3(0,0,0)))
    if event.char == "q":
        pubControllDron.publish(Twist(Vector3(0,0,0),Vector3(0,0,3)))
    if event.char == "e":
        pubControllDron.publish(Twist(Vector3(0,0,0),Vector3(0,0,-3)))

def talker(): 
    motion = Twist()
    rospy.init_node('talker', anonymous=True)
    rate = rospy.Rate(10) # 10hz
    root = Tk()
    root.geometry("750x300+500+-1000")
    root.bind('<Key>', onKeyPress)
    app = DronControll(root,pubControllDron,pubControllTakeoff,pubControllLanding,pubControllCamera)
    root.mainloop() 

if __name__ == '__main__':
    try:
        talker()
    except rospy.ROSInterruptException:
        pass
