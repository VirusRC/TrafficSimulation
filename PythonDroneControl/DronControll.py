import rospy
import os
from geometry_msgs.msg import PoseStamped
#from geometry_msgs.msg import Twist
import wx
from geometry_msgs.msg import Twist
from geometry_msgs.msg import Vector3
from std_msgs.msg import Empty

from std_msgs.msg import String
from Tkinter import Tk, BOTH
from ttk import Frame, Button, Style
from Tkinter import *

class DronControll(Frame):

    #Botones Despegar aterrizar
    btnDespegar = 0
    btnAterrizar = 0
    #Botones controll
    btnGiroIzquierda = 0
    btnGiroDerecha = 0
    btnAdelante = 0
    btnAtras = 0
    btnIzquierda = 0
    btnDerecha = 0
    #Botones camera
    btnTiltDownCamera = 0
    btnTiltUpCamera = 0
    btnPanLeft = 0
    btnPanRight = 0
    btnEnableCamera = 0

    #TOPICS
    pubControllDron = 0
    pubControllTakeoff = 0
    pubControllLanding = 0

    #ETIQUETAS
    etqControlDron = 0
    etqControlCamera = 0

   
    def __init__(self,parent,pubControllDron,pubControllTakeoff,pubControllLanding,pubControllCamera):
        Frame.__init__(self, parent)

        self.parent = parent
        self.initUI()

        self.pubControllDron = pubControllDron
        self.pubControllTakeoff = pubControllTakeoff
        self.pubControllLanding = pubControllLanding
        self.pubControllCamera = pubControllCamera
    
        
    def initUI(self):

        self.parent.title("Quit button")
        self.style = Style()
        self.style.theme_use("default")
        self.pack(fill=BOTH, expand=1)
        self.initButtons()

        self.etqControlDron = Label(self,text="Dron Controll")
        self.etqControlDron.place(x=20,y=10)

        self.etqControlCamera = Label(self,text="Camera Controll")
        self.etqControlCamera.place(x=370,y=10)

    def initButtons(self):

        #Controll drone

        self.btnIzquierda = Button(self,text = "Go Left",command = self.GoLeft)
        self.btnIzquierda.place(x=60,y=60)

        self.btnDerecha = Button(self,text = "Go Right",command = self.GoRight)
        self.btnDerecha.place(x=260,y=60)

        self.btnGiroIzquierda = Button(self, text="Rotate Left",command=self.Izquierda)
        self.btnGiroIzquierda.place(x=50, y=100)

        self.btnGiroDerecha = Button(self,text = "Rotate Right",command=self.Derecha);
        self.btnGiroDerecha.place(x=250, y=100);

        self.btnAdelante = Button(self,text = "Forward",command=self.Delante);
        self.btnAdelante.place(x=160,y=50);

        self.btnAtras = Button(self,text = "Backwards",command=self.Atras);
        self.btnAtras.place(x=150,y=140);

        #takeoff & landing

        self.btnDespegar = Button(self,text = "Takeoff",command=self.Takeoff);
        self.btnDespegar.place(x=50,y=220);

        self.btnAterrizar = Button(self,text = "Landing",command=self.Land);
        self.btnAterrizar.place(x=250,y=220);
        self.btnAterrizar.config(state='disabled')

        #camera_controll
        self.btnTiltUpCamera = Button(self,text="Tilt Up Camera",command=self.tiltup)
        self.btnTiltUpCamera.place(x=450,y=60);

        self.btnTiltDownCamera = Button(self,text="Tilt Down Camera",command=self.tiltdown)
        self.btnTiltDownCamera.place(x=600,y=60);

        self.btnPanRight = Button(self,text="Pan Right Camera",command=self.panRight)
        self.btnPanRight.place(x=600,y=150);

        self.btnPanLeft = Button(self,text="Pan Left Camera",command=self.panLeft)
        self.btnPanLeft.place(x=450,y=150);

    #Camera controll
    def panLeft(self):
        print("pan left")
        self.pubControllCamera.publish(Twist(Vector3(0,0,0),Vector3(0,0,-10)))

    def panRight(self):
        print("pan right")
        self.pubControllCamera.publish(Twist(Vector3(0,0,0),Vector3(0,0,10)))

    def tiltup(self):
        print("tilt up")
        self.pubControllCamera.publish(Twist(Vector3(0,0,0),Vector3(0,10,0)))

    def tiltdown(self):
        print("tilt down")
        self.pubControllCamera.publish(Twist(Vector3(0,0,0),Vector3(0,-10,0)))

    #Dron controll
    def GoLeft(self):
        print("go left")
        self.pubControllDron.publish(Twist(Vector3(0,1,0),Vector3(0,0,0)))

    def GoRight(self):
        print("go right")
        self.pubControllDron.publish(Twist(Vector3(0,-1,0),Vector3(0,0,0)))

    def Delante(self):
        print("Adelante...")
        self.pubControllDron.publish(Twist(Vector3(2,0,0),Vector3(0,0,0)))

    def Atras(self):
        print("Atras")
        self.pubControllDron.publish(Twist(Vector3(-2,0,0),Vector3(0,0,0)))

    def Derecha(self):
        print("Derecha")
        self.pubControllDron.publish(Twist(Vector3(0,0,0),Vector3(0,0,-3)))

    def Izquierda(self):
        print("Izquierda")
        self.pubControllDron.publish(Twist(Vector3(0,0,0),Vector3(0,0,3)))

    #Dron landing and takeoff
    def Land(self):
        print("LANDING...")
        self.btnAterrizar.config(state='disabled')
        self.btnDespegar.config(state='normal')
        self.pubControllLanding.publish(Empty())
        

    def Takeoff(self):
        print("Takeoff...")
        self.btnDespegar.config(state='disabled')
        self.btnAterrizar.config(state='normal')
        self.pubControllTakeoff.publish(Empty())

